USE DbQLBH;
GO

IF COL_LENGTH('dbo.SanPham', 'AnhSPData') IS NULL
    ALTER TABLE dbo.SanPham ADD AnhSPData varbinary(MAX) NULL;
IF COL_LENGTH('dbo.SanPham', 'AnhSPContentType') IS NULL
    ALTER TABLE dbo.SanPham ADD AnhSPContentType varchar(100) NULL;
IF COL_LENGTH('dbo.SanPham', 'AnhSP') IS NOT NULL
    ALTER TABLE dbo.SanPham ALTER COLUMN AnhSP nvarchar(255) NULL;

IF COL_LENGTH('dbo.NhanVien', 'AnhNVData') IS NULL
    ALTER TABLE dbo.NhanVien ADD AnhNVData varbinary(MAX) NULL;
IF COL_LENGTH('dbo.NhanVien', 'AnhNVContentType') IS NULL
    ALTER TABLE dbo.NhanVien ADD AnhNVContentType varchar(100) NULL;
IF COL_LENGTH('dbo.NhanVien', 'AnhNV') IS NOT NULL
    ALTER TABLE dbo.NhanVien ALTER COLUMN AnhNV nvarchar(255) NULL;

IF COL_LENGTH('dbo.KhachHang', 'AnhKH') IS NULL
    ALTER TABLE dbo.KhachHang ADD AnhKH nvarchar(255) NULL;
IF COL_LENGTH('dbo.KhachHang', 'AnhKHData') IS NULL
    ALTER TABLE dbo.KhachHang ADD AnhKHData varbinary(MAX) NULL;
IF COL_LENGTH('dbo.KhachHang', 'AnhKHContentType') IS NULL
    ALTER TABLE dbo.KhachHang ADD AnhKHContentType varchar(100) NULL;
GO

CREATE OR ALTER PROC SanPham_Them
    @TenSP nvarchar(50),
    @DonGia money,
    @MoTaCT ntext,
    @MaLSP smallint,
    @MaDVT tinyint,
    @AnhSP nvarchar(255) = NULL,
    @AnhSPData varbinary(MAX) = NULL,
    @AnhSPContentType varchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaSP char(9);
    DECLARE @MaSo bigint;
    SET @MaSP = FORMAT(GETDATE(), 'yyMMdd') + '001';

    WHILE @MaSP IN (SELECT MaSP FROM dbo.SanPham)
    BEGIN
        SET @MaSo = CAST(@MaSP AS bigint) + 1;
        SET @MaSP = CAST(@MaSo AS char(9));
    END;

    INSERT INTO dbo.SanPham
        (MaSP, TenSP, AnhSP, AnhSPData, AnhSPContentType, DonGia, MoTaCT, MaLSP, MaDVT)
    VALUES
        (@MaSP, @TenSP, @AnhSP, @AnhSPData, @AnhSPContentType, @DonGia, @MoTaCT, @MaLSP, @MaDVT);
END
GO

CREATE OR ALTER PROC NhanVien_Them
    @HoNV nvarchar(30),
    @TenNV nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @TenDN nvarchar(50),
    @MatKhau varbinary(MAX),
    @MaPB tinyint,
    @AnhNV nvarchar(255) = NULL,
    @AnhNVData varbinary(MAX) = NULL,
    @AnhNVContentType varchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaNV int;
    SET @MaNV = 1;

    WHILE @MaNV IN (SELECT MaNV FROM dbo.NhanVien)
        SET @MaNV = @MaNV + 1;

    INSERT INTO dbo.NhanVien
        (MaNV, HoNV, TenNV, DienThoai, Email, TenDN, MatKhau,
         AnhNV, AnhNVData, AnhNVContentType, MaPB)
    VALUES
        (@MaNV, @HoNV, @TenNV, @DienThoai, @Email, @TenDN, @MatKhau,
         @AnhNV, @AnhNVData, @AnhNVContentType, @MaPB);
END
GO

