USE DbQLBH;
GO

CREATE OR ALTER PROC dbo.GioHang_DS
    @MaKH smallint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT sp.MaSP, sp.TenSP, sp.AnhSP, sp.DonGia,
           cgh.SoLuong, CAST(sp.DonGia * cgh.SoLuong AS money) AS ThanhTien
    FROM dbo.GioHang gh
    INNER JOIN dbo.CTGioHang cgh ON cgh.MaGH = gh.MaGH
    INNER JOIN dbo.SanPham sp ON sp.MaSP = cgh.MaSP
    WHERE gh.MaKH = @MaKH
    ORDER BY sp.TenSP;
END
GO

CREATE OR ALTER PROC dbo.GioHang_Them
    @MaKH smallint,
    @MaSP char(9),
    @SoLuong smallint
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @SoLuong < 1 OR @SoLuong > 255
        THROW 50001, N'Số lượng phải từ 1 đến 255.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.SanPham WHERE MaSP = @MaSP)
        THROW 50002, N'Sản phẩm không tồn tại.', 1;

    BEGIN TRANSACTION;
    DECLARE @MaGH int;

    SELECT @MaGH = MaGH
    FROM dbo.GioHang WITH (UPDLOCK, HOLDLOCK)
    WHERE MaKH = @MaKH;

    IF @MaGH IS NULL
    BEGIN
        INSERT INTO dbo.GioHang (MaKH) VALUES (@MaKH);
        SET @MaGH = CONVERT(int, SCOPE_IDENTITY());
    END;

    IF EXISTS (SELECT 1 FROM dbo.CTGioHang WHERE MaGH = @MaGH AND MaSP = @MaSP)
    BEGIN
        IF EXISTS
        (
            SELECT 1 FROM dbo.CTGioHang
            WHERE MaGH = @MaGH AND MaSP = @MaSP
              AND SoLuong + @SoLuong > 255
        )
            THROW 50003, N'Tổng số lượng sản phẩm trong giỏ không được vượt quá 255.', 1;

        UPDATE dbo.CTGioHang
        SET SoLuong = SoLuong + @SoLuong
        WHERE MaGH = @MaGH AND MaSP = @MaSP;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.CTGioHang (MaGH, MaSP, SoLuong)
        VALUES (@MaGH, @MaSP, @SoLuong);
    END;

    UPDATE dbo.GioHang SET NgayCapNhat = SYSDATETIME() WHERE MaGH = @MaGH;
    COMMIT TRANSACTION;
END
GO

CREATE OR ALTER PROC dbo.GioHang_CapNhat
    @MaKH smallint,
    @MaSP char(9),
    @SoLuong smallint
AS
BEGIN
    SET NOCOUNT ON;

    IF @SoLuong < 1 OR @SoLuong > 255
        THROW 50004, N'Số lượng phải từ 1 đến 255.', 1;

    UPDATE cgh
    SET SoLuong = @SoLuong
    FROM dbo.CTGioHang cgh
    INNER JOIN dbo.GioHang gh ON gh.MaGH = cgh.MaGH
    WHERE gh.MaKH = @MaKH AND cgh.MaSP = @MaSP;

    IF @@ROWCOUNT = 0
        THROW 50005, N'Sản phẩm không còn trong giỏ hàng.', 1;

    UPDATE gh
    SET NgayCapNhat = SYSDATETIME()
    FROM dbo.GioHang gh
    WHERE gh.MaKH = @MaKH;
END
GO

CREATE OR ALTER PROC dbo.GioHang_Xoa
    @MaKH smallint,
    @MaSP char(9)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE cgh
    FROM dbo.CTGioHang cgh
    INNER JOIN dbo.GioHang gh ON gh.MaGH = cgh.MaGH
    WHERE gh.MaKH = @MaKH AND cgh.MaSP = @MaSP;

    UPDATE dbo.GioHang SET NgayCapNhat = SYSDATETIME() WHERE MaKH = @MaKH;
END
GO

CREATE OR ALTER PROC dbo.GioHang_DatHang
    @MaKH smallint
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @MaGH int, @DiaChi nvarchar(50), @MaXP char(5);
        DECLARE @MaDBH char(11), @MaSo int = 1, @TienTo char(8);
        DECLARE @NgayDat smalldatetime = GETDATE();

        SELECT @DiaChi = DiaChi, @MaXP = MaXP
        FROM dbo.KhachHang WITH (UPDLOCK, HOLDLOCK)
        WHERE MaKH = @MaKH;

        IF @DiaChi IS NULL OR @MaXP IS NULL
            THROW 50006, N'Không tìm thấy thông tin khách hàng.', 1;

        SELECT @MaGH = MaGH
        FROM dbo.GioHang WITH (UPDLOCK, HOLDLOCK)
        WHERE MaKH = @MaKH;

        IF @MaGH IS NULL OR NOT EXISTS (SELECT 1 FROM dbo.CTGioHang WHERE MaGH = @MaGH)
            THROW 50007, N'Giỏ hàng đang trống.', 1;

        SET @TienTo = 'BH' + CONVERT(char(6), @NgayDat, 12);
        SET @MaDBH = @TienTo + RIGHT('000' + CONVERT(varchar(3), @MaSo), 3);

        WHILE EXISTS (SELECT 1 FROM dbo.DonBH WHERE MaDBH = @MaDBH)
        BEGIN
            SET @MaSo += 1;
            IF @MaSo > 999
                THROW 50008, N'Đã hết số thứ tự đơn bán hàng trong ngày.', 1;

            SET @MaDBH = @TienTo + RIGHT('000' + CONVERT(varchar(3), @MaSo), 3);
        END;

        INSERT INTO dbo.DonBH
            (MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH)
        VALUES
            (@MaDBH, @NgayDat, NULL, @DiaChi, @MaXP, NULL, @MaKH);

        INSERT INTO dbo.CTBH (MaDBH, MaSP, SLB, DGB)
        SELECT @MaDBH, cgh.MaSP, CONVERT(tinyint, cgh.SoLuong), sp.DonGia
        FROM dbo.CTGioHang cgh
        INNER JOIN dbo.SanPham sp ON sp.MaSP = cgh.MaSP
        WHERE cgh.MaGH = @MaGH;

        DELETE FROM dbo.CTGioHang WHERE MaGH = @MaGH;

        COMMIT TRANSACTION;
        SELECT @MaDBH AS MaDBH;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
