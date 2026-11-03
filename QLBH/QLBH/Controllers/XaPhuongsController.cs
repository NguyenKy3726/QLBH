
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class XaPhuongsController : Controller
{
    private readonly AppDbContext _context;

    public XaPhuongsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: XAPHUONGS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenTinh = _context.Tinh_DS()
            .ToDictionary(t => t.MaTinh, t => t.TenTinh);
        return View(_context.XaPhuong_DS());
    }

    // GET: XAPHUONGS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var xaphuong = _context.XaPhuong_CT(id);
        if (xaphuong == null)
        {
            return NotFound();
        }
        ViewBag.TenTinh = _context.Tinh_DS()
            .ToDictionary(t => t.MaTinh, t => t.TenTinh);
        return View(xaphuong);
    }

    // GET: XAPHUONGS/Create
    public IActionResult Create()
    {
        ViewBag.MaTinh = new SelectList(_context.Tinh_DS(), "MaTinh", "TenTinh");
        return View();
    }

    // POST: XAPHUONGS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenXP,MaTinh")] XaPhuong xaphuong)
    {
        ModelState.Remove(nameof(XaPhuong.MaXP));
        if (ModelState.IsValid)
        {
            _context.XaPhuong_Them(xaphuong);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaTinh = new SelectList(_context.Tinh_DS(), "MaTinh", "TenTinh", xaphuong.MaTinh);
        return View(xaphuong);
    }

    // GET: XAPHUONGS/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var xaphuong = _context.XaPhuong_CT(id);
        if (xaphuong == null)
        {
            return NotFound();
        }
        ViewBag.MaTinh = new SelectList(_context.Tinh_DS(), "MaTinh", "TenTinh", xaphuong.MaTinh);
        return View(xaphuong);
    }

    // POST: XAPHUONGS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? maxp, [Bind("MaXP,TenXP,MaTinh")] XaPhuong xaphuong)
    {
        if (maxp != xaphuong.MaXP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(xaphuong);
                _context.XaPhuong_Sua(xaphuong);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XaPhuongExists(xaphuong.MaXP))
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
        ViewBag.MaTinh = new SelectList(_context.Tinh_DS(), "MaTinh", "TenTinh", xaphuong.MaTinh);
        return View(xaphuong);
    }

    // GET: XaPhuongs/Delete/01001
    public IActionResult Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var xaphuong = _context.XaPhuong_CT(id);

        if (xaphuong == null)
        {
            return NotFound();
        }

        return View(xaphuong);
    }

    // POST: XaPhuongs/Delete/01001
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var xaphuong = _context.XaPhuong_CT(id);

        if (xaphuong == null)
        {
            return NotFound();
        }

        try
        {
            _context.XaPhuong_Xoa(id);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", xaphuong);
        }
    }

    private bool XaPhuongExists(string? maxp)
    {
        return _context.XaPhuongs.Any(e => e.MaXP == maxp);
    }
}
