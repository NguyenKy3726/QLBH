using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;
using QLBH.Data;

public class NhomSPsController : Controller
{
    private readonly AppDbContext _context;

    public NhomSPsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: NHOMSPS
    public async Task<IActionResult> Index()    
    {
        return View(_context.NhomSP_DS());
    }

    // GET: NHOMSPS/Details/5
    public async Task<IActionResult> Details(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhomsp = _context.NhomSP_CT(id.Value);
        if (nhomsp == null)
        {
            return NotFound();
        }

        return View(nhomsp);
    }

    // GET: NHOMSPS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: NHOMSPS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaNSP,TenNSP")] NhomSP nhomsp)
    {
        if (ModelState.IsValid)
        {
            _context.NhomSP_Them(nhomsp);
            return RedirectToAction(nameof(Index));
        }
        return View(nhomsp);
    }

    // GET: NHOMSPS/Edit/5
    public async Task<IActionResult> Edit(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhomsp = _context.NhomSP_CT(id.Value);
        if (nhomsp == null)
        {
            return NotFound();
        }

        return View(nhomsp);
    }

    // POST: NHOMSPS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(byte? mansp, [Bind("MaNSP,TenNSP")] NhomSP nhomsp)
    {
        if (mansp != nhomsp.MaNSP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nhomsp);
                _context.NhomSP_Sua(nhomsp);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhomSPExists(nhomsp.MaNSP))
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
        return View(nhomsp);
    }

    // GET: NHOMSPS/Delete/5
    public async Task<IActionResult> Delete(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhomsp = _context.NhomSP_CT(id.Value);
        if (nhomsp == null)
        {
            return NotFound();
        }

        return View(nhomsp);
    }

    // POST: NHOMSPS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var nhomsp = _context.NhomSP_CT(id.Value);

        if (nhomsp == null)
        {
            return NotFound();
        }

        try
        {
            _context.NhomSP_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", nhomsp);
        }
    }

    private bool NhomSPExists(byte? mansp)
    {
        return _context.NhomSPs.Any(e => e.MaNSP == mansp);
    }
}
