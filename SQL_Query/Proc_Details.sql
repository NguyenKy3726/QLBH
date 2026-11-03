USE DbQLBH;
GO

-- 1. Chi tiết đơn vị tính
CREATE OR ALTER PROC dbo.DonViTinh_CT
    @MaDVT tinyint
AS
    SELECT MaDVT, TenDVT
    FROM dbo.DonViTinh
    WHERE MaDVT = @MaDVT;
GO

-- 2. Chi tiết nhóm sản phẩm
CREATE OR ALTER PROC dbo.NhomSP_CT
    @MaNSP tinyint
AS
    SELECT MaNSP, TenNSP
    FROM dbo.NhomSP
    WHERE MaNSP = @MaNSP;
GO

-- 3. Chi tiết loại sản phẩm
CREATE OR ALTER PROC dbo.LoaiSP_CT
    @MaLSP smallint
AS
    SELECT MaLSP, TenLSP, MaNSP
    FROM dbo.LoaiSP
    WHERE MaLSP = @MaLSP;
GO

-- 4. Chi tiết sản phẩm
CREATE OR ALTER PROC dbo.SanPham_CT
    @MaSP char(9)
AS
    SELECT MaSP, TenSP, DonGia, AnhSP, MoTaCT, MaLSP, MaDVT
    FROM dbo.SanPham
    WHERE MaSP = @MaSP;
GO

-- 5. Chi tiết tỉnh
CREATE OR ALTER PROC dbo.Tinh_CT
    @MaTinh char(2)
AS
    SELECT MaTinh, TenTinh
    FROM dbo.Tinh
    WHERE MaTinh = @MaTinh;
GO

-- 6. Chi tiết xã/phường
CREATE OR ALTER PROC dbo.XaPhuong_CT
    @MaXP char(5)
AS
    SELECT MaXP, TenXP, MaTinh
    FROM dbo.XaPhuong
    WHERE MaXP = @MaXP;
GO

-- 7. Chi tiết phòng ban
CREATE OR ALTER PROC dbo.PhongBan_CT
    @MaPB tinyint
AS
    SELECT MaPB, TenPB
    FROM dbo.PhongBan
    WHERE MaPB = @MaPB;
GO

-- 8. Chi tiết quyền
CREATE OR ALTER PROC dbo.Quyen_CT
    @MaQuyen tinyint
AS
    SELECT MaQuyen, TenQuyen, MoTa
    FROM dbo.Quyen
    WHERE MaQuyen = @MaQuyen;
GO

-- 9. Chi tiết nhân viên
CREATE OR ALTER PROC dbo.NhanVien_CT
    @MaNV tinyint
AS
    SELECT MaNV, HoNV, TenNV, DienThoai, Email,
           TenDN, MatKhau, AnhNV, MaPB
    FROM dbo.NhanVien
    WHERE MaNV = @MaNV;
GO

-- 10. Chi tiết phân quyền: khóa chính ghép
CREATE OR ALTER PROC dbo.PhanQuyen_CT
    @MaQuyen tinyint,
    @MaNV tinyint
AS
    SELECT MaQuyen, MaNV
    FROM dbo.PhanQuyen
    WHERE MaQuyen = @MaQuyen
      AND MaNV = @MaNV;
GO

-- 11. Chi tiết nhà cung cấp
CREATE OR ALTER PROC dbo.NhaCC_CT
    @MaNCC smallint
AS
    SELECT MaNCC, TenNCC, DienThoai, Email, DiaChi, MaXP
    FROM dbo.NhaCC
    WHERE MaNCC = @MaNCC;
GO

-- 12. Chi tiết đơn mua hàng
CREATE OR ALTER PROC dbo.DonMH_CT
    @MaDMH char(11)
AS
    SELECT MaDMH, NgayDat, NgayGiao, MaNV, MaNCC
    FROM dbo.DonMH
    WHERE MaDMH = @MaDMH;
GO

-- 13. Chi tiết một dòng mua hàng: khóa chính ghép
CREATE OR ALTER PROC dbo.CTMH_CT
    @MaDMH char(7),
    @MaSP char(9)
AS
    SELECT MaDMH, MaSP, SLM, DGM
    FROM dbo.CTMH
    WHERE MaDMH = @MaDMH
      AND MaSP = @MaSP;
GO

-- 14. Chi tiết khách hàng
CREATE OR ALTER PROC dbo.KhachHang_CT
    @MaKH smallint
AS
    SELECT MaKH, HoKH, TenKH, DienThoai, Email,
           DiaChi, MaXP, TenDN, MatKhau
    FROM dbo.KhachHang
    WHERE MaKH = @MaKH;
GO

-- 15. Chi tiết đơn bán hàng
CREATE OR ALTER PROC dbo.DonBH_CT
    @MaDBH char(11)
AS
    SELECT MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH
    FROM dbo.DonBH
    WHERE MaDBH = @MaDBH;
GO

-- 16. Chi tiết một dòng bán hàng: khóa chính ghép
CREATE OR ALTER PROC dbo.CTBH_CT
    @MaDBH char(9),
    @MaSP char(9)
AS
    SELECT MaDBH, MaSP, SLB, DGB
    FROM dbo.CTBH
    WHERE MaDBH = @MaDBH
      AND MaSP = @MaSP;
GO