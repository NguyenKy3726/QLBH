using Microsoft.AspNetCore.Mvc;
using QLBH.Data;

namespace QLBH.Controllers
{
    public class AccountController: Controller
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
            var MaNV = _context.NhanVien_DangNhap(TenDN, MatKhau);
            if (MaNV == 0)
            {
                ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu.");
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            // đọc thông tin chi tiết nhân viên và Lưu vào session
            var nv = _context.NhanVien_CT(MaNV);
            //Tự thêm vào vì có cảnh báo lỗi null 'nv'
            if (nv == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Không tìm thấy thông tin nhân viên."
                );

                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            HttpContext.Session.SetString("Username", nv.TenDN);
            HttpContext.Session.SetString("DisplayName", nv.HoNV + " " + nv.TenNV);

            // Redirect về returnUrl nếu an toàn
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "NhanViens");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("DisplayName");
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
