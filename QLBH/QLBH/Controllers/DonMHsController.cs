
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

[QLBH.Filters.TablePermissionAuthorize("DonMH")]
public class DonMHsController : Controller
{
    private readonly AppDbContext _context;

    public DonMHsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DONMHS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenNV = _context.NhanVien_DS()
            .ToDictionary(nv => nv.MaNV, nv =>nv.HoNV +" "+ nv.TenNV);
        ViewBag.TenNCC = _context.NhaCC_DS()
            .ToDictionary(ncc => ncc.MaNCC, ncc => ncc.TenNCC);
        return View(_context.DonMH_DS());
    }

    // GET: DONMHS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donmh = _context.DonMH_CT(id);
        if (donmh == null)
        {
            return NotFound();
        }
        ViewBag.TenNV = _context.NhanVien_DS()
            .ToDictionary(nv => nv.MaNV, nv => nv.HoNV + " "+ nv.TenNV );
        ViewBag.TenNCC = _context.NhaCC_DS()
            .ToDictionary(ncc => ncc.MaNCC, ncc => ncc.TenNCC);
        return View(donmh);
    }

    // GET: DONMHS/Create
    public IActionResult Create()
    {
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi"
        );
        ViewBag.MaNCC = new SelectList(
            _context.NhaCC_DS().Select(ncc => new
            {
                ncc.MaNCC,
                HienThi = $"{ncc.MaNCC} - {ncc.TenNCC}"
            }),
            "MaNCC",
            "HienThi"
        );
        return View();
    }

    // POST: DONMHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NgayDat,NgayGiao,MaNV,MaNCC")] DonMH donmh)
    {
        ModelState.Remove(nameof(DonMH.MaDMH));
        if (ModelState.IsValid)
        {
            _context.DonMH_Them(donmh);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donmh.MaNV
        );
        ViewBag.MaNCC = new SelectList(
            _context.NhaCC_DS().Select(ncc => new
            {
                ncc.MaNCC,
                HienThi = $"{ncc.MaNCC} - {ncc.TenNCC}"
            }),
            "MaNCC",
            "HienThi",
            donmh.MaNCC
        );
        return View(donmh);
    }

    // GET: DONMHS/Edit/5
    public async Task<IActionResult> Edit(string? madmh)
    {
        if (madmh == null)
        {
            return NotFound();
        }

        var donmh = _context.DonMH_CT(madmh);
        if (donmh == null)
        {
            return NotFound();
        }
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donmh.MaNV
        );
        ViewBag.MaNCC = new SelectList(
            _context.NhaCC_DS().Select(ncc => new
            {
                ncc.MaNCC,
                HienThi = $"{ncc.MaNCC} - {ncc.TenNCC}"
            }),
            "MaNCC",
            "HienThi",
            donmh.MaNCC
        );
        return View(donmh);
    }

    // POST: DONMHS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? madmh, [Bind("MaDMH,NgayDat,NgayGiao,MaNV,MaNCC")] DonMH donmh)
    {
        if (madmh != donmh.MaDMH)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(donmh);
                _context.DonMH_Sua(donmh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonMHExists(donmh.MaDMH))
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
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.HoNV} {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donmh.MaNV
        );
        ViewBag.MaNCC = new SelectList(
            _context.NhaCC_DS().Select(ncc => new
            {
                ncc.MaNCC,
                HienThi = $"{ncc.MaNCC} - {ncc.TenNCC}"
            }),
            "MaNCC",
            "HienThi",
            donmh.MaNCC
        );
        return View(donmh);
    }

    // GET: DonMHs/Delete/MH00001
    public IActionResult Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donmh = _context.DonMH_CT(id);

        if (donmh == null)
        {
            return NotFound();
        }

        return View(donmh);
    }

    // POST: DonMHs/Delete/MH00001
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donmh = _context.DonMH_CT(id);

        if (donmh == null)
        {
            return NotFound();
        }

        try
        {
            _context.DonMH_Xoa(id);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", donmh);
        }
    }

    private bool DonMHExists(string? madmh)
    {
        return _context.DonMHs.Any(e => e.MaDMH == madmh);
    }
}
