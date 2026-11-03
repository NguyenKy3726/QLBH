
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

[QLBH.Filters.TablePermissionAuthorize("PhongBan")]
public class PhongBansController : Controller
{
    private readonly AppDbContext _context;

    public PhongBansController(AppDbContext context)
    {
        _context = context;
    }

    // GET: PHONGBANS
    public async Task<IActionResult> Index()    
    {
        return View(_context.PhongBan_DS());
    }

    // GET: PHONGBANS/Details/5
    public async Task<IActionResult> Details(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phongban = _context.PhongBan_CT(id.Value);
        if (phongban == null)
        {
            return NotFound();
        }

        return View(phongban);
    }

    // GET: PHONGBANS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PHONGBANS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaPB,TenPB")] PhongBan phongban)
    {
        if (ModelState.IsValid)
        {
            _context.PhongBan_Them(phongban);
            return RedirectToAction(nameof(Index));
        }
        return View(phongban);
    }

    // GET: PHONGBANS/Edit/5
    public async Task<IActionResult> Edit(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phongban = _context.PhongBan_CT(id.Value);
        if (phongban == null)
        {
            return NotFound();
        }

        return View(phongban);
    }

    // POST: PHONGBANS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(byte? mapb, [Bind("MaPB,TenPB")] PhongBan phongban)
    {
        if (mapb != phongban.MaPB)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(phongban);
                _context.PhongBan_Sua(phongban);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhongBanExists(phongban.MaPB))
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
        return View(phongban);
    }

    // GET: PhongBans/Delete/1
    public IActionResult Delete(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phongban = _context.PhongBan_CT(id.Value);

        if (phongban == null)
        {
            return NotFound();
        }

        return View(phongban);
    }

    // POST: PhongBans/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phongban = _context.PhongBan_CT(id.Value);

        if (phongban == null)
        {
            return NotFound();
        }

        try
        {
            _context.PhongBan_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", phongban);
        }
    }

    private bool PhongBanExists(byte? mapb)
    {
        return _context.PhongBans.Any(e => e.MaPB == mapb);
    }
}
