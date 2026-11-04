using Microsoft.AspNetCore.Mvc;
using QLBH.Data;
using System.Diagnostics;
using QLBH.Models;

namespace QLBH.Controllers
{
    //[SessionAuthorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Chỉ đọc danh mục và một số sản phẩm để trình bày trang chủ.
            var productTypes = _context.LoaiSP_DS();
            ViewBag.DanhMuc = productTypes.ToDictionary(item => item.MaLSP, item => item.TenLSP);
            ViewBag.TenLSP = ViewBag.DanhMuc;
            ViewBag.TenDVT = _context.DonViTinh_DS()
                .ToDictionary(item => item.MaDVT, item => item.TenDVT);
            ViewBag.GoiY = _context.SanPham_DS().Take(4).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