CREATE OR ALTER PROC KhachHang_Them
    @HoKH nvarchar(30),
    @TenKH nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5),
    @TenDN nvarchar(50),
    @MatKhau varbinary(MAX),
    @AnhKH nvarchar(255) = NULL,
    @AnhKHData varbinary(MAX) = NULL,
    @AnhKHContentType varchar(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaKH int;
    SET @MaKH = 1;

    WHILE @MaKH IN (SELECT MaKH FROM dbo.KhachHang)
        SET @MaKH = @MaKH + 1;

    INSERT INTO dbo.KhachHang
        (MaKH, HoKH, TenKH, DienThoai, Email, DiaChi, MaXP, TenDN, MatKhau,
         AnhKH, AnhKHData, AnhKHContentType)
    VALUES
        (@MaKH, @HoKH, @TenKH, @DienThoai, @Email, @DiaChi, @MaXP, @TenDN, @MatKhau,
         @AnhKH, @AnhKHData, @AnhKHContentType);
END
GO

CREATE OR ALTER PROC SanPham_Sua
    @MaSP char(9),
    @TenSP nvarchar(50),
    @DonGia money,
    @MoTaCT ntext,
    @MaLSP smallint,
    @MaDVT tinyint,
    @AnhSP nvarchar(255) = NULL,
    @AnhSPData varbinary(MAX) = NULL,
    @AnhSPContentType varchar(100) = NULL,
    @XoaAnh bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.SanPham
    SET TenSP = @TenSP,
        DonGia = @DonGia,
        AnhSP = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhSPData IS NOT NULL THEN @AnhSP ELSE AnhSP END,
        AnhSPData = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhSPData IS NOT NULL THEN @AnhSPData ELSE AnhSPData END,
        AnhSPContentType = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhSPData IS NOT NULL THEN @AnhSPContentType ELSE AnhSPContentType END,
        MoTaCT = @MoTaCT,
        MaLSP = @MaLSP,
        MaDVT = @MaDVT
    WHERE MaSP = @MaSP;
END
GO

CREATE OR ALTER PROC dbo.NhanVien_Sua
    @MaNV tinyint,
    @HoNV nvarchar(30),
    @TenNV nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @TenDN nvarchar(50),
    @MaPB tinyint,
    @AnhNV nvarchar(255) = NULL,
    @AnhNVData varbinary(MAX) = NULL,
    @AnhNVContentType varchar(100) = NULL,
    @XoaAnh bit = 0,
    @MatKhauMoi nvarchar(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.NhanVien
    SET HoNV = @HoNV,
        TenNV = @TenNV,
        DienThoai = @DienThoai,
        Email = @Email,
        TenDN = @TenDN,
        AnhNV = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhNVData IS NOT NULL THEN @AnhNV ELSE AnhNV END,
        AnhNVData = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhNVData IS NOT NULL THEN @AnhNVData ELSE AnhNVData END,
        AnhNVContentType = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhNVData IS NOT NULL THEN @AnhNVContentType ELSE AnhNVContentType END,
        MaPB = @MaPB,
        MatKhau = CASE
            WHEN @MatKhauMoi IS NULL OR DATALENGTH(@MatKhauMoi) = 0 THEN MatKhau
            ELSE HASHBYTES('SHA2_512', @MatKhauMoi)
        END
    WHERE MaNV = @MaNV;
END
GO

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
    @AnhKH nvarchar(255) = NULL,
    @AnhKHData varbinary(MAX) = NULL,
    @AnhKHContentType varchar(100) = NULL,
    @XoaAnh bit = 0
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.KhachHang
    SET HoKH = @HoKH,
        TenKH = @TenKH,
        DienThoai = @DienThoai,
        Email = @Email,
        DiaChi = @DiaChi,
        MaXP = @MaXP,
        TenDN = @TenDN,
        AnhKH = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhKHData IS NOT NULL THEN @AnhKH ELSE AnhKH END,
        AnhKHData = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhKHData IS NOT NULL THEN @AnhKHData ELSE AnhKHData END,
        AnhKHContentType = CASE WHEN @XoaAnh = 1 THEN NULL WHEN @AnhKHData IS NOT NULL THEN @AnhKHContentType ELSE AnhKHContentType END,
        MatKhau = CASE
            WHEN @MatKhauMoi IS NULL OR DATALENGTH(@matkhaumoi) = 0 THEN MatKhau
            ELSE HASHBYTES('SHA2_512', @matkhaumoi)
        END
    WHERE MaKH = @MaKH;
END
GO
