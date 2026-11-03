use DbQLBH
go
create or alter proc NhanVien_DangNhap
	@TenDN varchar(50),
	@MatKhau nvarchar(50)
as
	select * from NhanVien where TenDN = @TenDN and MatKhau = HASHBYTES('SHA2_512', @MatKhau)
	if @@ROWCOUNT = 0
		select MaNV = 0 --Nếu đăng nhập sai trả về MaNV mặc định = 0