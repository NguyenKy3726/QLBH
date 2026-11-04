-- 1. Cập nhật bảng Đơn vị tính
CREATE OR ALTER PROC DonViTinh_Sua
    @MaDVT tinyint,
    @TenDVT nvarchar(20)
AS
    UPDATE DonViTinh
    SET TenDVT = @TenDVT
    WHERE MaDVT = @MaDVT
GO

-- 2. Cập nhật bảng Nhóm sản phẩm
CREATE OR ALTER PROC NhomSP_Sua
    @MaNSP tinyint,
    @TenNSP nvarchar(45)
AS
    UPDATE NhomSP
    SET TenNSP = @TenNSP
    WHERE MaNSP = @MaNSP
GO

-- 3. Cập nhật bảng Loại sản phẩm
CREATE OR ALTER PROC LoaiSP_Sua
    @MaLSP smallint,
    @TenLSP nvarchar(50),
    @MaNSP tinyint
AS
    UPDATE LoaiSP
    SET TenLSP = @TenLSP,
        MaNSP = @MaNSP
    WHERE MaLSP = @MaLSP
GO

-- 4. Cập nhật bảng Sản phẩm
CREATE OR ALTER PROC SanPham_Sua
    @MaSP char(9),
    @TenSP nvarchar(50),
    @DonGia money,
    @AnhSP nvarchar(100),
    @MoTaCT ntext,
    @MaLSP smallint,
    @MaDVT tinyint
AS
    UPDATE SanPham
    SET TenSP = @TenSP,
        DonGia = @DonGia,
        AnhSP = @AnhSP,
        MoTaCT = @MoTaCT,
        MaLSP = @MaLSP,
        MaDVT = @MaDVT
    WHERE MaSP = @MaSP
GO

-- 5. Cập nhật bảng Tỉnh
CREATE OR ALTER PROC Tinh_Sua
    @MaTinh char(2),
    @TenTinh nvarchar(50)
AS
    UPDATE Tinh
    SET TenTinh = @TenTinh
    WHERE MaTinh = @MaTinh
GO

-- 6. Cập nhật bảng Xã/phường
CREATE OR ALTER PROC XaPhuong_Sua
    @MaXP char(5),
    @TenXP nvarchar(50),
    @MaTinh char(2)
AS
    UPDATE XaPhuong
    SET TenXP = @TenXP,
        MaTinh = @MaTinh
    WHERE MaXP = @MaXP
GO

-- 7. Cập nhật bảng Phòng ban
CREATE OR ALTER PROC PhongBan_Sua
    @MaPB tinyint,
    @TenPB nvarchar(40)
AS
    UPDATE PhongBan
    SET TenPB = @TenPB
    WHERE MaPB = @MaPB
GO

-- 8. Cập nhật bảng Quyền
CREATE OR ALTER PROC Quyen_Sua
    @MaQuyen tinyint,
    @TenQuyen nvarchar(50),
    @MoTa ntext
AS
    UPDATE Quyen
    SET TenQuyen = @TenQuyen,
        MoTa = @MoTa
    WHERE MaQuyen = @MaQuyen
GO

-- 9. Cập nhật bảng Nhân viên
CREATE OR ALTER PROC dbo.NhanVien_Sua
    @MaNV tinyint,
    @HoNV nvarchar(30),
    @TenNV nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @TenDN nvarchar(50),
    @AnhNV nvarchar(250),
    @MaPB tinyint,
    @MatKhauMoi nvarchar(50) = NULL
AS
    UPDATE dbo.NhanVien
    SET HoNV = @HoNV,
        TenNV = @TenNV,
        DienThoai = @DienThoai,
        Email = @Email,
        TenDN = @TenDN,
        AnhNV = @AnhNV,
        MaPB = @MaPB,
        MatKhau = CASE
            WHEN @MatKhauMoi IS NULL
                 OR DATALENGTH(@MatKhauMoi) = 0
                THEN MatKhau
            ELSE HASHBYTES('SHA2_512', @MatKhauMoi)
        END
    WHERE MaNV = @MaNV;
GO

-- 10. Bảng Phân quyền
-- Chỉ có hai cột MaQuyen và MaNV, đều thuộc khóa chính.
-- Không tạo proc sửa vì không có thuộc tính ngoài khóa để cập nhật.

-- 11. Cập nhật bảng Nhà cung cấp
CREATE OR ALTER PROC NhaCC_Sua
    @MaNCC smallint,
    @TenNCC nvarchar(50),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5)
AS
    UPDATE NhaCC
    SET TenNCC = @TenNCC,
        DienThoai = @DienThoai,
        Email = @Email,
        DiaChi = @DiaChi,
        MaXP = @MaXP
    WHERE MaNCC = @MaNCC
GO

-- 12. Cập nhật bảng Đơn mua hàng
CREATE OR ALTER PROC DonMH_Sua
    @MaDMH char(11),
    @NgayDat smalldatetime,
    @NgayGiao smalldatetime,
    @MaNV tinyint,
    @MaNCC smallint
AS
    UPDATE DonMH
    SET NgayDat = @NgayDat,
        NgayGiao = @NgayGiao,
        MaNV = @MaNV,
        MaNCC = @MaNCC
    WHERE MaDMH = @MaDMH
GO

-- 13. Cập nhật bảng Chi tiết mua hàng
CREATE OR ALTER PROC CTMH_Sua
    @MaDMH char(11),
    @MaSP char(9),
    @SLM smallint,
    @DGM money
AS
    UPDATE CTMH
    SET SLM = @SLM,
        DGM = @DGM
    WHERE MaDMH = @MaDMH
      AND MaSP = @MaSP
GO

