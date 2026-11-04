use DbQLBH
-- 1. Đơn vị tính
CREATE TABLE dbo.DonViTinh
(
    MaDVT  tinyint      NOT NULL,
    TenDVT nvarchar(20) NOT NULL,

    CONSTRAINT PK_DonViTinh PRIMARY KEY (MaDVT)
);
GO
-- 2. Nhóm sản phẩm
CREATE TABLE dbo.NhomSP
(
    MaNSP  tinyint      NOT NULL,
    TenNSP nvarchar(45) NOT NULL,

    CONSTRAINT PK_NhomSP PRIMARY KEY (MaNSP)
);
GO
-- 3. Loại sản phẩm
CREATE TABLE dbo.LoaiSP
(
    MaLSP  smallint     NOT NULL,
    TenLSP nvarchar(50) NOT NULL,
    MaNSP  tinyint      NOT NULL,

CONSTRAINT PK_LoaiSP PRIMARY KEY (MaLSP),

CONSTRAINT FK_LoaiSP_NhomSP FOREIGN KEY (MaNSP) REFERENCES dbo.NhomSP (MaNSP)

);
GO
-- 4. Sản phẩm
CREATE TABLE dbo.SanPham
(
    MaSP   char(9)       NOT NULL,
    TenSP  nvarchar(50)  NOT NULL,
    DonGia money         NOT NULL,
    AnhSP  nvarchar(255) NULL,
    AnhSPData varbinary(MAX) NULL,
    AnhSPContentType varchar(100) NULL,
    MoTaCT ntext         NULL,
    MaLSP  smallint      NOT NULL,
    MaDVT  tinyint       NOT NULL,

CONSTRAINT PK_SanPham PRIMARY KEY (MaSP),

    CONSTRAINT FK_SanPham_LoaiSP
        FOREIGN KEY (MaLSP)
        REFERENCES dbo.LoaiSP (MaLSP),

    CONSTRAINT FK_SanPham_DonViTinh
        FOREIGN KEY (MaDVT)
        REFERENCES dbo.DonViTinh (MaDVT),

    -- Mã gồm đúng 9 chữ số, số thứ tự từ 001 đến 999.
    CONSTRAINT CK_SanPham_MaSP
        CHECK
        (
            MaSP COLLATE Latin1_General_100_BIN2 LIKE
                '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
            AND RIGHT(MaSP, 3) <> '000'
        )
);
GO
-- 5. Tỉnh
CREATE TABLE dbo.Tinh
(
    MaTinh  char(2)      NOT NULL,
    TenTinh nvarchar(50) NOT NULL,

    CONSTRAINT PK_Tinh PRIMARY KEY (MaTinh)
);
GO
-- 6. Xã/phường
CREATE TABLE dbo.XaPhuong
(
    MaXP   char(5)      NOT NULL,
    TenXP  nvarchar(50) NOT NULL,
    MaTinh char(2)      NOT NULL,

CONSTRAINT PK_XaPhuong PRIMARY KEY (MaXP),

    CONSTRAINT FK_XaPhuong_Tinh
        FOREIGN KEY (MaTinh)
        REFERENCES dbo.Tinh (MaTinh)
);
GO
-- 7. Phòng ban
CREATE TABLE dbo.PhongBan
(
    MaPB  tinyint      NOT NULL,
    TenPB nvarchar(40) NOT NULL,

    CONSTRAINT PK_PhongBan PRIMARY KEY (MaPB)
);
GO
-- 8. Quyền
CREATE TABLE dbo.Quyen
(
    MaQuyen  tinyint      NOT NULL,
    TenQuyen nvarchar(50) NOT NULL,
    MoTa     ntext        NULL,

    CONSTRAINT PK_Quyen PRIMARY KEY (MaQuyen)
);
GO
-- 9. Nhân viên
CREATE TABLE dbo.NhanVien
(
    MaNV      tinyint       NOT NULL,
    HoNV      nvarchar(30)  NOT NULL,
    TenNV     nvarchar(20)  NOT NULL,
    DienThoai varchar(11)   NOT NULL,
    Email     varchar(40)   NOT NULL,
    TenDN     nvarchar(50)  NOT NULL,
    MatKhau   varbinary(MAX) NOT NULL,
    AnhNV     nvarchar(255)  NULL,
    AnhNVData varbinary(MAX) NULL,
    AnhNVContentType varchar(100) NULL,
    MaPB      tinyint       NOT NULL,

CONSTRAINT PK_NhanVien PRIMARY KEY (MaNV),

    CONSTRAINT FK_NhanVien_PhongBan
        FOREIGN KEY (MaPB)
        REFERENCES dbo.PhongBan (MaPB)
);
GO
-- 10. Phân quyền
CREATE TABLE dbo.PhanQuyen
(
    MaQuyen tinyint NOT NULL,
    MaNV    tinyint NOT NULL,

CONSTRAINT PK_PhanQuyen PRIMARY KEY (MaQuyen, MaNV),

    CONSTRAINT FK_PhanQuyen_Quyen
        FOREIGN KEY (MaQuyen)
        REFERENCES dbo.Quyen (MaQuyen),

    CONSTRAINT FK_PhanQuyen_NhanVien
        FOREIGN KEY (MaNV)
        REFERENCES dbo.NhanVien (MaNV)
);
GO
-- 11. Nhà cung cấp
CREATE TABLE dbo.NhaCC
(
    MaNCC     smallint     NOT NULL,
    TenNCC    nvarchar(50) NOT NULL,
    DienThoai varchar(11)  NULL,
    Email     varchar(40)  NULL,
    DiaChi    nvarchar(50) NOT NULL,
    MaXP      char(5)      NOT NULL,

CONSTRAINT PK_NhaCC PRIMARY KEY (MaNCC),

    CONSTRAINT FK_NhaCC_XaPhuong
        FOREIGN KEY (MaXP)
        REFERENCES dbo.XaPhuong (MaXP)
);
GO
-- 12. Đơn mua hàng
CREATE TABLE dbo.DonMH
(
    MaDMH    char(11)       NOT NULL,
    NgayDat  smalldatetime NOT NULL,
    NgayGiao smalldatetime NULL,
    MaNV     tinyint       NOT NULL,
    MaNCC    smallint      NOT NULL,

CONSTRAINT PK_DonMH PRIMARY KEY (MaDMH),

    CONSTRAINT FK_DonMH_NhanVien
        FOREIGN KEY (MaNV)
        REFERENCES dbo.NhanVien (MaNV),

    CONSTRAINT FK_DonMH_NhaCC
        FOREIGN KEY (MaNCC)
        REFERENCES dbo.NhaCC (MaNCC)
);
GO
-- 13. Chi tiết mua hàng
CREATE TABLE dbo.CTMH
(
    MaDMH char(11)  NOT NULL,
    MaSP  char(9)  NOT NULL,
    SLM   smallint NOT NULL,
    DGM   money    NOT NULL,

CONSTRAINT PK_CTMH PRIMARY KEY (MaDMH, MaSP),

    CONSTRAINT FK_CTMH_DonMH
        FOREIGN KEY (MaDMH)
        REFERENCES dbo.DonMH (MaDMH),

    CONSTRAINT FK_CTMH_SanPham
        FOREIGN KEY (MaSP)
        REFERENCES dbo.SanPham (MaSP)
);
GO
-- 14. Khách hàng
CREATE TABLE dbo.KhachHang
(
    MaKH      smallint       NOT NULL,
    HoKH      nvarchar(30)   NOT NULL,
    TenKH     nvarchar(20)   NOT NULL,
    DienThoai varchar(11)    NULL,
    Email     varchar(40)    NULL,
    DiaChi    nvarchar(50)   NOT NULL,
    MaXP      char(5)        NOT NULL,
    TenDN     nvarchar(50)   NOT NULL,
    MatKhau   varbinary(MAX) NOT NULL,
    AnhKH     nvarchar(255) NULL,
    AnhKHData varbinary(MAX) NULL,
    AnhKHContentType varchar(100) NULL,

CONSTRAINT PK_KhachHang PRIMARY KEY (MaKH),

    CONSTRAINT FK_KhachHang_XaPhuong
        FOREIGN KEY (MaXP)
        REFERENCES dbo.XaPhuong (MaXP)
);
GO
-- 15. Đơn bán hàng
CREATE TABLE dbo.DonBH
(
    MaDBH    char(11)       NOT NULL,
    NgayDat  smalldatetime NOT NULL,
    NgayGiao smalldatetime NULL,
    DCGH     nvarchar(50)  NOT NULL,
    MaXP     char(5)       NOT NULL,
    MaNV     tinyint       NULL,
    MaKH     smallint      NOT NULL,
    TrangThai varchar(20)    NOT NULL CONSTRAINT DF_DonBH_TrangThai DEFAULT ('CHO_XAC_NHAN'),

CONSTRAINT PK_DonBH PRIMARY KEY (MaDBH),
    CONSTRAINT CK_DonBH_TrangThai CHECK (TrangThai IN
        ('CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DANG_CHUAN_BI', 'DANG_GIAO', 'HOAN_TAT', 'DA_HUY')),

    CONSTRAINT FK_DonBH_XaPhuong
        FOREIGN KEY (MaXP)
        REFERENCES dbo.XaPhuong (MaXP),
    CONSTRAINT FK_DonBH_NhanVien
        FOREIGN KEY (MaNV)
        REFERENCES dbo.NhanVien (MaNV),
    CONSTRAINT FK_DonBH_KhachHang
        FOREIGN KEY (MaKH)
        REFERENCES dbo.KhachHang (MaKH)
);
GO
-- 16. Lịch sử trạng thái đơn bán hàng
CREATE TABLE dbo.LichSuTrangThaiDonBH
(
    MaLSTT       bigint IDENTITY(1,1) NOT NULL,
    MaDBH        char(11)             NOT NULL,
    TrangThaiCu  varchar(20)          NULL,
    TrangThaiMoi varchar(20)          NOT NULL,
    ThoiDiem     datetime2            NOT NULL CONSTRAINT DF_LichSuTrangThaiDonBH_ThoiDiem DEFAULT (SYSDATETIME()),
    MaNV         tinyint              NULL,
    GhiChu       nvarchar(500)        NULL,

    CONSTRAINT PK_LichSuTrangThaiDonBH PRIMARY KEY (MaLSTT),
    CONSTRAINT FK_LichSuTrangThaiDonBH_DonBH FOREIGN KEY (MaDBH)
        REFERENCES dbo.DonBH (MaDBH),
    CONSTRAINT FK_LichSuTrangThaiDonBH_NhanVien FOREIGN KEY (MaNV)
        REFERENCES dbo.NhanVien (MaNV),
    CONSTRAINT CK_LichSuTrangThaiDonBH_TrangThaiCu CHECK
        (TrangThaiCu IS NULL OR TrangThaiCu IN
            ('CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DANG_CHUAN_BI', 'DANG_GIAO', 'HOAN_TAT', 'DA_HUY')),
    CONSTRAINT CK_LichSuTrangThaiDonBH_TrangThaiMoi CHECK
        (TrangThaiMoi IN
            ('CHO_XAC_NHAN', 'DA_XAC_NHAN', 'DANG_CHUAN_BI', 'DANG_GIAO', 'HOAN_TAT', 'DA_HUY'))
);
GO

