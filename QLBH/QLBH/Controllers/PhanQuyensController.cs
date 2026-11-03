using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;
using QLBH.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

public class PhanQuyensController : Controller
{
    private readonly AppDbContext _context;

    public PhanQuyensController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PHANQUYENS
    public async Task<IActionResult> Index()
    {
        ViewBag.TenQuyen = _context.Quyen_DS()
            .ToDictionary(q => q.MaQuyen, q => q.TenQuyen);

        ViewBag.TenNV = _context.NhanVien_DS()
            .ToDictionary(n => n.MaNV, n => n.HoNV + " " + n.TenNV);
        return View(_context.PhanQuyen_DS());
    }

    // GET: PHANQUYENS/Details?maquyen=1&manv=2
    public async Task<IActionResult> Details(byte? maquyen, byte? manv)
    {
        if (maquyen == null || manv == null)
        {
            return NotFound();
        }

        var phanQuyen = _context.PhanQuyen_CT(maquyen.Value, manv.Value);
        if (phanQuyen == null)
        {
            return NotFound();
        }

        return View(phanQuyen);
    }

    // GET: PHANQUYENS/Create
    public IActionResult Create()
    {
        ViewBag.MaQuyen = new SelectList(_context.Quyen_DS(), "MaQuyen", "TenQuyen");
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi"
        );
        return View();
    }

    // POST: PHANQUYENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaQuyen,MaNV")] PhanQuyen phanQuyen)
    {
        if (ModelState.IsValid)
        {
            _context.PhanQuyen_Them(phanQuyen);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaQuyen = new SelectList(_context.Quyen_DS(), "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            phanQuyen.MaNV
        );
        return View(phanQuyen);
    }

    // GET: PHANQUYENS/Edit?maquyen=1&manv=2
    public async Task<IActionResult> Edit(byte? maquyen, byte? manv)
    {
        if (maquyen == null || manv == null)
        {
            return NotFound();
        }

        var phanQuyen = _context.PhanQuyen_CT(maquyen.Value, manv.Value);
        if (phanQuyen == null)
        {
            return NotFound();
        }
        ViewBag.MaQuyen = new SelectList(_context.Quyen_DS(), "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            phanQuyen.MaNV
        );
        return View(phanQuyen);
    }

    // POST: PHANQUYENS/Edit?maquyen=1&manv=2
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(byte? maquyen, byte? manv, [Bind("MaQuyen,MaNV")] PhanQuyen phanQuyen)
    {
        if (maquyen != phanQuyen.MaQuyen || manv != phanQuyen.MaNV)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phanQuyen);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhanQuyenExists(phanQuyen.MaQuyen, phanQuyen.MaNV))
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
        ViewBag.MaQuyen = new SelectList(_context.Quyen_DS(), "MaQuyen", "TenQuyen", phanQuyen.MaQuyen);
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            phanQuyen.MaNV
        );
        return View(phanQuyen);
    }

    // GET: PHANQUYENS/Delete?maquyen=1&manv=2
    public async Task<IActionResult> Delete(byte? maquyen, byte? manv)
    {
        if (maquyen == null || manv == null)
        {
            return NotFound();
        }

        var phanQuyen = _context.PhanQuyen_CT(maquyen.Value, manv.Value);
        if (phanQuyen == null)
        {
            return NotFound();
        }

        return View(phanQuyen);
    }

    // POST: PHANQUYENS/Delete?maquyen=1&manv=2
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(byte? maquyen, byte? manv)
    {
        if (maquyen == null || manv == null)
        {
            return NotFound();
        }
        var phanQuyen = _context.PhanQuyen_CT(maquyen.Value, manv.Value);
        if (phanQuyen == null)
        {
            return NotFound();
        }

        _context.PhanQuyen_Xoa(maquyen.Value, manv.Value);

        return RedirectToAction(nameof(Index));
    }

    private bool PhanQuyenExists(byte? maquyen, byte? manv)
    {
        return _context.PhanQuyens.Any(e => e.MaQuyen == maquyen && e.MaNV == manv);
    }
}
