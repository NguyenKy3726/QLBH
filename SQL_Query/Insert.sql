USE DbQLBH;
GO

SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- 1. Đơn vị tính: 5 dòng
    INSERT INTO dbo.DonViTinh (MaDVT, TenDVT)
    VALUES
        (1, N'Chai'),
        (2, N'Lon'),
        (3, N'Hộp'),
        (4, N'Gói'),
        (5, N'Thùng');


    -- 2. Nhóm sản phẩm: 3 dòng
    INSERT INTO dbo.NhomSP (MaNSP, TenNSP)
    VALUES
        (1, N'Đồ uống'),
        (2, N'Bánh kẹo'),
        (3, N'Thực phẩm');


    -- 3. Loại sản phẩm: 6 dòng
    INSERT INTO dbo.LoaiSP (MaLSP, TenLSP, MaNSP)
    VALUES
        (1, N'Nước suối', 1),
        (2, N'Nước ngọt', 1),
        (3, N'Bánh quy', 2),
        (4, N'Kẹo', 2),
        (5, N'Mì ăn liền', 3),
        (6, N'Thực phẩm đóng hộp', 3);


    -- 4. Sản phẩm: 12 dòng
    -- Đơn giá mẫu tính bằng VND.
    -- Chưa có file ảnh nên AnhSP để NULL.
    INSERT INTO dbo.SanPham
        (MaSP, TenSP, DonGia, AnhSP, MoTaCT, MaLSP, MaDVT)
    VALUES
        ('261015001', N'Nước suối An Nhiên 500ml',
         5000, NULL, N'Nước uống đóng chai, dung tích 500ml.', 1, 1),

        ('261015002', N'Nước suối An Nhiên 1.5L',
         10000, NULL, N'Nước uống đóng chai, dung tích 1.5 lít.', 1, 1),

        ('261015003', N'Nước ngọt vị cola 330ml',
         10000, NULL, N'Nước ngọt có gas vị cola, lon 330ml.', 2, 2),

        ('261015004', N'Nước ngọt vị cam 330ml',
         10000, NULL, N'Nước ngọt có gas vị cam, lon 330ml.', 2, 2),

        ('261015005', N'Bánh quy bơ 200g',
         35000, NULL, N'Bánh quy vị bơ, hộp 200g.', 3, 3),

        ('261015006', N'Bánh quy cacao 150g',
         25000, NULL, N'Bánh quy vị cacao, gói 150g.', 3, 4),

        ('261015007', N'Kẹo sữa 100g',
         15000, NULL, N'Kẹo vị sữa, gói 100g.', 4, 4),

        ('261015008', N'Kẹo trái cây 120g',
         18000, NULL, NULL, 4, 4),

        ('261015009', N'Mì ăn liền vị tôm 75g',
         5000, NULL, N'Mì ăn liền vị tôm, gói 75g.', 5, 4),

        ('261015010', N'Mì ăn liền vị bò 75g',
         5000, NULL, N'Mì ăn liền vị bò, gói 75g.', 5, 4),

        ('261015011', N'Cá hộp sốt cà 155g',
         22000, NULL, N'Cá đóng hộp sốt cà chua, hộp 155g.', 6, 3),

        ('261015012', N'Thịt hộp 170g',
         32000, NULL, NULL, 6, 3);


    -- 5. Tỉnh: 3 dòng
    -- Mã nội bộ dùng cho bài thực hành.
    INSERT INTO dbo.Tinh (MaTinh, TenTinh)
    VALUES
        ('01', N'Hà Nội'),
        ('02', N'Đà Nẵng'),
        ('03', N'Hồ Chí Minh');


    -- 6. Xã/phường: 6 dòng
    -- Tên minh họa, không phải danh mục hành chính chính thức.
    INSERT INTO dbo.XaPhuong (MaXP, TenXP, MaTinh)
    VALUES
        ('01001', N'Phường Minh An', '01'),
        ('01002', N'Phường Bình Minh', '01'),
        ('02001', N'Phường An Hòa', '02'),
        ('02002', N'Phường Hòa Bình', '02'),
        ('03001', N'Phường Tân An', '03'),
        ('03002', N'Phường An Bình', '03');


    -- 7. Phòng ban: 3 dòng
    INSERT INTO dbo.PhongBan (MaPB, TenPB)
    VALUES
        (1, N'Quản lý'),
        (2, N'Bán hàng'),
        (3, N'Kho');


    -- 8. Quyền: 4 dòng
    INSERT INTO dbo.Quyen (MaQuyen, TenQuyen, MoTa)
    VALUES
        (1, N'Quản trị',
         N'Quản lý tài khoản, danh mục và phân quyền.'),

        (2, N'Bán hàng',
         N'Quản lý khách hàng và lập đơn bán hàng.'),

        (3, N'Nhập hàng',
         N'Quản lý nhà cung cấp và lập đơn mua hàng.'),

        (4, N'Xem báo cáo',
         N'Xem báo cáo mua hàng, bán hàng và doanh thu.');


    -- Mật khẩu chung
