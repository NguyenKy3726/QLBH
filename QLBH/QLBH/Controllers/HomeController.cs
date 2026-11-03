using Microsoft.AspNetCore.Mvc;
using QLBH.Filters;
using QLBH.Models;
using System.Diagnostics;

namespace QLBH.Controllers
{
    //[SessionAuthorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
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
