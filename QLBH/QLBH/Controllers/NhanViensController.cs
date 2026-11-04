
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QLBH.Data;
using QLBH.Models;
using QLBH.Services;

[QLBH.Filters.TablePermissionAuthorize("NhanVien")]
public class NhanViensController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public NhanViensController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    // GET: NHANVIENS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenPB =_context.PhongBan_DS()
            .ToDictionary(pb => pb.MaPB, pb => pb.TenPB);
        return View(_context.NhanVien_DS());
    }

    // GET: NHANVIENS/Details/5
    public async Task<IActionResult> Details(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhanvien = _context.NhanVien_CT(id.Value);
        if (nhanvien == null)
        {
            return NotFound();
        }
        ViewBag.TenPB = _context.PhongBan_DS()
            .ToDictionary(pb => pb.MaPB, pb => pb.TenPB);
        return View(nhanvien);
    }

    // GET: NHANVIENS/Create
    public IActionResult Create()
    {
        ViewBag.MaPB = new SelectList(_context.PhongBan_DS(), "MaPB", "TenPB");
        return View();
    }

    // POST: NHANVIENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Create(
        [Bind("MaNV,HoNV,TenNV,DienThoai,Email,TenDN,MatKhau,MaPB")] NhanVien nhanvien,
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
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "employees", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    nhanvien.AnhNV = upload.RelativePath;
            }
        }

        if (ModelState.IsValid)
        {
            _context.NhanVien_Them(nhanvien);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaPB = new SelectList(_context.PhongBan_DS(), "MaPB", "TenPB", nhanvien.MaPB);
        return View(nhanvien);
    }

    // GET: NHANVIENS/Edit/5
    public async Task<IActionResult> Edit(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhanvien = _context.NhanVien_CT(id.Value);
        if (nhanvien == null)
        {
            return NotFound();
        }
        ViewBag.MaPB = new SelectList(_context.PhongBan_DS(), "MaPB", "TenPB", nhanvien.MaPB);
        return View(nhanvien);
    }

    // POST: NHANVIENS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<IActionResult> Edit(
        byte? manv,
        [Bind("MaNV,HoNV,TenNV,DienThoai,Email,TenDN,MaPB")] NhanVien nhanvien,
        string? matkhaumoi,
        IFormFile? fileAnh,
        CancellationToken cancellationToken)
    {
        if (manv != nhanvien.MaNV)
        {
            return NotFound();
        }

        // Mật khẩu mới được nhận riêng qua matkhaumoi, nên không bắt buộc gửi mật khẩu hiện tại.
        ModelState.Remove(nameof(NhanVien.MatKhau));
        if (matkhaumoi != null && matkhaumoi.Length > 50)
        {
            ModelState.AddModelError("MatKhauMoi", "Mật khẩu không được vượt quá 50 ký tự.");
        }

        var currentEmployee = _context.NhanVien_CT(manv.Value);
        if (currentEmployee is null)
            return NotFound();

        nhanvien.AnhNV = currentEmployee.AnhNV;
        if (fileAnh is { Length: > 0 })
        {
            var validationError = await ImageUploadService.ValidateAsync(fileAnh, cancellationToken);
            if (validationError is not null)
                ModelState.AddModelError("fileAnh", validationError);
            else if (ModelState.IsValid)
            {
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "employees", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    nhanvien.AnhNV = upload.RelativePath;
            }
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nhanvien);
                _context.NhanVien_Sua(nhanvien, matkhaumoi);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(nhanvien.MaNV))
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
        ViewBag.MaPB = new SelectList(_context.PhongBan_DS(), "MaPB", "TenPB", nhanvien.MaPB);
        return View(nhanvien);
    }

    // GET: NhanViens/Delete/1
    public IActionResult Delete(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhanvien = _context.NhanVien_CT(id.Value);

        if (nhanvien == null)
        {
            return NotFound();
        }

        return View(nhanvien);
    }

    // POST: NhanViens/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhanvien = _context.NhanVien_CT(id.Value);

        if (nhanvien == null)
        {
            return NotFound();
        }

        try
        {
            _context.NhanVien_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", nhanvien);
        }
    }

    private bool NhanVienExists(byte? manv)
    {
        return _context.NhanViens.Any(e => e.MaNV == manv);
    }
}
