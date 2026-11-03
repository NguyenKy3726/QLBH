using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using QLBH.Data;
using QLBH.Models;

[QLBH.Filters.TablePermissionAuthorize("CTMH")]
public class CTMHsController : Controller
{
    private readonly AppDbContext _context;

    public CTMHsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CTMHS
    public async Task<IActionResult> Index()
    {
        ViewBag.TenSP = _context.SanPham_DS()
            .ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(_context.CTMH_DS());
    }

    // GET: CTMHS/Details?madmh=DMH0001&masp=SP0000001
    public async Task<IActionResult> Details(string? madmh, string? masp)
    {
        if (madmh == null || masp == null)
        {
            return NotFound();
        }

        var ctmh = _context.CTMH_CT(madmh, masp);
        if (ctmh == null)
        {
            return NotFound();
        }
        ViewBag.TenSP = _context.SanPham_DS()
            .ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(ctmh);
    }

    // GET: CTMHS/Create
    public IActionResult Create()
    {
        ViewBag.MaDMH = new SelectList(_context.DonMH_DS(), "MaDMH", "MaDMH");
        ViewBag.MaSP = new SelectList(
            _context.SanPham_DS().Select(sp => new
            {
                sp.MaSP,
                HienThi = $"{sp.MaSP} - {sp.TenSP}"
            }),
            "MaSP",
            "HienThi"
        );
        return View();
    }

    // POST: CTMHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaDMH,MaSP,SLM,DGM")] CTMH ctmh)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.CTMH_Them(ctmh);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException ex) when (ex.Number == 50000)
            {
                ViewBag.LoiTao = ex.Message;
            }
        }
        ViewBag.MaDMH = new SelectList(_context.DonMH_DS(), "MaDMH", "TenDMH", ctmh.MaDMH);
        ViewBag.MaSP = new SelectList(
            _context.SanPham_DS().Select(sp => new
            {
                sp.MaSP,
                HienThi = $"{sp.MaSP} - {sp.TenSP}"
            }),
            "MaSP",
            "HienThi",
            ctmh.MaSP
        );
        return View(ctmh);
    }

    // GET: CTMHS/Edit?madmh=DMH0001&masp=SP0000001
    public async Task<IActionResult> Edit(string? madmh, string? masp)
    {
        if (madmh == null || masp == null)
        {
            return NotFound();
        }

        var ctmh = _context.CTMH_CT(madmh, masp);
        if (ctmh == null)
        {
            return NotFound();
        }
        ViewBag.MaDMH = new SelectList(_context.DonMH_DS(), "MaDMH", "TenDMH", ctmh.MaDMH);
        ViewBag.MaSP = new SelectList(
            _context.SanPham_DS().Select(sp => new
            {
                sp.MaSP,
                HienThi = $"{sp.MaSP} - {sp.TenSP}"
            }),
            "MaSP",
            "HienThi",
            ctmh.MaSP
        );
        ViewBag.TenSP = _context.SanPham_DS()
            .ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(ctmh);
    }

    // POST: CTMHS/Edit?madmh=DMH0001&masp=SP0000001
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? madmh, string? masp, [Bind("MaDMH,MaSP,SLM,DGM")] CTMH ctmh)
    {
        if (madmh != ctmh.MaDMH || masp != ctmh.MaSP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ctmh);
                _context.CTMH_Sua(ctmh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTMHExists(ctmh.MaDMH, ctmh.MaSP))
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
        ViewBag.MaDMH = new SelectList(_context.DonMH_DS(), "MaDMH", "TenDMH", ctmh.MaDMH);
        ViewBag.MaSP = new SelectList(
            _context.SanPham_DS().Select(sp => new
            {
                sp.MaSP,
                HienThi = $"{sp.MaSP} - {sp.TenSP}"
            }),
            "MaSP",
            "HienThi",
            ctmh.MaSP
        );
        return View(ctmh);
    }

    // GET: CTMHs/Delete/MH00001?masp=261015001
    public IActionResult Delete(string? madmh, string? masp)
    {
        if (madmh == null || masp == null)
        {
            return NotFound();
        }

        var ctmh = _context.CTMH_CT(madmh, masp);

        if (ctmh == null)
        {
            return NotFound();
        }

        return View(ctmh);
    }

    // POST: CTMHs/Delete/MH00001?masp=261015001
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id, string? masp)
    {
        if (id == null || masp == null)
        {
            return NotFound();
        }

        var ctmh = _context.CTMH_CT(id, masp);

        if (ctmh == null)
        {
            return NotFound();
        }

        try
        {
            _context.CTMH_Xoa(id, masp);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", ctmh);
        }
    }
    private bool CTMHExists(string? madmh, string? masp)
    {
        return _context.CTMHs.Any(e => e.MaDMH == madmh && e.MaSP == masp);
    }
}
