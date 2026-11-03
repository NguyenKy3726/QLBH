USE DbQLBH
GO

-- 1. Đơn vị tính
CREATE OR ALTER PROC DonViTinh_Them
    @TenDVT nvarchar(20)
AS
BEGIN
    DECLARE @MaDVT int
    SET @MaDVT = 1

    WHILE @MaDVT IN (SELECT MaDVT FROM DonViTinh)
    BEGIN
        SET @MaDVT = @MaDVT + 1
    END

    INSERT INTO DonViTinh(MaDVT, TenDVT)
    VALUES(@MaDVT, @TenDVT)
END
GO


-- 2. Nhóm sản phẩm
CREATE OR ALTER PROC NhomSP_Them
    @TenNSP nvarchar(45)
AS
BEGIN
    DECLARE @MaNSP int
    SET @MaNSP = 1

    WHILE @MaNSP IN (SELECT MaNSP FROM NhomSP)
    BEGIN
        SET @MaNSP = @MaNSP + 1
    END

    INSERT INTO NhomSP(MaNSP, TenNSP)
    VALUES(@MaNSP, @TenNSP)
END
GO


-- 3. Loại sản phẩm
CREATE OR ALTER PROC LoaiSP_Them
    @TenLSP nvarchar(50),
    @MaNSP tinyint
AS
BEGIN
    DECLARE @MaLSP int
    SET @MaLSP = 1

    WHILE @MaLSP IN (SELECT MaLSP FROM LoaiSP)
    BEGIN
        SET @MaLSP = @MaLSP + 1
    END

    INSERT INTO LoaiSP(MaLSP, TenLSP, MaNSP)
    VALUES(@MaLSP, @TenLSP, @MaNSP)
END
GO


-- 4. Sản phẩm
-- Mã gồm yyMMdd + số thứ tự 001, 002...
CREATE OR ALTER PROC SanPham_Them
    @TenSP nvarchar(50),
    @AnhSP nvarchar(100),
    @DonGia money,
    @MoTaCT ntext,
    @MaLSP smallint,
    @MaDVT tinyint
AS
BEGIN
    DECLARE @MaSP char(9)
    DECLARE @MaSo bigint

    SET @MaSP = FORMAT(GETDATE(), 'yyMMdd') + '001'

    WHILE @MaSP IN (SELECT MaSP FROM SanPham)
    BEGIN
        SET @MaSo = CAST(@MaSP AS bigint) + 1
        SET @MaSP = CAST(@MaSo AS char(9))
    END

    INSERT INTO SanPham
        (MaSP, TenSP, AnhSP, DonGia, MoTaCT, MaLSP, MaDVT)
    VALUES
        (@MaSP, @TenSP, @AnhSP, @DonGia, @MoTaCT, @MaLSP, @MaDVT)
END
GO


-- 5. Tỉnh
-- Mã 01, 02, 03...
CREATE OR ALTER PROC Tinh_Them
    @TenTinh nvarchar(50)
AS
BEGIN
    DECLARE @MaTinh char(2)
    DECLARE @MaSo int

    SET @MaTinh = '01'

    WHILE @MaTinh IN (SELECT MaTinh FROM Tinh)
    BEGIN
        SET @MaSo = CAST(@MaTinh AS int) + 1
        SET @MaTinh = FORMAT(@MaSo, '00')
    END

    INSERT INTO Tinh(MaTinh, TenTinh)
    VALUES(@MaTinh, @TenTinh)
END
GO


-- 6. Xã/phường
-- Mã tỉnh + số thứ tự 001, 002...
CREATE OR ALTER PROC XaPhuong_Them
    @TenXP nvarchar(50),
    @MaTinh char(2)
AS
BEGIN
    DECLARE @MaXP char(5)
    DECLARE @MaSo int

    SET @MaXP = @MaTinh + '001'

    WHILE @MaXP IN (SELECT MaXP FROM XaPhuong)
    BEGIN
        SET @MaSo = CAST(@MaXP AS int) + 1
        SET @MaXP = FORMAT(@MaSo, '00000')
    END

    INSERT INTO XaPhuong(MaXP, TenXP, MaTinh)
    VALUES(@MaXP, @TenXP, @MaTinh)
