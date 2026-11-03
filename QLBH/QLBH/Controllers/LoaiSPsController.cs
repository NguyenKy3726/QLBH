
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class LoaiSPsController : Controller
{
    private readonly AppDbContext _context;

    public LoaiSPsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: LOAISPS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenNSP = _context.NhomSP_DS()
            .ToDictionary(nsp => nsp.MaNSP, nsp => nsp.TenNSP);
        return View(_context.LoaiSP_DS());
    }

    // GET: LOAISPS/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var loaisp = _context.LoaiSP_CT(id.Value);
        if (loaisp == null)
        {
            return NotFound();
        }
        ViewBag.TenNSP = _context.NhomSP_DS()
            .ToDictionary(nsp => nsp.MaNSP, nsp => nsp.TenNSP);
        return View(loaisp);
    }

    // GET: LOAISPS/Create
    public IActionResult Create()
    {
        ViewBag.MaNSP = new SelectList(_context.NhomSP_DS(), "MaNSP", "TenNSP");
        return View();
    }

    // POST: LOAISPS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaLSP,TenLSP,MaNSP")] LoaiSP loaisp)
    {
        if (ModelState.IsValid)
        {
            _context.LoaiSP_Them(loaisp);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaNSP = new SelectList(_context.NhomSP_DS(), "MaNSP", "TenNSP", loaisp.MaNSP);

        return View(loaisp);
    }

    // GET: LOAISPS/Edit/5
    public async Task<IActionResult> Edit(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var loaisp = _context.LoaiSP_CT(id.Value);
        if (loaisp == null)
        {
            return NotFound();
        }
        ViewBag.MaNSP = new SelectList(_context.NhomSP_DS(), "MaNSP", "TenNSP", loaisp.MaNSP);

        return View(loaisp);
    }

    // POST: LOAISPS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(short? malsp, [Bind("MaLSP,TenLSP,MaNSP")] LoaiSP loaisp)
    {
        if (malsp != loaisp.MaLSP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(loaisp);
                _context.LoaiSP_Sua(loaisp);    
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiSPExists(loaisp.MaLSP))
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
        ViewBag.MaNSP = new SelectList(_context.NhomSP_DS(), "MaNSP", "TenNSP", loaisp.MaNSP);

        return View(loaisp);
    }

    // GET: LoaiSPs/Delete/5
    public IActionResult Delete(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var loaisp = _context.LoaiSP_CT(id.Value);

        if (loaisp == null)
        {
            return NotFound();
        }

        return View(loaisp);
    }

    // POST: LoaiSPs/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var loaisp = _context.LoaiSP_CT(id.Value);

        if (loaisp == null)
        {
            return NotFound();
        }

        try
        {
            _context.LoaiSP_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", loaisp);
        }
    }

    private bool LoaiSPExists(short? malsp)
    {
        return _context.LoaiSPs.Any(e => e.MaLSP == malsp);
    }
}
