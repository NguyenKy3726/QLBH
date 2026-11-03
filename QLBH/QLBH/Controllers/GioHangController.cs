using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using QLBH.Data;

namespace QLBH.Controllers
{
    public class GioHangController : Controller
    {
        private readonly AppDbContext _context;

        public GioHangController(AppDbContext context) { _context = context; }

        public IActionResult Index()
        {
            var maKH = CurrentCustomerId();
            if (maKH == null) return RedirectToCustomerLogin();

            var items = _context.GioHang_DS(maKH.Value);
            ViewBag.TongTien = items.Sum(item => item.ThanhTien);
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Them(string maSP, short soLuong = 1)
        {
            var maKH = CurrentCustomerId();
            if (maKH == null) return RedirectToCustomerLogin();
            if (string.IsNullOrWhiteSpace(maSP) || soLuong < 1 || soLuong > 255)
            {
                TempData["LoiGioHang"] = "Sản phẩm hoặc số lượng không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.GioHang_Them(maKH.Value, maSP, soLuong);
                TempData["ThongBaoGioHang"] = "Đã thêm sản phẩm vào giỏ hàng.";
            }
            catch (SqlException ex) { TempData["LoiGioHang"] = ex.Message; }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CapNhat(string maSP, short soLuong)
        {
            var maKH = CurrentCustomerId();
            if (maKH == null) return RedirectToCustomerLogin();

            try
            {
                _context.GioHang_CapNhat(maKH.Value, maSP, soLuong);
                TempData["ThongBaoGioHang"] = "Đã cập nhật số lượng.";
            }
            catch (SqlException ex) { TempData["LoiGioHang"] = ex.Message; }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Xoa(string maSP)
        {
            var maKH = CurrentCustomerId();
            if (maKH == null) return RedirectToCustomerLogin();

            _context.GioHang_Xoa(maKH.Value, maSP);
            TempData["ThongBaoGioHang"] = "Đã xóa sản phẩm khỏi giỏ.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DatHang()
        {
            var maKH = CurrentCustomerId();
            if (maKH == null) return RedirectToCustomerLogin();

            try
            {
                var maDBH = _context.GioHang_DatHang(maKH.Value);
                if (string.IsNullOrWhiteSpace(maDBH))
                {
                    TempData["LoiGioHang"] = "Chưa tạo được đơn hàng.";
                    return RedirectToAction(nameof(Index));
                }
                TempData["MaDBH"] = maDBH.Trim();
                return RedirectToAction(nameof(ThanhCong));
            }
            catch (SqlException ex)
            {
                TempData["LoiGioHang"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult ThanhCong()
        {
            if (CurrentCustomerId() == null) return RedirectToCustomerLogin();
            ViewBag.MaDBH = TempData["MaDBH"];
            if (ViewBag.MaDBH == null)
                return RedirectToAction(nameof(Index));
            return View();
        }

        private short? CurrentCustomerId() =>
            short.TryParse(HttpContext.Session.GetString("MaKH"), out var maKH) ? maKH : null;

        private IActionResult RedirectToCustomerLogin()
        {
            var returnUrl = Url.Action(nameof(Index), "GioHang");
            return RedirectToAction("KhachHangDangNhap", "Account", new { returnUrl });
        }
    }
}