END
GO


-- 7. Phòng ban
CREATE OR ALTER PROC PhongBan_Them
    @TenPB nvarchar(40)
AS
BEGIN
    DECLARE @MaPB int
    SET @MaPB = 1

    WHILE @MaPB IN (SELECT MaPB FROM PhongBan)
    BEGIN
        SET @MaPB = @MaPB + 1
    END

    INSERT INTO PhongBan(MaPB, TenPB)
    VALUES(@MaPB, @TenPB)
END
GO


-- 8. Quyền
CREATE OR ALTER PROC Quyen_Them
    @TenQuyen nvarchar(50),
    @MoTa ntext
AS
BEGIN
    DECLARE @MaQuyen int
    SET @MaQuyen = 1

    WHILE @MaQuyen IN (SELECT MaQuyen FROM Quyen)
    BEGIN
        SET @MaQuyen = @MaQuyen + 1
    END

    INSERT INTO Quyen(MaQuyen, TenQuyen, MoTa)
    VALUES(@MaQuyen, @TenQuyen, @MoTa)
END
GO


-- 9. Nhân viên
CREATE OR ALTER PROC NhanVien_Them
    @HoNV nvarchar(30),
    @TenNV nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @TenDN nvarchar(50),
    @MatKhau varbinary(MAX),
    @AnhNV nvarchar(250),
    @MaPB tinyint
AS
BEGIN
    DECLARE @MaNV int
    SET @MaNV = 1

    WHILE @MaNV IN (SELECT MaNV FROM NhanVien)
    BEGIN
        SET @MaNV = @MaNV + 1
    END

    INSERT INTO NhanVien
        (MaNV, HoNV, TenNV, DienThoai, Email,
         TenDN, MatKhau, AnhNV, MaPB)
    VALUES
        (@MaNV, @HoNV, @TenNV, @DienThoai, @Email,
         @TenDN, @MatKhau, @AnhNV, @MaPB)
END
GO


-- 10. Phân quyền
-- Hai mã được chọn từ bảng Quyen và NhanVien.
CREATE OR ALTER PROC PhanQuyen_Them
    @MaQuyen tinyint,
    @MaNV tinyint
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO PhanQuyen(MaQuyen, MaNV)
        VALUES(@MaQuyen, @MaNV)
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2627, 2601)
            THROW 50000, N'Nhân viên này đã được gán quyền này. Không thể tạo trùng cặp quyền và nhân viên.', 1;

        THROW;
    END CATCH
END
GO


-- 11. Nhà cung cấp
CREATE OR ALTER PROC NhaCC_Them
    @TenNCC nvarchar(50),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5)
AS
BEGIN
    DECLARE @MaNCC int
    SET @MaNCC = 1

    WHILE @MaNCC IN (SELECT MaNCC FROM NhaCC)
    BEGIN
        SET @MaNCC = @MaNCC + 1
    END

    INSERT INTO NhaCC
        (MaNCC, TenNCC, DienThoai, Email, DiaChi, MaXP)
    VALUES
        (@MaNCC, @TenNCC, @DienThoai, @Email, @DiaChi, @MaXP)
END
GO


-- 12. Đơn mua hàng
-- Mã MH00001, MH00002...
CREATE OR ALTER PROC DonMH_Them
    @NgayDat smalldatetime,
    @NgayGiao smalldatetime,
    @MaNV tinyint,
    @MaNCC smallint
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaDMH char(11);
    DECLARE @MaSo int = 1;
    DECLARE @Ngay char(6);
    DECLARE @TienTo char(8);

    SET @Ngay = CONVERT(char(6), @NgayDat, 12); -- yymmdd
    SET @TienTo = 'MH' + @Ngay;

    SET @MaDMH = @TienTo
               + RIGHT('000' + CAST(@MaSo AS varchar(3)), 3);

    WHILE EXISTS (
        SELECT 1
        FROM DonMH
        WHERE MaDMH = @MaDMH
    )
    BEGIN
        SET @MaSo = @MaSo + 1;

        IF @MaSo > 999
        BEGIN
            THROW 50001, N'Đã hết số thứ tự đơn mua hàng trong ngày.', 1;
        END;

        SET @MaDMH = @TienTo
                   + RIGHT('000' + CAST(@MaSo AS varchar(3)), 3);
    END;

    INSERT INTO DonMH (MaDMH, NgayDat, NgayGiao, MaNV, MaNCC)
    VALUES (@MaDMH, @NgayDat, @NgayGiao, @MaNV, @MaNCC);
END
GO


-- 13. Chi tiết mua hàng
-- Hai mã được chọn từ bảng DonMH và SanPham.
CREATE OR ALTER PROC CTMH_Them
    @MaDMH char(11),
    @MaSP char(9),
    @SLM smallint,
    @DGM money
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO CTMH(MaDMH, MaSP, SLM, DGM)
        VALUES(@MaDMH, @MaSP, @SLM, @DGM)
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2627, 2601)
            THROW 50000, N'Chi tiết mua hàng này đã tồn tại. Mỗi mã đơn mua hàng chỉ được có một dòng cho cùng một sản phẩm.', 1;

        THROW;
    END CATCH
END
GO


-- 14. Khách hàng
CREATE OR ALTER PROC KhachHang_Them
    @HoKH nvarchar(30),
    @TenKH nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5),
    @TenDN nvarchar(50),
    @MatKhau varbinary(MAX)
AS
BEGIN
    DECLARE @MaKH int
    SET @MaKH = 1

    WHILE @MaKH IN (SELECT MaKH FROM KhachHang)
    BEGIN
        SET @MaKH = @MaKH + 1
    END

    INSERT INTO KhachHang
        (MaKH, HoKH, TenKH, DienThoai, Email,
         DiaChi, MaXP, TenDN, MatKhau)
    VALUES
        (@MaKH, @HoKH, @TenKH, @DienThoai, @Email,
         @DiaChi, @MaXP, @TenDN, @MatKhau)
END
GO


-- 15. Đơn bán hàng
-- Mã BH0000001, BH0000002...
CREATE OR ALTER PROC DonBH_Them
    @NgayDat smalldatetime,
    @NgayGiao smalldatetime,
    @DCGH nvarchar(50),
    @MaXP char(5),
    @MaNV tinyint,
    @MaKH smallint
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaDBH char(11);
    DECLARE @MaSo int = 1;
    DECLARE @Ngay char(6);
    DECLARE @TienTo char(8);

    SET @Ngay = CONVERT(char(6), @NgayDat, 12); -- yymmdd
    SET @TienTo = 'BH' + @Ngay;

    SET @MaDBH = @TienTo
               + RIGHT('000' + CAST(@MaSo AS varchar(3)), 3);

    WHILE EXISTS (
        SELECT 1
        FROM DonBH
        WHERE MaDBH = @MaDBH
    )
    BEGIN
        SET @MaSo = @MaSo + 1;

        IF @MaSo > 999
        BEGIN
            THROW 50001, N'Đã hết số thứ tự đơn bán hàng trong ngày.', 1;
        END;

        SET @MaDBH = @TienTo
                   + RIGHT('000' + CAST(@MaSo AS varchar(3)), 3);
    END;

    INSERT INTO DonBH
        (MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH)
    VALUES
        (@MaDBH, @NgayDat, @NgayGiao, @DCGH, @MaXP, @MaNV, @MaKH);
END
GO


-- 16. Chi tiết bán hàng
-- Hai mã được chọn từ bảng DonBH và SanPham.
CREATE OR ALTER PROC CTBH_Them
    @MaDBH char(11),
    @MaSP char(9),
    @SLB tinyint,
    @DGB money
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO CTBH(MaDBH, MaSP, SLB, DGB)
        VALUES(@MaDBH, @MaSP, @SLB, @DGB)
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2627, 2601)
            THROW 50000, N'Chi tiết bán hàng này đã tồn tại. Mỗi mã đơn bán hàng chỉ được có một dòng cho cùng một sản phẩm.', 1;

        THROW;
    END CATCH
END
GO