DECLARE @MatKhauMau varbinary(MAX)
    = HASHBYTES('SHA2_512', N'ahihi2006');


    -- 9. Nhân viên: 5 dòng
    INSERT INTO dbo.NhanVien
        (MaNV, HoNV, TenNV, DienThoai, Email,
         TenDN, MatKhau, AnhNV, MaPB)
    VALUES
        (1, N'Nguyễn Văn', N'An', '0375641438',
         'an@example.com', N'admin',
         @MatKhauMau, NULL, 1),

        (2, N'Trần Thị', N'Bình', '0364082026',
         'binh@example.com', N'nvbinh',
         @MatKhauMau, NULL, 2),

        (3, N'Lê Minh', N'Châu', '0394378351',
         'chau@example.com', N'nvchau',
         @MatKhauMau, NULL, 2),

        (4, N'Phạm Quốc', N'Dũng', '0905206364',
         'dung@example.com', N'nvdung',
         @MatKhauMau, NULL, 3),

        (5, N'Võ Thị', N'Hà', '0902056821',
         'ha@example.com', N'nvha',
         @MatKhauMau, NULL, 3);


    -- 10. Phân quyền: 8 dòng
    INSERT INTO dbo.PhanQuyen (MaQuyen, MaNV)
    VALUES
        (1, 1),
        (4, 1),
        (2, 2),
        (4, 2),
        (2, 3),
        (3, 4),
        (4, 4),
        (3, 5);


    -- 11. Nhà cung cấp: 3 dòng
    INSERT INTO dbo.NhaCC
        (MaNCC, TenNCC, DienThoai, Email, DiaChi, MaXP)
    VALUES
        (1, N'Công ty Đồ uống Bình Minh',
         '0910000001', 'douong@example.com',
         N'12 Đường Hoa Mai', '01001'),

        (2, N'Công ty Bánh kẹo An Phúc',
         '0910000002', NULL,
         N'25 Đường Hoa Sen', '02001'),

        (3, N'Công ty Thực phẩm Minh An',
         NULL, 'thucpham@example.com',
         N'36 Đường Hoa Lan', '03001');


    -- 12. Đơn mua hàng: 4 dòng
    -- Ba đơn đã giao, một đơn chưa giao.
    INSERT INTO dbo.DonMH
        (MaDMH, NgayDat, NgayGiao, MaNV, MaNCC)
    VALUES
        ('MH00001', '2026-10-15T06:00:00',
         '2026-10-15T07:00:00', 4, 1),

        ('MH00002', '2026-10-15T06:10:00',
         '2026-10-15T07:10:00', 5, 2),

        ('MH00003', '2026-10-15T06:20:00',
         '2026-10-15T07:20:00', 4, 2),

        ('MH00004', '2026-10-15T07:30:00',
         NULL, 5, 3);


    -- 13. Chi tiết mua hàng: 10 dòng
    INSERT INTO dbo.CTMH (MaDMH, MaSP, SLM, DGM)
    VALUES
        ('MH00001', '261015001', 100, 3500),
        ('MH00001', '261015002',  60, 7000),
        ('MH00001', '261015003', 100, 7500),

        ('MH00002', '261015005',  40, 27000),
        ('MH00002', '261015006',  50, 18000),
        ('MH00002', '261015007',  60, 10000),

        ('MH00003', '261015007',  40, 10000),
        ('MH00003', '261015008',  60, 12000),

        ('MH00004', '261015009', 100, 3500),
        ('MH00004', '261015010', 100, 3500);


    -- 14. Khách hàng: 6 dòng
    INSERT INTO dbo.KhachHang
        (MaKH, HoKH, TenKH, DienThoai, Email,
         DiaChi, MaXP, TenDN, MatKhau)
    VALUES
        (1, N'Nguyễn Thị', N'Lan',
         '0920000001', 'lan@example.com',
         N'10 Đường Hoa Mai', '01001',
         N'khlan', @MatKhauMau),

        (2, N'Trần Văn', N'Minh',
         '0920000002', NULL,
         N'20 Đường Hoa Đào', '01002',
         N'khminh', @MatKhauMau),

        (3, N'Lê Thị', N'Ngọc',
         '0920000003', 'ngoc@example.com',
         N'30 Đường Hoa Sen', '02001',
         N'khngoc', @MatKhauMau),

        (4, N'Phạm Hoàng', N'Nam',
         NULL, 'nam@example.com',
         N'40 Đường Hoa Cúc', '02002',
         N'khnam', @MatKhauMau),

        (5, N'Võ Thanh', N'Tâm',
         '0920000005', NULL,
         N'50 Đường Hoa Lan', '03001',
         N'khtam', @MatKhauMau),

        (6, N'Đặng Thị', N'Thu',
         '0920000006', 'thu@example.com',
         N'60 Đường Hoa Hồng', '03002',
         N'khthu', @MatKhauMau);


    -- 15. Đơn bán hàng: 6 dòng
    -- Bốn đơn đã giao được đặt HOAN_TAT; hai đơn chưa giao được đặt DA_XAC_NHAN.
    -- Trạng thái hai đơn chưa giao là trạng thái mẫu khởi tạo.
    INSERT INTO dbo.DonBH
        (MaDBH, NgayDat, NgayGiao, DCGH, MaXP, MaNV, MaKH, TrangThai)
    VALUES
        ('BH0000001', '2026-10-15T07:30:00',
         '2026-10-15T08:00:00',
         N'10 Đường Hoa Mai', '01001', 2, 1, 'HOAN_TAT'),

        ('BH0000002', '2026-10-15T07:40:00',
         '2026-10-15T08:10:00',
         N'20 Đường Hoa Đào', '01002', 3, 2, 'HOAN_TAT'),

        ('BH0000003', '2026-10-15T07:50:00',
         '2026-10-15T08:20:00',
         N'30 Đường Hoa Sen', '02001', 2, 3, 'HOAN_TAT'),

        ('BH0000004', '2026-10-15T08:00:00',
         '2026-10-15T08:30:00',
         N'40 Đường Hoa Cúc', '02002', 3, 4, 'HOAN_TAT'),

        ('BH0000005', '2026-10-15T08:40:00',
         NULL,
         N'50 Đường Hoa Lan', '03001', 2, 5, 'DA_XAC_NHAN'),

        ('BH0000006', '2026-10-15T08:50:00',
         NULL,
         N'60 Đường Hoa Hồng', '03002', 3, 6, 'DA_XAC_NHAN');

    -- Đây là bản ghi trạng thái khởi tạo cho dữ liệu mẫu; file không có
    -- lịch sử thao tác trước thời điểm tạo bộ dữ liệu này.
    INSERT INTO dbo.LichSuTrangThaiDonBH
        (MaDBH, TrangThaiCu, TrangThaiMoi, ThoiDiem, MaNV, GhiChu)
    SELECT MaDBH, NULL, TrangThai, SYSDATETIME(), NULL,
           N'Khởi tạo trạng thái trong dữ liệu mẫu.'
    FROM dbo.DonBH
    WHERE MaDBH IN
        ('BH0000001', 'BH0000002', 'BH0000003',
         'BH0000004', 'BH0000005', 'BH0000006');


    -- 16. Chi tiết bán hàng: 15 dòng
    -- Chỉ bán các sản phẩm đã được giao ở đơn mua.
    INSERT INTO dbo.CTBH (MaDBH, MaSP, SLB, DGB)
    VALUES
        ('BH0000001', '261015001',  5, 5000),
        ('BH0000001', '261015003',  3, 10000),
        ('BH0000001', '261015005',  1, 35000),

        ('BH0000002', '261015002',  2, 10000),
        ('BH0000002', '261015006',  2, 25000),

        ('BH0000003', '261015001', 10, 5000),
        ('BH0000003', '261015007',  3, 15000),
        ('BH0000003', '261015008',  2, 18000),

        ('BH0000004', '261015003',  6, 10000),
        ('BH0000004', '261015005',  2, 35000),

        ('BH0000005', '261015002',  4, 10000),
        ('BH0000005', '261015006',  1, 25000),
        ('BH0000005', '261015008',  3, 18000),

        ('BH0000006', '261015001',  6, 5000),
        ('BH0000006', '261015007',  2, 15000);


    COMMIT TRANSACTION;

    PRINT N'Đã thêm dữ liệu mẫu và trạng thái khởi tạo cho đơn bán hàng thành công.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO