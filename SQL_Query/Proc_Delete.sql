use DbQLBH
go
-- 1. Xóa bảng Đơn vị tính
CREATE OR ALTER PROC DonViTinh_Xoa
    @MaDVT tinyint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM SanPham WHERE MaDVT = @MaDVT)
    BEGIN
        RAISERROR(N'Không thể xóa đơn vị tính này vì đang có sản phẩm sử dụng.', 16, 1);
        RETURN;
    END

    DELETE FROM DonViTinh
    WHERE MaDVT = @MaDVT;
END
GO

-- 2. Xóa bảng Nhóm sản phẩm
CREATE OR ALTER PROC NhomSP_Xoa
    @MaNSP tinyint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM LoaiSP WHERE MaNSP = @MaNSP)
    BEGIN
        RAISERROR(N'Không thể xóa nhóm sản phẩm này vì đang có loại sản phẩm thuộc nhóm này.', 16, 1);
        RETURN;
    END

    DELETE FROM NhomSP
    WHERE MaNSP = @MaNSP;
END
GO

-- 3. Xóa bảng Loại sản phẩm
CREATE OR ALTER PROC LoaiSP_Xoa
    @MaLSP smallint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM SanPham WHERE MaLSP = @MaLSP)
    BEGIN
        RAISERROR(N'Không thể xóa loại sản phẩm này vì đang có sản phẩm thuộc loại này.', 16, 1);
        RETURN;
    END

    DELETE FROM LoaiSP
    WHERE MaLSP = @MaLSP;
END
GO

-- 4. Xóa bảng Sản phẩm
CREATE OR ALTER PROC SanPham_Xoa
    @MaSP char(9)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM CTMH WHERE MaSP = @MaSP)
       OR EXISTS (SELECT 1 FROM CTBH WHERE MaSP = @MaSP)
    BEGIN
        RAISERROR(N'Không thể xóa sản phẩm này vì sản phẩm này đã xuất hiện trong chi tiết mua hàng hoặc chi tiết bán hàng.', 16, 1);
        RETURN;
    END

    DELETE FROM SanPham
    WHERE MaSP = @MaSP;
END
GO

-- 5. Xóa bảng Tỉnh
CREATE OR ALTER PROC Tinh_Xoa
    @MaTinh char(2)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM XaPhuong WHERE MaTinh = @MaTinh)
    BEGIN
        RAISERROR(N'Không thể xóa tỉnh này vì đang có xã/phường thuộc tỉnh này.', 16, 1);
        RETURN;
    END

    DELETE FROM Tinh
    WHERE MaTinh = @MaTinh;
END
GO

-- 6. Xóa bảng Xã/phường
CREATE OR ALTER PROC XaPhuong_Xoa
    @MaXP char(5)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM NhaCC WHERE MaXP = @MaXP)
       OR EXISTS (SELECT 1 FROM KhachHang WHERE MaXP = @MaXP)
       OR EXISTS (SELECT 1 FROM DonBH WHERE MaXP = @MaXP)
    BEGIN
        RAISERROR(N'Không thể xóa xã/phường này vì đang được nhà cung cấp, khách khách hàng hoặc đơn bán hàng sử dụng.', 16, 1);
        RETURN;
    END

    DELETE FROM XaPhuong
    WHERE MaXP = @MaXP;
END
GO

-- 7. Xóa bảng Phòng ban
CREATE OR ALTER PROC PhongBan_Xoa
    @MaPB tinyint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM NhanVien WHERE MaPB = @MaPB)
    BEGIN
        RAISERROR(N'Không thể xóa phòng ban này vì đang có nhân viên đang thuộc phòng ban.', 16, 1);
        RETURN;
    END

    DELETE FROM PhongBan
    WHERE MaPB = @MaPB;
END
GO

-- 8. Xóa bảng Quyền
CREATE OR ALTER PROC Quyen_Xoa
    @MaQuyen tinyint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM PhanQuyen WHERE MaQuyen = @MaQuyen)
    BEGIN
        RAISERROR(N'Không thể xóa quyền này vì quyền này đang được cấp cho nhân viên.', 16, 1);
        RETURN;
    END

    DELETE FROM Quyen
    WHERE MaQuyen = @MaQuyen;
END
GO

-- 9. Xóa bảng Nhân viên
CREATE OR ALTER PROC NhanVien_Xoa
    @MaNV tinyint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM PhanQuyen WHERE MaNV = @MaNV)
       OR EXISTS (SELECT 1 FROM DonMH WHERE MaNV = @MaNV)
       OR EXISTS (SELECT 1 FROM DonBH WHERE MaNV = @MaNV)
    BEGIN
        RAISERROR(N'Không thể xóa nhân viên này vì đang có phân quyền hoặc đơn hàng do nhân viên này phụ trách.', 16, 1);
        RETURN;
    END

    DELETE FROM NhanVien
    WHERE MaNV = @MaNV;
END
GO

-- 10. Xóa bảng Phân quyền
CREATE OR ALTER PROC PhanQuyen_Xoa
    @MaQuyen tinyint,
    @MaNV tinyint
AS
BEGIN
    DELETE FROM PhanQuyen
    WHERE MaQuyen = @MaQuyen
      AND MaNV = @MaNV;
END
GO

-- 11. Xóa bảng Nhà cung cấp
CREATE OR ALTER PROC NhaCC_Xoa
    @MaNCC smallint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DonMH WHERE MaNCC = @MaNCC)
    BEGIN
        RAISERROR(N'Không thể xóa nhà cung cấp này vì đã có đơn mua hàng của nhà cung cấp này.', 16, 1);
        RETURN;
    END

    DELETE FROM NhaCC
    WHERE MaNCC = @MaNCC;
END
GO

-- 12. Xóa bảng Đơn mua hàng
CREATE OR ALTER PROC DonMH_Xoa
    @MaDMH char(11)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM CTMH WHERE MaDMH = @MaDMH)
    BEGIN
        RAISERROR(N'Không thể xóa đơn mua hàng này vì vẫn còn chi tiết mua hàng. Hãy xóa các dòng chi tiết trước.', 16, 1);
        RETURN;
    END

    DELETE FROM DonMH
    WHERE MaDMH = @MaDMH;
END
GO

-- 13. Xóa bảng Chi tiết mua hàng
CREATE OR ALTER PROC CTMH_Xoa
    @MaDMH char(11),
    @MaSP char(9)
AS
BEGIN
    DELETE FROM CTMH
    WHERE MaDMH = @MaDMH
      AND MaSP = @MaSP;
END
GO

-- 14. Xóa bảng Khách hàng
CREATE OR ALTER PROC KhachHang_Xoa
    @MaKH smallint
AS
BEGIN
    IF EXISTS (SELECT 1 FROM DonBH WHERE MaKH = @MaKH)
    BEGIN
        RAISERROR(N'Không thể xóa khách hàng này vì đã có đơn bán hàng của khách hàng trong hệ thống.', 16, 1);
        RETURN;
    END

    DELETE FROM KhachHang
    WHERE MaKH = @MaKH;
END
GO

-- 15. Xóa bảng Đơn bán hàng
CREATE OR ALTER PROC DonBH_Xoa
    @MaDBH char(11)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.DonBH WHERE MaDBH = @MaDBH AND MaNV IS NULL)
    BEGIN
        RAISERROR(N'Không thể xóa đơn hàng do khách hàng đặt; cần giữ lại làm lịch sử bán hàng.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM CTBH WHERE MaDBH = @MaDBH)
    BEGIN
        RAISERROR(N'Không thể xóa đơn bán hàng này vì vẫn còn chi tiết bán hàng. Hãy xóa các dòng chi tiết trước.', 16, 1);
        RETURN;
    END

    DELETE FROM DonBH
    WHERE MaDBH = @MaDBH;
END
GO

-- 16. Xóa bảng Chi tiết bán hàng
CREATE OR ALTER PROC CTBH_Xoa
    @MaDBH char(11),
    @MaSP char(9)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.DonBH WHERE MaDBH = @MaDBH AND MaNV IS NULL)
    BEGIN
        RAISERROR(N'Không thể xóa chi tiết của đơn do khách hàng đặt. Dòng giỏ hàng đã được chuyển thành lịch sử đơn bán.', 16, 1);
        RETURN;
    END;

    DELETE FROM CTBH
    WHERE MaDBH = @MaDBH
      AND MaSP = @MaSP;
END
GO