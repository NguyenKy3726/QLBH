using Microsoft.AspNetCore.Mvc;
using QLBH.Data;

namespace QLBH.Controllers;

public class ImagesController : Controller
{
    private readonly AppDbContext _context;

    public ImagesController(AppDbContext context) => _context = context;

    [HttpGet("/media/product/{id}")]
    public IActionResult SanPham(string id) => Send("product", id);

    [HttpGet("/media/employee/{id}")]
    public IActionResult NhanVien(string id) => Send("employee", id);

    [HttpGet("/media/customer/{id}")]
    public IActionResult KhachHang(string id) => Send("customer", id);

    private IActionResult Send(string kind, string id)
    {
        var image = _context.StoredImage(kind, id);
        return image is null ? NotFound() : File(image.Value.Bytes, image.Value.ContentType);
    }
}
