using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class CTBHsController : Controller
{
    private readonly AppDbContext _context;

    public CTBHsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: CTBHS
    public async Task<IActionResult> Index()
    {
        ViewBag.TenSP = _context.SanPham_DS()
            .ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(_context.CTBH_DS());
    }

    // GET: CTBHS/Details?madbh=DBH0001&masp=SP0000001
    public async Task<IActionResult> Details(string? madbh, string? masp)
    {
        if (madbh == null || masp == null)
        {
            return NotFound();
        }

        var ctbh = _context.CTBH_CT(madbh, masp);
        if (ctbh == null)
        {
            return NotFound();
        }
        ViewBag.TenSP = _context.SanPham_DS()
            .ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(ctbh);
    }

    // GET: CTBHS/Create
    public IActionResult Create()
    {
        ViewBag.MaDBH = new SelectList(_context.DonBH_DS(), "MaDBH", "MaDBH");
        ViewBag.MaSP = new SelectList(_context.SanPham_DS(), "MaSP", "TenSP");
        return View();
    }

    // POST: CTBHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaDBH,MaSP,SLB,DGB")] CTBH ctbh)
    {
        if (ModelState.IsValid)
        {
            _context.CTBH_Them(ctbh);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaDBH = new SelectList(_context.DonBH_DS(), "MaDBH", "MaDBH", ctbh.MaDBH);
        ViewBag.MaSP = new SelectList(_context.SanPham_DS(), "MaSP", "TenSP", ctbh.MaSP);
        return View(ctbh);
    }

    // GET: CTBHS/Edit?madbh=DBH0001&masp=SP0000001
    public async Task<IActionResult> Edit(string? madbh, string? masp)
    {
        if (madbh == null || masp == null)
        {
            return NotFound();
        }

        var ctbh = _context.CTBH_CT(madbh, masp);
        if (ctbh == null)
        {
            return NotFound();
        }
        ViewBag.MaDBH = new SelectList(_context.DonBH_DS(), "MaDBH", "MaDBH", ctbh.MaDBH);
        ViewBag.MaSP = new SelectList(_context.SanPham_DS(), "MaSP", "TenSP", ctbh.MaSP);
        ViewBag.TenSP = _context.SanPham_DS().ToDictionary(sp => sp.MaSP, sp => sp.TenSP);
        return View(ctbh);
    }

    // POST: CTBHS/Edit?madbh=DBH0001&masp=SP0000001
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? madbh, string? masp, [Bind("MaDBH,MaSP,SLB,DGB")] CTBH ctbh)
    {
        if (madbh != ctbh.MaDBH || masp != ctbh.MaSP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(ctbh);
                _context.CTBH_Sua(ctbh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTBHExists(ctbh.MaDBH, ctbh.MaSP))
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
        ViewBag.MaDBH = new SelectList(_context.DonBH_DS(), "MaDBH", "MaDBH", ctbh.MaDBH);
        ViewBag.MaSP = new SelectList(_context.SanPham_DS(), "MaSP", "TenSP", ctbh.MaSP);
        return View(ctbh);
    }

    // GET: DonBHs/Delete/BH0000001
    public IActionResult Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donbh = _context.DonBH_CT(id);

        if (donbh == null)
        {
            return NotFound();
        }

        return View(donbh);
    }

    // POST: DonBHs/Delete/BH0000001
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donbh = _context.DonBH_CT(id);

        if (donbh == null)
        {
            return NotFound();
        }

        try
        {
            _context.DonBH_Xoa(id);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", donbh);
        }
    }

    private bool CTBHExists(string? madbh, string? masp)
    {
        return _context.CTBHs.Any(e => e.MaDBH == madbh && e.MaSP == masp);
    }
}
