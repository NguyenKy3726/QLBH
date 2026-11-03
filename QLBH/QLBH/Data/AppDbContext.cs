using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace QLBH.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts)
            : base(opts) { }

        // 1. Thao tác trên bảng DonViTinh
        public DbSet<DonViTinh> DonViTinhs { get; set; }

        public List<DonViTinh> DonViTinh_DS()
        {
            return DonViTinhs
               .FromSqlRaw("EXEC dbo.DonViTinh_DS")
                .ToList();
        }

        public DonViTinh? DonViTinh_CT(byte MaDVT)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaDVT", MaDVT)
            };
            return DonViTinhs.FromSqlRaw("EXEC DonViTinh_CT @MaDVT", p).ToList().FirstOrDefault();
        }

        public int DonViTinh_Them(DonViTinh dvt)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonViTinh_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenDVT", dvt.TenDVT));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonViTinh_Sua(DonViTinh dvt)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonViTinh_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDVT", dvt.MaDVT));
            cmd.Parameters.Add(new SqlParameter("@TenDVT", dvt.TenDVT));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonViTinh_Xoa(byte MaDVT)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonViTinh_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDVT", MaDVT));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 2. Thao tác trên bảng NhomSP
        public DbSet<NhomSP> NhomSPs { get; set; }

        public List<NhomSP> NhomSP_DS()
        {
            return NhomSPs
                .FromSqlRaw("EXEC dbo.NhomSP_DS")
                .ToList();
        }

        public NhomSP? NhomSP_CT(byte MaNSP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaNSP", MaNSP)
            };
            return NhomSPs.FromSqlRaw("EXEC NhomSP_CT @MaNSP", p).ToList().FirstOrDefault();
        }

        public int NhomSP_Them(NhomSP nsp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhomSP_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenNSP", nsp.TenNSP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhomSP_Sua(NhomSP nsp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhomSP_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaNSP", nsp.MaNSP));
            cmd.Parameters.Add(new SqlParameter("@TenNSP", nsp.TenNSP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhomSP_Xoa(byte MaNSP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhomSP_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaNSP", MaNSP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 3. Thao tác trên bảng LoaiSP
        public DbSet<LoaiSP> LoaiSPs { get; set; }

        public List<LoaiSP> LoaiSP_DS()
        {
            return LoaiSPs
                .FromSqlRaw("EXEC dbo.LoaiSP_DS")
                .ToList();
        }

        public LoaiSP? LoaiSP_CT(short MaLSP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaLSP", MaLSP)
            };
            return LoaiSPs.FromSqlRaw("EXEC LoaiSP_CT @MaLSP", p).ToList().FirstOrDefault();
        }

        public int LoaiSP_Them(LoaiSP lsp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "LoaiSP_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenLSP", lsp.TenLSP));
            cmd.Parameters.Add(new SqlParameter("@MaNSP", lsp.MaNSP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int LoaiSP_Sua(LoaiSP lsp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "LoaiSP_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaLSP", lsp.MaLSP));
            cmd.Parameters.Add(new SqlParameter("@TenLSP", lsp.TenLSP));
            cmd.Parameters.Add(new SqlParameter("@MaNSP", lsp.MaNSP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int LoaiSP_Xoa(short MaLSP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "LoaiSP_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaLSP", MaLSP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 4. Thao tác trên bảng SanPham
        public DbSet<SanPham> SanPhams { get; set; }

        public List<SanPham> SanPham_DS()
        {
            return SanPhams
                .FromSqlRaw("EXEC dbo.SanPham_DS")
                .ToList();
        }

        public SanPham? SanPham_CT(string MaSP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaSP", MaSP)
            };
            return SanPhams.FromSqlRaw("EXEC SanPham_CT @MaSP", p).ToList().FirstOrDefault();
        }

        public int SanPham_Them(SanPham sp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SanPham_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenSP", sp.TenSP));
            cmd.Parameters.Add(new SqlParameter("@DonGia", sp.DonGia));
            cmd.Parameters.Add(new SqlParameter("@AnhSP", (object?)sp.AnhSP ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MoTaCT", (object?)sp.MoTaCT ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaLSP", sp.MaLSP));
            cmd.Parameters.Add(new SqlParameter("@MaDVT", sp.MaDVT));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int SanPham_Sua(SanPham sp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SanPham_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaSP", sp.MaSP));
            cmd.Parameters.Add(new SqlParameter("@TenSP", sp.TenSP));
            cmd.Parameters.Add(new SqlParameter("@DonGia", sp.DonGia));
            cmd.Parameters.Add(new SqlParameter("@AnhSP", (object?)sp.AnhSP ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MoTaCT", (object?)sp.MoTaCT ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaLSP", sp.MaLSP));
            cmd.Parameters.Add(new SqlParameter("@MaDVT", sp.MaDVT));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int SanPham_Xoa(string MaSP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SanPham_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaSP", MaSP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 5. Thao tác trên bảng Tinh
        public DbSet<Tinh> Tinhs { get; set; }

        public List<Tinh> Tinh_DS()
        {
            return Tinhs
                .FromSqlRaw("EXEC dbo.Tinh_DS")
                .ToList();
        }

        public Tinh? Tinh_CT(string MaTinh)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaTinh", MaTinh)
            };
            return Tinhs.FromSqlRaw("EXEC Tinh_CT @MaTinh", p).ToList().FirstOrDefault();
        }

        public int Tinh_Them(Tinh tinh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Tinh_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenTinh", tinh.TenTinh));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int Tinh_Sua(Tinh tinh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Tinh_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaTinh", tinh.MaTinh));
            cmd.Parameters.Add(new SqlParameter("@TenTinh", tinh.TenTinh));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int Tinh_Xoa(string MaTinh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Tinh_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaTinh", MaTinh));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 6. Thao tác trên bảng XaPhuong
        public DbSet<XaPhuong> XaPhuongs { get; set; }

        public List<XaPhuong> XaPhuong_DS()
        {
            return XaPhuongs
                .FromSqlRaw("EXEC dbo.XaPhuong_DS")
                .ToList();
        }

        public XaPhuong? XaPhuong_CT(string MaXP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaXP", MaXP)
            };
            return XaPhuongs.FromSqlRaw("EXEC XaPhuong_CT @MaXP", p).ToList().FirstOrDefault();
        }

        public int XaPhuong_Them(XaPhuong xp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "XaPhuong_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenXP", xp.TenXP));
            cmd.Parameters.Add(new SqlParameter("@MaTinh", xp.MaTinh));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int XaPhuong_Sua(XaPhuong xp)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "XaPhuong_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaXP", xp.MaXP));
            cmd.Parameters.Add(new SqlParameter("@TenXP", xp.TenXP));
            cmd.Parameters.Add(new SqlParameter("@MaTinh", xp.MaTinh));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public int XaPhuong_Xoa(string MaXP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "XaPhuong_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaXP", MaXP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 7. Thao tác trên bảng PhongBan
        public DbSet<PhongBan> PhongBans { get; set; }

        public List<PhongBan> PhongBan_DS()
        {
            return PhongBans
                .FromSqlRaw("EXEC dbo.PhongBan_DS")
                .ToList();
        }

        public PhongBan? PhongBan_CT(byte MaPB)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaPB", MaPB)
            };
            return PhongBans.FromSqlRaw("EXEC PhongBan_CT @MaPB", p).ToList().FirstOrDefault();
        }

        public int PhongBan_Them(PhongBan pb)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PhongBan_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenPB", pb.TenPB));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int PhongBan_Sua(PhongBan pb)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PhongBan_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaPB", pb.MaPB));
            cmd.Parameters.Add(new SqlParameter("@TenPB", pb.TenPB));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int PhongBan_Xoa(byte MaPB)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PhongBan_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaPB", MaPB));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 8. Thao tác trên bảng Quyen
        public DbSet<Quyen> Quyens { get; set; }

        public List<Quyen> Quyen_DS()
        {
            return Quyens
                .FromSqlRaw("EXEC dbo.Quyen_DS")
                .ToList();
        }

        public Quyen? Quyen_CT(byte MaQuyen)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaQuyen", MaQuyen)
            };
            return Quyens.FromSqlRaw("EXEC Quyen_CT @MaQuyen", p).ToList().FirstOrDefault();
        }

        public int Quyen_Them(Quyen quyen)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Quyen_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenQuyen", quyen.TenQuyen));
            cmd.Parameters.Add(new SqlParameter("@MoTa", (object?)quyen.MoTa ?? DBNull.Value));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int Quyen_Sua(Quyen quyen)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Quyen_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaQuyen", quyen.MaQuyen));
            cmd.Parameters.Add(new SqlParameter("@TenQuyen", quyen.TenQuyen));
            cmd.Parameters.Add(new SqlParameter("@MoTa", (object?)quyen.MoTa ?? DBNull.Value));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int Quyen_Xoa(byte MaQuyen)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "Quyen_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaQuyen", MaQuyen));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }
        // 9. Thao tác trên bảng NhanVien
        public DbSet<NhanVien> NhanViens { get; set; }

        public List<NhanVien> NhanVien_DS()
        {
            return NhanViens
                .FromSqlRaw("EXEC dbo.NhanVien_DS")
                .ToList();
        }

        public NhanVien? NhanVien_CT(byte MaNV)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaNV", MaNV)
            };
            return NhanViens.FromSqlRaw("EXEC NhanVien_CT @MaNV", p).ToList().FirstOrDefault();
        }

        public int NhanVien_Them(NhanVien nv)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhanVien_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HoNV", nv.HoNV));
            cmd.Parameters.Add(new SqlParameter("@TenNV", nv.TenNV));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", nv.DienThoai));
            cmd.Parameters.Add(new SqlParameter("@Email", nv.Email));
            cmd.Parameters.Add(new SqlParameter("@TenDN", nv.TenDN));
            cmd.Parameters.Add(new SqlParameter("@MatKhau", nv.MatKhau));
            cmd.Parameters.Add(new SqlParameter("@AnhNV", (object?)nv.AnhNV ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaPB", nv.MaPB));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhanVien_Sua(NhanVien nv, string? matkhaumoi)    
        {
            var cmd = Database.GetDbConnection().CreateCommand();

            cmd.CommandText = "NhanVien_Sua";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@MaNV", nv.MaNV));
            cmd.Parameters.Add(new SqlParameter("@HoNV", nv.HoNV));
            cmd.Parameters.Add(new SqlParameter("@TenNV", nv.TenNV));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", nv.DienThoai));
            cmd.Parameters.Add(new SqlParameter("@Email", nv.Email));
            cmd.Parameters.Add(new SqlParameter("@TenDN", nv.TenDN));
            cmd.Parameters.Add(new SqlParameter("@AnhNV", (object?)nv.AnhNV ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaPB", nv.MaPB));
            cmd.Parameters.Add(new SqlParameter("@MatKhauMoi", SqlDbType.NVarChar, 50)
            {
                Value = (object?)matkhaumoi ?? DBNull.Value
            });

            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhanVien_Xoa(byte MaNV)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhanVien_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaNV", MaNV));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public short KhachHang_DangNhap(string tenDN, string matKhau)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "KhachHang_DangNhap";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenDN", tenDN));
            cmd.Parameters.Add(new SqlParameter("@MatKhau", matKhau));

            Database.OpenConnection();
            try
            {
                using var reader = cmd.ExecuteReader();
                return reader.Read() ? reader.GetInt16(reader.GetOrdinal("MaKH")) : (short)0;
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        public byte NhanVien_DangNhap(string TenDN, string MatKhau) //Trả về Mã nhân viên nếu đăng nhập thành công
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhanVien_DangNhap";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenDN", TenDN));
            cmd.Parameters.Add(new SqlParameter("@MatKhau", MatKhau));
            Database.OpenConnection();
            var reader = cmd.ExecuteReader();
            byte maNV = 0;
            if (reader.Read())//Nếu có dữ liệu để đọc
            {
                maNV = reader.GetByte(reader.GetOrdinal("MaNV"));
            }
            Database.CloseConnection();
            return maNV;
        }

        // 10. Thao tác trên bảng PhanQuyen
        public DbSet<PhanQuyen> PhanQuyens { get; set; }

        public List<PhanQuyen> PhanQuyen_DS()
        {
            return PhanQuyens
                .FromSqlRaw("EXEC dbo.PhanQuyen_DS")
                .ToList();
        }

        public PhanQuyen? PhanQuyen_CT(byte maquyen, byte manv)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaQuyen", maquyen),
                new Microsoft.Data.SqlClient.SqlParameter("MaNV", manv)
            };
            return PhanQuyens.FromSqlRaw("EXEC PhanQuyen_CT @MaQuyen, @MaNV", p).ToList().FirstOrDefault();
        }

        public int PhanQuyen_Them(PhanQuyen pq)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PhanQuyen_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaQuyen", pq.MaQuyen));
            cmd.Parameters.Add(new SqlParameter("@MaNV", pq.MaNV));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int PhanQuyen_Xoa(byte MaQuyen, byte MaNV)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "PhanQuyen_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaQuyen", MaQuyen));
            cmd.Parameters.Add(new SqlParameter("@MaNV", MaNV));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 11. Thao tác trên bảng NhaCC
        public DbSet<NhaCC> NhaCCs { get; set; }

        public List<NhaCC> NhaCC_DS()
        {
            return NhaCCs
                .FromSqlRaw("EXEC dbo.NhaCC_DS")
                .ToList();
        }

        public NhaCC? NhaCC_CT(short MaNCC)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaNCC", MaNCC)
            };
            return NhaCCs.FromSqlRaw("EXEC NhaCC_CT @MaNCC", p).ToList().FirstOrDefault();
        }

        public int NhaCC_Them(NhaCC ncc)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhaCC_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TenNCC", ncc.TenNCC));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", (object?)ncc.DienThoai ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object?)ncc.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DiaChi", ncc.DiaChi));
            cmd.Parameters.Add(new SqlParameter("@MaXP", ncc.MaXP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhaCC_Sua(NhaCC ncc)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhaCC_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaNCC", ncc.MaNCC));
            cmd.Parameters.Add(new SqlParameter("@TenNCC", ncc.TenNCC));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", (object?)ncc.DienThoai ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object?)ncc.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DiaChi", ncc.DiaChi));
            cmd.Parameters.Add(new SqlParameter("@MaXP", ncc.MaXP));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int NhaCC_Xoa(short MaNCC)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "NhaCC_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaNCC", MaNCC));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 12. Thao tác trên bảng DonMH
        public DbSet<DonMH> DonMHs { get; set; }

        public List<DonMH> DonMH_DS()
        {
            return DonMHs
                .FromSqlRaw("EXEC dbo.DonMH_DS")
                .ToList();
        }

        public DonMH? DonMH_CT(string MaDMH)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaDMH", MaDMH)
            };
            return DonMHs.FromSqlRaw("EXEC DonMH_CT @MaDMH", p).ToList().FirstOrDefault();
        }

        public int DonMH_Them(DonMH dmh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonMH_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@NgayDat", dmh.NgayDat));
            cmd.Parameters.Add(new SqlParameter("@NgayGiao", (object?)dmh.NgayGiao ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaNV", dmh.MaNV));
            cmd.Parameters.Add(new SqlParameter("@MaNCC", dmh.MaNCC));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonMH_Sua(DonMH dmh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonMH_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDMH", dmh.MaDMH));
            cmd.Parameters.Add(new SqlParameter("@NgayDat", dmh.NgayDat));
            cmd.Parameters.Add(new SqlParameter("@NgayGiao", (object?)dmh.NgayGiao ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaNV", dmh.MaNV));
            cmd.Parameters.Add(new SqlParameter("@MaNCC", dmh.MaNCC));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonMH_Xoa(string MaDMH)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonMH_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDMH", MaDMH));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 13. Thao tác trên bảng CTMH
        public DbSet<CTMH> CTMHs { get; set; }

        public List<CTMH> CTMH_DS()
        {
            return CTMHs
                .FromSqlRaw("EXEC dbo.CTMH_DS")
                .ToList();
        }

        public CTMH? CTMH_CT(string MaDMH, string MaSP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaDMH", MaDMH),
                new Microsoft.Data.SqlClient.SqlParameter("MaSP", MaSP)
            };
            return CTMHs.FromSqlRaw("EXEC CTMH_CT @MaDMH, @MaSP", p).ToList().FirstOrDefault();
        }

        public int CTMH_Them(CTMH ctmh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTMH_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDMH", ctmh.MaDMH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", ctmh.MaSP));
            cmd.Parameters.Add(new SqlParameter("@SLM", ctmh.SLM));
            cmd.Parameters.Add(new SqlParameter("@DGM", ctmh.DGM));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int CTMH_Sua(CTMH ctmh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTMH_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDMH", ctmh.MaDMH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", ctmh.MaSP));
            cmd.Parameters.Add(new SqlParameter("@SLM", ctmh.SLM));
            cmd.Parameters.Add(new SqlParameter("@DGM", ctmh.DGM));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int CTMH_Xoa(string MaDMH, string MaSP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTMH_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDMH", MaDMH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", MaSP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 14. Thao tác trên bảng KhachHang
        public DbSet<KhachHang> KhachHangs { get; set; }

        public List<KhachHang> KhachHang_DS()
        {
            return KhachHangs
                .FromSqlRaw("EXEC dbo.KhachHang_DS")
                .ToList();
        }

        public KhachHang? KhachHang_CT(short MaKH)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaKH", MaKH)
            };
            return KhachHangs.FromSqlRaw("EXEC KhachHang_CT @MaKH", p).ToList().FirstOrDefault();
        }

        public int KhachHang_Them(KhachHang kh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "KhachHang_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@HoKH", kh.HoKH));
            cmd.Parameters.Add(new SqlParameter("@TenKH", kh.TenKH));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", (object?)kh.DienThoai ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object?)kh.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DiaChi", kh.DiaChi));
            cmd.Parameters.Add(new SqlParameter("@MaXP", kh.MaXP));
            cmd.Parameters.Add(new SqlParameter("@TenDN", kh.TenDN));
            cmd.Parameters.Add(new SqlParameter("@MatKhau", kh.MatKhau));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int KhachHang_Sua(KhachHang kh, string? matkhaumoi)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "KhachHang_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaKH", kh.MaKH));
            cmd.Parameters.Add(new SqlParameter("@HoKH", kh.HoKH));
            cmd.Parameters.Add(new SqlParameter("@TenKH", kh.TenKH));
            cmd.Parameters.Add(new SqlParameter("@DienThoai", (object?)kh.DienThoai ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@Email", (object?)kh.Email ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DiaChi", kh.DiaChi));
            cmd.Parameters.Add(new SqlParameter("@MaXP", kh.MaXP));
            cmd.Parameters.Add(new SqlParameter("@TenDN", kh.TenDN));
            cmd.Parameters.Add(new SqlParameter("@matkhaumoi", SqlDbType.NVarChar, 50)
            {
                Value = (object?)matkhaumoi ?? DBNull.Value
            });
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int KhachHang_Xoa(short MaKH)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "KhachHang_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaKH", MaKH));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // 15. Thao tác trên bảng DonBH
        public DbSet<DonBH> DonBHs { get; set; }

        public List<DonBH> DonBH_DS()
        {
            return DonBHs
                .FromSqlRaw("EXEC dbo.DonBH_DS")
                .ToList();
        }

        public DonBH? DonBH_CT(string MaDBH)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaDBH", MaDBH)
            };
            return DonBHs.FromSqlRaw("EXEC DonBH_CT @MaDBH", p).ToList().FirstOrDefault();
        }

        public int DonBH_Them(DonBH dbh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonBH_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@NgayDat", dbh.NgayDat));
            cmd.Parameters.Add(new SqlParameter("@NgayGiao", (object?)dbh.NgayGiao ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DCGH", dbh.DCGH));
            cmd.Parameters.Add(new SqlParameter("@MaXP", dbh.MaXP));
            cmd.Parameters.Add(new SqlParameter("@MaNV", (object?)dbh.MaNV ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaKH", dbh.MaKH));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonBH_Sua(DonBH dbh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonBH_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDBH", dbh.MaDBH));
            cmd.Parameters.Add(new SqlParameter("@NgayDat", dbh.NgayDat));
            cmd.Parameters.Add(new SqlParameter("@NgayGiao", (object?)dbh.NgayGiao ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@DCGH", dbh.DCGH));
            cmd.Parameters.Add(new SqlParameter("@MaXP", dbh.MaXP));
            cmd.Parameters.Add(new SqlParameter("@MaNV", (object?)dbh.MaNV ?? DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@MaKH", dbh.MaKH));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int DonBH_Xoa(string MaDBH)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "DonBH_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDBH", MaDBH));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }
        // 16. Thao tác trên bảng CTBH
        public DbSet<CTBH> CTBHs { get; set; }

        public List<CTBH> CTBH_DS()
        {
            return CTBHs
                .FromSqlRaw("EXEC dbo.CTBH_DS")
                .ToList();
        }

        public CTBH? CTBH_CT(string MaDBH, string MaSP)
        {
            var p = new[]
            {
                new Microsoft.Data.SqlClient.SqlParameter("MaDBH", MaDBH),
                new Microsoft.Data.SqlClient.SqlParameter("MaSP", MaSP)
            };
            return CTBHs.FromSqlRaw("EXEC CTBH_CT @MaDBH, @MaSP", p).ToList().FirstOrDefault();
        }

        public int CTBH_Them(CTBH ctbh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTBH_Them";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDBH", ctbh.MaDBH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", ctbh.MaSP));
            cmd.Parameters.Add(new SqlParameter("@SLB", ctbh.SLB));
            cmd.Parameters.Add(new SqlParameter("@DGB", ctbh.DGB));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int CTBH_Sua(CTBH ctbh)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTBH_Sua";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDBH", ctbh.MaDBH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", ctbh.MaSP));
            cmd.Parameters.Add(new SqlParameter("@SLB", ctbh.SLB));
            cmd.Parameters.Add(new SqlParameter("@DGB", ctbh.DGB));
            Database.OpenConnection();
            var kq = cmd.ExecuteNonQuery();
            Database.CloseConnection();
            return kq;
        }

        public int CTBH_Xoa(string MaDBH, string MaSP)
        {
            var cmd = Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "CTBH_Xoa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MaDBH", MaDBH));
            cmd.Parameters.Add(new SqlParameter("@MaSP", MaSP));
            Database.OpenConnection();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                Database.CloseConnection();
            }
        }
        // Các thao tác giỏ hàng. Giỏ và chi tiết giỏ được lưu riêng với đơn bán hàng.
        public DbSet<GioHangItemViewModel> GioHangItems { get; set; } = null!;

        public List<GioHangItemViewModel> GioHang_DS(short maKH)
        {
            var p = new SqlParameter("@MaKH", maKH);
            return GioHangItems
                .FromSqlRaw("EXEC dbo.GioHang_DS @MaKH", p)
                .AsNoTracking()
                .ToList();
        }

        public int GioHang_Them(short maKH, string maSP, short soLuong)
        {
            return Database.ExecuteSqlRaw(
                "EXEC dbo.GioHang_Them @MaKH, @MaSP, @SoLuong",
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaSP", SqlDbType.Char, 9) { Value = maSP },
                new SqlParameter("@SoLuong", soLuong));
        }

        public int GioHang_CapNhat(short maKH, string maSP, short soLuong)
        {
            return Database.ExecuteSqlRaw(
                "EXEC dbo.GioHang_CapNhat @MaKH, @MaSP, @SoLuong",
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaSP", SqlDbType.Char, 9) { Value = maSP },
                new SqlParameter("@SoLuong", soLuong));
        }

        public int GioHang_Xoa(short maKH, string maSP)
        {
            return Database.ExecuteSqlRaw(
                "EXEC dbo.GioHang_Xoa @MaKH, @MaSP",
                new SqlParameter("@MaKH", maKH),
                new SqlParameter("@MaSP", SqlDbType.Char, 9) { Value = maSP });
        }

        public string? GioHang_DatHang(short maKH)
        {
            var connection = Database.GetDbConnection();
            Database.OpenConnection();
            try
            {
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "dbo.GioHang_DatHang";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MaKH", maKH));
                var result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? null : Convert.ToString(result);
            }
            finally
            {
                Database.CloseConnection();
            }
        }

        // Cấu hình khóa chính ghép
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GioHangItemViewModel>().HasNoKey();

            // PhanQuyen: khóa gồm MaQuyen và MaNV
            modelBuilder.Entity<PhanQuyen>()
                .HasKey(c => new { c.MaQuyen, c.MaNV });

            // CTMH: khóa gồm MaDMH và MaSP
            modelBuilder.Entity<CTMH>()
                .HasKey(c => new { c.MaDMH, c.MaSP });

            // CTBH: khóa gồm MaDBH và MaSP
            modelBuilder.Entity<CTBH>()
                .HasKey(c => new { c.MaDBH, c.MaSP });
        }
    }
}