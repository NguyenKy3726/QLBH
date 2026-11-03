using Microsoft.AspNetCore.Mvc;
using QLBH.Data;

namespace QLBH.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context) { _context = context; }

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
            var maNV = _context.NhanVien_DangNhap(TenDN, MatKhau);
            if (maNV == 0)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            var nv = _context.NhanVien_CT(maNV);
            if (nv == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin nhân viên.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString("UserType", "NhanVien");
            HttpContext.Session.SetString("Username", nv.TenDN);
            HttpContext.Session.SetString("DisplayName", nv.HoNV + " " + nv.TenNV);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "NhanViens");
        }

        [HttpGet]
        public IActionResult KhachHangDangNhap(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KhachHangDangNhap(string TenDN, string MatKhau, string? returnUrl = null)
        {
            var maKH = _context.KhachHang_DangNhap(TenDN, MatKhau);
            if (maKH == 0)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            var kh = _context.KhachHang_CT(maKH);
            if (kh == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin khách hàng.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            HttpContext.Session.Clear();
            HttpContext.Session.SetString("UserType", "KhachHang");
            HttpContext.Session.SetString("MaKH", maKH.ToString());
            HttpContext.Session.SetString("DisplayName", $"{kh.HoKH} {kh.TenKH}");

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "SanPhams");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
