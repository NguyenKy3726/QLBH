using Microsoft.AspNetCore.Mvc;
using QLBH.Data;
using QLBH.Filters;
using QLBH.Models;

namespace QLBH.Controllers;

[SessionAuthorize]
public class QuanLyController : Controller
{
    private readonly AppDbContext _context;

    public QuanLyController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Role") != "NhanVien")
            return RedirectToAction("AccessDenied", "Account");

        var maNV = HttpContext.Session.GetInt32("MaNV");
        if (maNV is null || maNV < byte.MinValue || maNV > byte.MaxValue)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        var laAdmin = HttpContext.Session.GetString("Permission:ADMIN") == "1";
        var bangDuocXem = laAdmin
            ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            : _context.NhanVien_BangDuocXem((byte)maNV.Value);

        var cacNhom = new List<QuanLyNhomViewModel>();
        ThemNhom(cacNhom, TaoNhom(bangDuocXem, laAdmin, "Danh mục sản phẩm",
            "Quản lý thông tin và cách phân loại sản phẩm.",
            "▦",
            ("DonViTinh", "Đơn vị tính", "Quản lý đơn vị tính của sản phẩm.", "DonViTinhs"),
            ("NhomSP", "Nhóm sản phẩm", "Quản lý các nhóm sản phẩm.", "NhomSPs"),
            ("LoaiSP", "Loại sản phẩm", "Quản lý các loại thuộc từng nhóm.", "LoaiSPs"),
            ("SanPham", "Sản phẩm", "Tra cứu và quản lý thông tin sản phẩm.", "SanPhams")));

        ThemNhom(cacNhom, TaoNhom(bangDuocXem, laAdmin, "Địa bàn",
            "Quản lý tỉnh và xã/phường dùng trong địa chỉ.",
            "⌖",
            ("Tinh", "Tỉnh", "Tra cứu và quản lý danh sách tỉnh.", "Tinhs"),
            ("XaPhuong", "Xã/phường", "Quản lý xã/phường theo tỉnh.", "XaPhuongs")));

        ThemNhom(cacNhom, TaoNhom(bangDuocXem, laAdmin, "Mua hàng",
            "Theo dõi nhà cung cấp và các đơn mua hàng.",
            "↙",
            ("NhaCC", "Nhà cung cấp", "Tra cứu thông tin nhà cung cấp.", "NhaCCs"),
            ("DonMH", "Đơn mua hàng", "Quản lý đơn đặt mua hàng.", "DonMHs"),
            ("CTMH", "Chi tiết mua hàng", "Xem sản phẩm và số lượng trong đơn mua.", "CTMHs")));

        ThemNhom(cacNhom, TaoNhom(bangDuocXem, laAdmin, "Bán hàng",
            "Quản lý khách hàng và các đơn bán hàng.",
            "↗",
            ("KhachHang", "Khách hàng", "Tra cứu và quản lý thông tin khách hàng.", "KhachHangs"),
            ("DonBH", "Đơn bán hàng", "Quản lý đơn bán hàng.", "DonBHs"),
            ("CTBH", "Chi tiết bán hàng", "Xem sản phẩm và số lượng trong đơn bán.", "CTBHs")));

        ThemNhom(cacNhom, TaoNhom(bangDuocXem, laAdmin, "Tổ chức và phân quyền",
            "Quản lý nhân sự, phòng ban và quyền truy cập.",
            "⚙",
            ("PhongBan", "Phòng ban", "Quản lý các phòng ban.", "PhongBans"),
            ("NhanVien", "Nhân viên", "Tra cứu và quản lý tài khoản nhân viên.", "NhanViens"),
            ("Quyen", "Quyền", "Xem các quyền được khai báo trong hệ thống.", "Quyens"),
            ("PhanQuyen", "Phân quyền", "Quản lý quyền được gán cho nhân viên.", "PhanQuyens")));

        return View(new QuanLyViewModel
        {
            TenNhanVien = HttpContext.Session.GetString("DisplayName") ?? "Nhân viên",
            NhomChucNang = cacNhom
        });
    }

    private static QuanLyNhomViewModel? TaoNhom(
        HashSet<string> bangDuocXem,
        bool laAdmin,
        string tieuDe,
        string moTa,
        string bieuTuong,
        params (string Bang, string TieuDe, string MoTa, string Controller)[] muc)
    {
        var chucNang = muc
            .Where(m => laAdmin || bangDuocXem.Contains(m.Bang))
            .Select(m => new QuanLyChucNangViewModel
            {
                TieuDe = m.TieuDe,
                MoTa = m.MoTa,
                Controller = m.Controller
            })
            .ToList();

        return chucNang.Count == 0
            ? null
            : new QuanLyNhomViewModel
            {
                TieuDe = tieuDe,
                MoTa = moTa,
                BieuTuong = bieuTuong,
                ChucNang = chucNang
            };
    }

    private static void ThemNhom(
        List<QuanLyNhomViewModel> danhSach,
        QuanLyNhomViewModel? nhom)
    {
        if (nhom is not null)
            danhSach.Add(nhom);
    }
}