-- 14. Cập nhật bảng Khách hàng
CREATE OR ALTER PROC dbo.KhachHang_Sua
    @MaKH smallint,
    @HoKH nvarchar(30),
    @TenKH nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5),
    @TenDN nvarchar(50),
    @matkhaumoi nvarchar(50) = NULL,
    @AnhKH nvarchar(255) = NULL
AS
    UPDATE dbo.KhachHang
    SET HoKH = @HoKH,
        TenKH = @TenKH,
        DienThoai = @DienThoai,
        Email = @Email,
        DiaChi = @DiaChi,
        MaXP = @MaXP,
        TenDN = @TenDN,
        AnhKH = @AnhKH,
        MatKhau = CASE
            WHEN @MatKhauMoi IS NULL
                 OR DATALENGTH(@matkhaumoi) = 0
                THEN MatKhau
            ELSE HASHBYTES('SHA2_512', @matkhaumoi)
        END
    WHERE MaKH = @MaKH;
GO

-- 15. Cập nhật thông tin đơn bán hàng.
-- Không cho sửa dữ liệu đơn sau khi đã xác nhận; thay đổi trạng thái dùng proc riêng.
CREATE OR ALTER PROC dbo.DonBH_Sua
    @MaDBH char(11),
    @NgayDat smalldatetime,
    @NgayGiao smalldatetime,
    @DCGH nvarchar(50),
    @MaXP char(5),
    @MaNV tinyint = NULL,
    @MaKH smallint
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.DonBH WHERE MaDBH = @MaDBH)
        THROW 50011, N'Không tìm thấy đơn bán hàng.', 1;

    IF EXISTS
    (
        SELECT 1 FROM dbo.DonBH
        WHERE MaDBH = @MaDBH AND TrangThai <> 'CHO_XAC_NHAN'
    )
        THROW 50012, N'Chỉ được sửa thông tin khi đơn còn chờ xác nhận.', 1;

    UPDATE dbo.DonBH
    SET NgayDat = @NgayDat,
        NgayGiao = @NgayGiao,
        DCGH = @DCGH,
        MaXP = @MaXP,
        MaNV = @MaNV,
        MaKH = @MaKH
    WHERE MaDBH = @MaDBH;
END
GO

-- Chỉ cho phép chuyển từng bước hợp lệ; cập nhật đơn và ghi lịch sử nguyên tử.
CREATE OR ALTER PROC dbo.DonBH_ChuyenTrangThai
    @MaDBH char(11),
    @TrangThaiMoi varchar(20),
    @MaNV tinyint,
    @GhiChu nvarchar(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @TrangThaiCu varchar(20);

        SELECT @TrangThaiCu = TrangThai
        FROM dbo.DonBH WITH (UPDLOCK, ROWLOCK)
        WHERE MaDBH = @MaDBH;

        IF @TrangThaiCu IS NULL
            THROW 50013, N'Không tìm thấy đơn bán hàng.', 1;

        IF @TrangThaiMoi NOT IN
            ('CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DANG_CHUAN_BI',
             'DANG_GIAO', 'HOAN_TAT', 'DA_HUY')
            THROW 50014, N'Trạng thái yêu cầu không hợp lệ.', 1;

        IF NOT
        (
            (@TrangThaiCu = 'CHO_XAC_NHAN'
                AND @TrangThaiMoi IN ('DA_XAC_NHAN', 'DA_HUY'))
            OR (@TrangThaiCu = 'DA_XAC_NHAN'
                AND @TrangThaiMoi IN ('DANG_CHUAN_BI', 'DA_HUY'))
            OR (@TrangThaiCu = 'DANG_CHUAN_BI'
                AND @TrangThaiMoi IN ('DANG_GIAO', 'DA_HUY'))
            OR (@TrangThaiCu = 'DANG_GIAO'
                AND @TrangThaiMoi = 'HOAN_TAT')
        )
            THROW 50015, N'Không thể chuyển đơn từ trạng thái hiện tại sang trạng thái này.', 1;

        IF @TrangThaiMoi = 'DA_HUY' AND NULLIF(LTRIM(RTRIM(@GhiChu)), N'') IS NULL
            THROW 50016, N'Vui lòng nhập lý do hủy đơn.', 1;

        UPDATE dbo.DonBH
        SET TrangThai = @TrangThaiMoi,
            MaNV = COALESCE(MaNV, @MaNV),
            NgayGiao = CASE
                WHEN @TrangThaiMoi = 'HOAN_TAT'
                    THEN COALESCE(NgayGiao, CONVERT(smalldatetime, GETDATE()))
                ELSE NgayGiao
            END
        WHERE MaDBH = @MaDBH;

        INSERT INTO dbo.LichSuTrangThaiDonBH
            (MaDBH, TrangThaiCu, TrangThaiMoi, ThoiDiem, MaNV, GhiChu)
        VALUES
            (@MaDBH, @TrangThaiCu, @TrangThaiMoi, SYSDATETIME(), @MaNV,
             NULLIF(LTRIM(RTRIM(@GhiChu)), N''));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- 16. Cập nhật bảng Chi tiết bán hàng
CREATE OR ALTER PROC dbo.CTBH_Sua
    @MaDBH char(11),
    @MaSP char(9),
    @SLB tinyint,
    @DGB money
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1 FROM dbo.DonBH
        WHERE MaDBH = @MaDBH AND TrangThai = 'CHO_XAC_NHAN'
    )
        THROW 50032, N'Chỉ được sửa chi tiết khi đơn còn chờ xác nhận.', 1;

    UPDATE dbo.CTBH
    SET SLB = @SLB,
        DGB = @DGB
    WHERE MaDBH = @MaDBH
      AND MaSP = @MaSP;
END
GO
