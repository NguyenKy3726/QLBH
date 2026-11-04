USE DbQLBH;
GO

CREATE OR ALTER PROC dbo.KhachHang_Them
    @HoKH nvarchar(30),
    @TenKH nvarchar(20),
    @DienThoai varchar(11),
    @Email varchar(40),
    @DiaChi nvarchar(50),
    @MaXP char(5),
    @TenDN nvarchar(50),
    @MatKhau varbinary(MAX),
    @AnhKH nvarchar(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaKH int = 1;
    WHILE @MaKH IN (SELECT MaKH FROM dbo.KhachHang)
        SET @MaKH += 1;

    INSERT INTO dbo.KhachHang
        (MaKH, HoKH, TenKH, DienThoai, Email, DiaChi, MaXP, TenDN, MatKhau, AnhKH)
    VALUES
        (@MaKH, @HoKH, @TenKH, @DienThoai, @Email, @DiaChi, @MaXP, @TenDN, @MatKhau, @AnhKH);
END;
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
    @AnhKH nvarchar(255) = NULL
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
        AnhKH = @AnhKH,
        MatKhau = CASE
            WHEN @matkhaumoi IS NULL OR DATALENGTH(@matkhaumoi) = 0 THEN MatKhau
            ELSE HASHBYTES('SHA2_512', @matkhaumoi)
        END
    WHERE MaKH = @MaKH;
END;
GO

CREATE OR ALTER PROC dbo.KhachHang_DS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaKH, HoKH, TenKH, DienThoai, Email, DiaChi, MaXP, TenDN, MatKhau, AnhKH
    FROM dbo.KhachHang
    ORDER BY MaKH, HoKH, TenKH;
END;
GO

CREATE OR ALTER PROC dbo.KhachHang_CT
    @MaKH smallint
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaKH, HoKH, TenKH, DienThoai, Email, DiaChi, MaXP, TenDN, MatKhau, AnhKH
    FROM dbo.KhachHang
    WHERE MaKH = @MaKH;
END;
GO
