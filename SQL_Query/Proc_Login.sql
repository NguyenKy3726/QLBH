use DbQLBH
go
create or alter proc NhanVien_DangNhap
	@TenDN varchar(50),
	@MatKhau nvarchar(50)
as
	select * from NhanVien where TenDN = @TenDN and MatKhau = HASHBYTES('SHA2_512', @MatKhau)
	if @@ROWCOUNT = 0
		select MaNV = 0 --Nếu đăng nhập sai trả về MaNV mặc định = 0

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
