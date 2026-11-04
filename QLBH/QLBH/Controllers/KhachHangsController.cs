
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QLBH.Data;
using QLBH.Models;
using QLBH.Services;

[QLBH.Filters.TablePermissionAuthorize("KhachHang")]
public class KhachHangsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public KhachHangsController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: KHACHHANGS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        return View(_context.KhachHang_DS());
    }

    // GET: KHACHHANGS/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var khachhang = _context.KhachHang_CT(id.Value);
        if (khachhang == null)
        {
            return NotFound();
        }
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        return View(khachhang);
    }

    // GET: KHACHHANGS/Create
    public IActionResult Create()
    {
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP");
        return View();
    }

    // POST: KHACHHANGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Create(
        [Bind("MaKH,HoKH,TenKH,DienThoai,Email,DiaChi,MaXP,TenDN,MatKhau")] KhachHang khachhang,
        IFormFile? fileAnh,
        CancellationToken cancellationToken)
    {
        if (fileAnh is { Length: > 0 })
        {
            var validationError = await ImageUploadService.ValidateAsync(fileAnh, cancellationToken);
            if (validationError is not null)
                ModelState.AddModelError("fileAnh", validationError);
            else if (ModelState.IsValid)
            {
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "customers", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    khachhang.AnhKH = upload.RelativePath;
            }
        }

        if (ModelState.IsValid)
        {
            _context.KhachHang_Them(khachhang);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", khachhang.MaXP);
        return View(khachhang);
    }

    // GET: KHACHHANGS/Edit/5
    public async Task<IActionResult> Edit(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var khachhang = _context.KhachHang_CT(id.Value);
        if (khachhang == null)
        {
            return NotFound();
        }
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", khachhang.MaXP);
        return View(khachhang);
    }

    // POST: KHACHHANGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Edit(
        short? makh,
        [Bind("MaKH,HoKH,TenKH,DienThoai,Email,DiaChi,MaXP,TenDN")] KhachHang khachhang,
        string? matkhaumoi,
        IFormFile? fileAnh,
        CancellationToken cancellationToken)
    {
        if (makh != khachhang.MaKH)
        {
            return NotFound();
        }
        ModelState.Remove(nameof(KhachHang.MatKhau));
        if (matkhaumoi != null && matkhaumoi.Length > 50)
            ModelState.AddModelError("MatKhauMoi", "Mật khẩu không được vượt quá 50 kí tự.");

        var currentCustomer = _context.KhachHang_CT(makh.Value);
        if (currentCustomer is null)
            return NotFound();

        khachhang.AnhKH = currentCustomer.AnhKH;
        if (fileAnh is { Length: > 0 })
        {
            var validationError = await ImageUploadService.ValidateAsync(fileAnh, cancellationToken);
            if (validationError is not null)
                ModelState.AddModelError("fileAnh", validationError);
            else if (ModelState.IsValid)
            {
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "customers", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    khachhang.AnhKH = upload.RelativePath;
            }
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.KhachHang_Sua(khachhang, matkhaumoi);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(khachhang.MaKH))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", khachhang.MaXP);
        return View(khachhang);
    }

    // GET: KhachHangs/Delete/1
    public IActionResult Delete(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var khachhang = _context.KhachHang_CT(id.Value);

        if (khachhang == null)
        {
            return NotFound();
        }

        return View(khachhang);
    }

    // POST: KhachHangs/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var khachhang = _context.KhachHang_CT(id.Value);

        if (khachhang == null)
        {
            return NotFound();
        }

        try
        {
            _context.KhachHang_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", khachhang);
        }
    }

    private bool KhachHangExists(short? makh)
    {
        return _context.KhachHangs.Any(e => e.MaKH == makh);
    }
}
