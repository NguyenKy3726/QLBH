using Microsoft.AspNetCore.Mvc;
using QLBH.Data;

namespace QLBH.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string TenDN, string MatKhau, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrWhiteSpace(TenDN) || string.IsNullOrWhiteSpace(MatKhau))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập tên đăng nhập và mật khẩu.");
                return View();
            }

            var maNV = _context.NhanVien_DangNhap(TenDN, MatKhau);
            if (maNV != 0)
            {
                var nv = _context.NhanVien_CT(maNV);
                if (nv == null)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin nhân viên.");
                    return View();
                }

                HttpContext.Session.Clear();
                HttpContext.Session.SetString("Role", "NhanVien");
                HttpContext.Session.SetString("UserType", "NhanVien");
                HttpContext.Session.SetString("Username", nv.TenDN);
                HttpContext.Session.SetString("DisplayName", $"{nv.HoNV} {nv.TenNV}");
                HttpContext.Session.SetInt32("MaNV", nv.MaNV);
                if (!string.IsNullOrWhiteSpace(nv.AnhNV))
                    HttpContext.Session.SetString("AvatarPath", nv.AnhNV);

                foreach (var tenQuyen in _context.NhanVien_QuyenDanhSach(nv.MaNV))
                    HttpContext.Session.SetString("Permission:" + tenQuyen.ToUpperInvariant(), "1");

                return RedirectAfterLogin(returnUrl);
            }

            var maKH = _context.KhachHang_DangNhap(TenDN, MatKhau);
            if (maKH != 0)
            {
                var kh = _context.KhachHang_CT(maKH);
                if (kh == null)
                {
                    ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin khách hàng.");
                    return View();
                }

                HttpContext.Session.Clear();
                HttpContext.Session.SetString("Role", "KhachHang");
                HttpContext.Session.SetString("UserType", "KhachHang");
                HttpContext.Session.SetString("Username", kh.TenDN);
                HttpContext.Session.SetString("DisplayName", $"{kh.HoKH} {kh.TenKH}");
                HttpContext.Session.SetInt32("MaKH", kh.MaKH);
                if (!string.IsNullOrWhiteSpace(kh.AnhKH))
                    HttpContext.Session.SetString("AvatarPath", kh.AnhKH);

                return RedirectAfterLogin(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout(string? returnUrl = null)
        {
            HttpContext.Session.Clear();
            return Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl!)
                : RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        private IActionResult RedirectAfterLogin(string? returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                ? LocalRedirect(returnUrl!)
                : RedirectToAction("Index", "Home");
        }
    }
}
