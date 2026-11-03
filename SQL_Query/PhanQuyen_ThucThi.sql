USE DbQLBH;
GO

/*
    Phân quyền theo ma trận bảng/CRUD.
    Có thể chạy lại an toàn: cấu hình của ADMIN, SALES, WAREHOUSE được đồng bộ
    đúng với ma trận đã thống nhất; không xóa dữ liệu nhân viên hay giao dịch.
*/
IF OBJECT_ID(N'dbo.QuyenBang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.QuyenBang
    (
        MaQuyen       tinyint     NOT NULL,
        TenBang       varchar(30) NOT NULL,
        ChoPhepThem   bit         NOT NULL CONSTRAINT DF_QuyenBang_C DEFAULT (0),
        ChoPhepXem    bit         NOT NULL CONSTRAINT DF_QuyenBang_R DEFAULT (0),
        ChoPhepSua    bit         NOT NULL CONSTRAINT DF_QuyenBang_U DEFAULT (0),
        ChoPhepXoa    bit         NOT NULL CONSTRAINT DF_QuyenBang_D DEFAULT (0),

        CONSTRAINT PK_QuyenBang PRIMARY KEY (MaQuyen, TenBang),
        CONSTRAINT FK_QuyenBang_Quyen FOREIGN KEY (MaQuyen)
            REFERENCES dbo.Quyen(MaQuyen)
    );
END;
GO

SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS
    (
        SELECT TenQuyen
        FROM dbo.Quyen
        WHERE TenQuyen IN (N'ADMIN', N'SALES', N'WAREHOUSE')
        GROUP BY TenQuyen
        HAVING COUNT(*) > 1
    )
        THROW 51000, N'Bảng Quyen đang có tên vai trò bị trùng. Hãy xử lý bản ghi trùng trước.', 1;

    DECLARE @TenQuyen nvarchar(50);
    DECLARE @MaMoi smallint;

    DECLARE role_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT v.TenQuyen
        FROM (VALUES (N'ADMIN'), (N'SALES'), (N'WAREHOUSE')) AS v(TenQuyen)
        WHERE NOT EXISTS
        (
            SELECT 1 FROM dbo.Quyen q WHERE q.TenQuyen = v.TenQuyen
        );

    OPEN role_cursor;
    FETCH NEXT FROM role_cursor INTO @TenQuyen;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @MaMoi = ISNULL(MAX(CONVERT(smallint, MaQuyen)), 0) + 1
        FROM dbo.Quyen;

        IF @MaMoi > 255
            THROW 51001, N'Bảng Quyen đã hết mã tinyint để tạo vai trò.', 1;

        INSERT dbo.Quyen(MaQuyen, TenQuyen, MoTa)
        VALUES
        (
            CONVERT(tinyint, @MaMoi),
            @TenQuyen,
            CASE @TenQuyen
                WHEN N'ADMIN' THEN N'Toàn quyền trên 16 bảng.'
                WHEN N'SALES' THEN N'Quyền bán hàng theo ma trận phân quyền.'
                ELSE N'Quyền nhập hàng và xem tồn/nghiệp vụ liên quan theo ma trận.'
            END
        );

        FETCH NEXT FROM role_cursor INTO @TenQuyen;
    END;

    CLOSE role_cursor;
    DEALLOCATE role_cursor;

    DECLARE @Bang TABLE (TenBang varchar(30) NOT NULL PRIMARY KEY);
    INSERT @Bang(TenBang) VALUES
        ('DonViTinh'), ('NhomSP'), ('LoaiSP'), ('SanPham'),
        ('Tinh'), ('XaPhuong'), ('PhongBan'), ('Quyen'),
        ('NhanVien'), ('PhanQuyen'), ('NhaCC'), ('DonMH'),
        ('CTMH'), ('KhachHang'), ('DonBH'), ('CTBH');

    DELETE qb
    FROM dbo.QuyenBang qb
    INNER JOIN dbo.Quyen q ON q.MaQuyen = qb.MaQuyen
    WHERE q.TenQuyen IN (N'ADMIN', N'SALES', N'WAREHOUSE');

    -- ADMIN: CRUD đủ 16 bảng.
    INSERT dbo.QuyenBang(MaQuyen, TenBang, ChoPhepThem, ChoPhepXem, ChoPhepSua, ChoPhepXoa)
    SELECT q.MaQuyen, b.TenBang, 1, 1, 1, 1
    FROM dbo.Quyen q CROSS JOIN @Bang b
    WHERE q.TenQuyen = N'ADMIN';

    -- SALES: R danh mục/địa giới; CRU khách hàng, đơn bán và chi tiết bán.
    INSERT dbo.QuyenBang(MaQuyen, TenBang, ChoPhepXem)
    SELECT q.MaQuyen, b.TenBang, 1
    FROM dbo.Quyen q
    CROSS JOIN (VALUES ('DonViTinh'), ('NhomSP'), ('LoaiSP'), ('SanPham'), ('Tinh'), ('XaPhuong')) b(TenBang)
    WHERE q.TenQuyen = N'SALES';

    INSERT dbo.QuyenBang(MaQuyen, TenBang, ChoPhepThem, ChoPhepXem, ChoPhepSua)
    SELECT q.MaQuyen, b.TenBang, 1, 1, 1
    FROM dbo.Quyen q
    CROSS JOIN (VALUES ('KhachHang'), ('DonBH'), ('CTBH')) b(TenBang)
    WHERE q.TenQuyen = N'SALES';

    -- WAREHOUSE: R danh mục/địa giới và đơn bán; CRU nhà cung cấp/đơn mua/chi tiết mua.
    INSERT dbo.QuyenBang(MaQuyen, TenBang, ChoPhepXem)
    SELECT q.MaQuyen, b.TenBang, 1
    FROM dbo.Quyen q
    CROSS JOIN (VALUES ('DonViTinh'), ('NhomSP'), ('LoaiSP'), ('SanPham'), ('Tinh'), ('XaPhuong'), ('DonBH'), ('CTBH')) b(TenBang)
    WHERE q.TenQuyen = N'WAREHOUSE';

    INSERT dbo.QuyenBang(MaQuyen, TenBang, ChoPhepThem, ChoPhepXem, ChoPhepSua)
    SELECT q.MaQuyen, b.TenBang, 1, 1, 1
    FROM dbo.Quyen q
    CROSS JOIN (VALUES ('NhaCC'), ('DonMH'), ('CTMH')) b(TenBang)
    WHERE q.TenQuyen = N'WAREHOUSE';

    COMMIT TRANSACTION;

    SELECT q.MaQuyen, q.TenQuyen, qb.TenBang,
           qb.ChoPhepThem, qb.ChoPhepXem, qb.ChoPhepSua, qb.ChoPhepXoa
    FROM dbo.Quyen q
    JOIN dbo.QuyenBang qb ON qb.MaQuyen = q.MaQuyen
    WHERE q.TenQuyen IN (N'ADMIN', N'SALES', N'WAREHOUSE')
    ORDER BY q.TenQuyen, qb.TenBang;
END TRY
BEGIN CATCH
    IF CURSOR_STATUS('local', 'role_cursor') >= 0
    BEGIN
        CLOSE role_cursor;
        DEALLOCATE role_cursor;
    END
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO
