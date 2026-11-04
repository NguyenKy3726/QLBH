
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;
using QLBH.Services;

[QLBH.Filters.TablePermissionAuthorize("NhanVien")]
public class NhanViensController : Controller
{
    private readonly AppDbContext _context;

    public NhanViensController(AppDbContext context)
    {
        _context = context;
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
    public async Task<IActionResult> Create([Bind("MaNV,HoNV,TenNV,DienThoai,Email,TenDN,MatKhau,MaPB")] NhanVien nhanvien, IFormFile? fileAnh)
    {
        var upload = await ImageUploadService.ReadAsync(fileAnh);
        if (upload.Error is not null)
            ModelState.AddModelError("fileAnh", upload.Error);

        if (ModelState.IsValid)
        {
            nhanvien.AnhNV = upload.FileName;
            _context.NhanVien_Them(nhanvien, upload.Bytes, upload.ContentType);
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
    public async Task<IActionResult> Edit(byte? manv, [Bind("MaNV,HoNV,TenNV,DienThoai,Email,TenDN,MaPB")] NhanVien nhanvien, string? matkhaumoi, IFormFile? fileAnh, bool removeImage = false)
    {
        if (manv != nhanvien.MaNV) return NotFound();

        var current = _context.NhanVien_CT(manv!.Value);
        if (current is null) return NotFound();

        ModelState.Remove(nameof(NhanVien.MatKhau));
        if (matkhaumoi is { Length: > 50 })
            ModelState.AddModelError("MatKhauMoi", "Mật khẩu không được vượt quá 50 ký tự.");

        var upload = await ImageUploadService.ReadAsync(fileAnh);
        if (upload.Error is not null)
            ModelState.AddModelError("fileAnh", upload.Error);
        if (removeImage && upload.Bytes is not null)
            ModelState.AddModelError("fileAnh", "Hãy chọn thay ảnh hoặc xóa ảnh hiện tại, không thể chọn cả hai.");

        if (ModelState.IsValid)
        {
            nhanvien.AnhNV = removeImage ? null : upload.FileName ?? current.AnhNV;
            try
            {
                _context.NhanVien_Sua(nhanvien, matkhaumoi, upload.Bytes, upload.ContentType, removeImage);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(nhanvien.MaNV)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        nhanvien.AnhNV = current.AnhNV;
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