-- 16. Chi tiết bán hàng
CREATE TABLE dbo.CTBH
(
    MaDBH char(11) NOT NULL,
    MaSP  char(9) NOT NULL,
    SLB   tinyint NOT NULL,
    DGB   money   NOT NULL,
CONSTRAINT PK_CTBH PRIMARY KEY (MaDBH, MaSP),
    CONSTRAINT FK_CTBH_DonBH
        FOREIGN KEY (MaDBH)
        REFERENCES dbo.DonBH (MaDBH),
    CONSTRAINT FK_CTBH_SanPham
        FOREIGN KEY (MaSP)
        REFERENCES dbo.SanPham (MaSP)

);
GO

-- 17. Giỏ hàng của khách hàng
CREATE TABLE dbo.GioHang
(
    MaGH int IDENTITY(1,1) NOT NULL,
    MaKH smallint NOT NULL,
    NgayCapNhat datetime2 NOT NULL CONSTRAINT DF_GioHang_NgayCapNhat DEFAULT (SYSDATETIME()),

    CONSTRAINT PK_GioHang PRIMARY KEY (MaGH),
    CONSTRAINT UQ_GioHang_MaKH UNIQUE (MaKH),
    CONSTRAINT FK_GioHang_KhachHang FOREIGN KEY (MaKH)
        REFERENCES dbo.KhachHang (MaKH)
);
GO

-- 18. Các sản phẩm đang nằm trong giỏ; đây không phải chi tiết bán hàng.
CREATE TABLE dbo.CTGioHang
(
    MaGH int NOT NULL,
    MaSP char(9) NOT NULL,
    SoLuong smallint NOT NULL,

    CONSTRAINT PK_CTGioHang PRIMARY KEY (MaGH, MaSP),
    CONSTRAINT CK_CTGioHang_SoLuong CHECK (SoLuong BETWEEN 1 AND 255),
    CONSTRAINT FK_CTGioHang_GioHang FOREIGN KEY (MaGH)
        REFERENCES dbo.GioHang (MaGH),
    CONSTRAINT FK_CTGioHang_SanPham FOREIGN KEY (MaSP)
        REFERENCES dbo.SanPham (MaSP)
);
GO

-- 19. Quyền thao tác theo bảng (được dùng bởi Insert.sql)
CREATE TABLE dbo.QuyenBang
(
    MaQuyen     tinyint      NOT NULL,
    TenBang     nvarchar(50) NOT NULL,
    ChoPhepThem bit          NOT NULL,
    ChoPhepXem  bit          NOT NULL,
    ChoPhepSua  bit          NOT NULL,
    ChoPhepXoa  bit          NOT NULL,

    CONSTRAINT PK_QuyenBang PRIMARY KEY (MaQuyen, TenBang),
    CONSTRAINT FK_QuyenBang_Quyen FOREIGN KEY (MaQuyen)
        REFERENCES dbo.Quyen (MaQuyen)
);
GO
