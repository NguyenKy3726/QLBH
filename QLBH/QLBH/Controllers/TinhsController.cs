
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class TinhsController : Controller
{
    private readonly AppDbContext _context;

    public TinhsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: TINHS
    public async Task<IActionResult> Index()    
    {
        return View(_context.Tinh_DS());
    }

    // GET: TINHS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tinh = _context.Tinh_CT(id);
        if (tinh == null)
        {
            return NotFound();
        }

        return View(tinh);
    }

    // GET: TINHS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TINHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenTinh")] Tinh tinh)
    {
        ModelState.Remove(nameof(Tinh.MaTinh));
        if (ModelState.IsValid)
        {
            _context.Tinh_Them(tinh);
            return RedirectToAction(nameof(Index));
        }
        return View(tinh);
    }

    // GET: TINHS/Edit/5
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tinh = _context.Tinh_CT(id);
        if (tinh == null)
        {
            return NotFound();
        }

        return View(tinh);
    }

    // POST: TINHS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? matinh, [Bind("MaTinh,TenTinh")] Tinh tinh)
    {
        if (matinh != tinh.MaTinh)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tinh);
                _context.Tinh_Sua(tinh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TinhExists(tinh.MaTinh))
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
        return View(tinh);
    }

    // GET: Tinhs/Delete/01
    public IActionResult Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tinh = _context.Tinh_CT(id);

        if (tinh == null)
        {
            return NotFound();
        }

        return View(tinh);
    }

    // POST: Tinhs/Delete/01
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var tinh = _context.Tinh_CT(id);

        if (tinh == null)
        {
            return NotFound();
        }

        try
        {
            _context.Tinh_Xoa(id);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", tinh);
        }
    }

    private bool TinhExists(string? matinh)
    {
        return _context.Tinhs.Any(e => e.MaTinh == matinh);
    }
}
