USE DbQLBH;
GO

-- 1. Đơn vị tính
CREATE OR ALTER PROC dbo.DonViTinh_DS
AS
    SELECT MaDVT, TenDVT
    FROM dbo.DonViTinh
    ORDER BY MaDVT, TenDVT;
GO

-- 2. Nhóm sản phẩm
CREATE OR ALTER PROC dbo.NhomSP_DS
AS
    SELECT MaNSP, TenNSP
    FROM dbo.NhomSP
    ORDER BY MaNSP, TenNSP;
GO

-- 3. Loại sản phẩm
CREATE OR ALTER PROC dbo.LoaiSP_DS
AS
    SELECT MaLSP, TenLSP, MaNSP
    FROM dbo.LoaiSP
    ORDER BY MaLSP, TenLSP;
GO

-- 4. Sản phẩm
CREATE OR ALTER PROC dbo.SanPham_DS
AS
    SELECT MaSP, TenSP, DonGia, AnhSP, MoTaCT, MaLSP, MaDVT
    FROM dbo.SanPham
    ORDER BY MaSP, TenSP;
GO

-- 5. Tỉnh
CREATE OR ALTER PROC dbo.Tinh_DS
AS
    SELECT MaTinh, TenTinh
    FROM dbo.Tinh
    ORDER BY MaTinh, TenTinh;
GO

-- 6. Xã/phường
CREATE OR ALTER PROC dbo.XaPhuong_DS
AS
    SELECT MaXP, TenXP, MaTinh
    FROM dbo.XaPhuong
    ORDER BY MaXP, TenXP;
GO

-- 7. Phòng ban
CREATE OR ALTER PROC dbo.PhongBan_DS
AS
    SELECT MaPB, TenPB
    FROM dbo.PhongBan
    ORDER BY MaPB, TenPB;
GO

-- 8. Quyền
CREATE OR ALTER PROC dbo.Quyen_DS
AS
    SELECT MaQuyen, TenQuyen, MoTa
    FROM dbo.Quyen
    ORDER BY MaQuyen, TenQuyen;
GO

-- 9. Nhân viên
CREATE OR ALTER PROC dbo.NhanVien_DS
AS
    SELECT MaNV, HoNV, TenNV, DienThoai, Email,
           TenDN, MatKhau, AnhNV, MaPB
    FROM dbo.NhanVien
    ORDER BY MaNV, HoNV, TenNV;
GO

-- 10. Phân quyền
CREATE OR ALTER PROC dbo.PhanQuyen_DS
AS
    SELECT MaQuyen, MaNV
    FROM dbo.PhanQuyen
    ORDER BY MaQuyen, MaNV;
GO

-- 11. Nhà cung cấp
CREATE OR ALTER PROC dbo.NhaCC_DS
AS
    SELECT MaNCC, TenNCC, DienThoai, Email, DiaChi, MaXP
    FROM dbo.NhaCC
    ORDER BY MaNCC, TenNCC;
GO

-- 12. Đơn mua hàng
CREATE OR ALTER PROC dbo.DonMH_DS
AS
    SELECT MaDMH, NgayDat, NgayGiao, MaNV, MaNCC
    FROM dbo.DonMH
    ORDER BY MaDMH, NgayDat;
GO

-- 13. Chi tiết mua hàng
CREATE OR ALTER PROC dbo.CTMH_DS
AS
    SELECT MaDMH, MaSP, SLM, DGM
    FROM dbo.CTMH
    ORDER BY MaDMH, MaSP;
GO

-- 14. Khách hàng
CREATE OR ALTER PROC dbo.KhachHang_DS
AS
    SELECT MaKH, HoKH, TenKH, DienThoai, Email,
           DiaChi, MaXP, TenDN, MatKhau
    FROM dbo.KhachHang
    ORDER BY MaKH, HoKH, TenKH;
GO

-- 15. Đơn bán hàng
CREATE OR ALTER PROC dbo.DonBH_DS
AS
    SELECT MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH, TrangThai
    FROM dbo.DonBH
    ORDER BY NgayDat DESC, MaDBH DESC;
GO

-- Danh sách đơn của đúng một khách hàng đang đăng nhập.
CREATE OR ALTER PROC dbo.DonBH_KhachHang_DS
    @MaKH smallint
AS
    SELECT MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH, TrangThai
    FROM dbo.DonBH
    WHERE MaKH = @MaKH
    ORDER BY NgayDat DESC, MaDBH DESC;
GO

-- 16. Chi tiết bán hàng
CREATE OR ALTER PROC dbo.CTBH_DS
AS
    SELECT MaDBH, MaSP, SLB, DGB
    FROM dbo.CTBH
    ORDER BY MaDBH, MaSP;
GO