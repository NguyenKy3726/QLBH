
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class QuyensController : Controller
{
    private readonly AppDbContext _context;

    public QuyensController(AppDbContext context)
    {
        _context = context;
    }

    // GET: QUYENS
    public async Task<IActionResult> Index()    
    {
        return View(_context.Quyen_DS());
    }

    // GET: QUYENS/Details/5
    public async Task<IActionResult> Details(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quyen = _context.Quyen_CT(id.Value);
        if (quyen == null)
        {
            return NotFound();
        }

        return View(quyen);
    }

    // GET: QUYENS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: QUYENS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaQuyen,TenQuyen,MoTa")] Quyen quyen)
    {
        if (ModelState.IsValid)
        {
            _context.Quyen_Them(quyen);
            return RedirectToAction(nameof(Index));
        }
        return View(quyen);
    }

    // GET: QUYENS/Edit/5
    public async Task<IActionResult> Edit(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quyen = _context.Quyen_CT(id.Value);
        if (quyen == null)
        {
            return NotFound();
        }

        return View(quyen);
    }

    // POST: QUYENS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(byte? maquyen, [Bind("MaQuyen,TenQuyen,MoTa")] Quyen quyen)
    {
        if (maquyen != quyen.MaQuyen)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(quyen);
                _context.Quyen_Sua(quyen);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuyenExists(quyen.MaQuyen))
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
        return View(quyen);
    }

    // GET: Quyens/Delete/1
    public IActionResult Delete(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quyen = _context.Quyen_CT(id.Value);

        if (quyen == null)
        {
            return NotFound();
        }

        return View(quyen);
    }

    // POST: Quyens/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var quyen = _context.Quyen_CT(id.Value);

        if (quyen == null)
        {
            return NotFound();
        }

        try
        {
            _context.Quyen_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", quyen);
        }
    }

    private bool QuyenExists(byte? maquyen)
    {
        return _context.Quyens.Any(e => e.MaQuyen == maquyen);
    }
}
