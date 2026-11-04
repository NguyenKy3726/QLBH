USE DbQLBH
GO

CREATE OR ALTER PROC dbo.NhanVien_DangNhap
    @TenDN nvarchar(50),
    @MatKhau nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaNV
    FROM dbo.NhanVien
    WHERE TenDN = @TenDN
      AND MatKhau = HASHBYTES('SHA2_512', @MatKhau);

    IF @@ROWCOUNT = 0
        SELECT CAST(0 AS smallint) AS MaNV;
END
GO


-- Đăng nhập khách hàng, trả về MaKH = 0 nếu thông tin không hợp lệ.
CREATE OR ALTER PROC dbo.KhachHang_DangNhap
    @TenDN nvarchar(50),
    @MatKhau nvarchar(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaKH
    FROM dbo.KhachHang
    WHERE TenDN = @TenDN
      AND MatKhau = HASHBYTES('SHA2_512', @MatKhau);

    IF @@ROWCOUNT = 0
        SELECT CAST(0 AS smallint) AS MaKH;
END
GO
