
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

[QLBH.Filters.TablePermissionAuthorize("NhaCC")]
public class NhaCCsController : Controller
{
    private readonly AppDbContext _context;

    public NhaCCsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: NHACCS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        return View(_context.NhaCC_DS());
    }

    // GET: NHACCS/Details/5
    public async Task<IActionResult> Details(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhacc = _context.NhaCC_CT(id.Value);
        if (nhacc == null)
        {
            return NotFound();
        }
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        return View(nhacc);
    }

    // GET: NHACCS/Create
    public IActionResult Create()
    {
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP");
        return View();
    }

    // POST: NHACCS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaNCC,TenNCC,DienThoai,Email,DiaChi,MaXP")] NhaCC nhacc)
    {
        if (ModelState.IsValid)
        {
            _context.NhaCC_Them(nhacc);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", nhacc.MaXP);
        return View(nhacc);
    }

    // GET: NHACCS/Edit/5
    public async Task<IActionResult> Edit(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhacc = _context.NhaCC_CT(id.Value);
        if (nhacc == null)
        {
            return NotFound();
        }
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", nhacc.MaXP);
        return View(nhacc);
    }
    // POST: NHACCS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(short? mancc, [Bind("MaNCC,TenNCC,DienThoai,Email,DiaChi,MaXP")] NhaCC nhacc)
    {
        if (mancc != nhacc.MaNCC)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(nhacc);
                _context.NhaCC_Sua(nhacc);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhaCCExists(nhacc.MaNCC))
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
        ViewBag.MaXP = new SelectList(_context.XaPhuong_DS(), "MaXP", "TenXP", nhacc.MaXP);
        return View(nhacc);
    }

    // GET: NhaCCs/Delete/1
    public IActionResult Delete(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhacc = _context.NhaCC_CT(id.Value);

        if (nhacc == null)
        {
            return NotFound();
        }

        return View(nhacc);
    }

    // POST: NhaCCs/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(short? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var nhacc = _context.NhaCC_CT(id.Value);

        if (nhacc == null)
        {
            return NotFound();
        }

        try
        {
            _context.NhaCC_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", nhacc);
        }
    }

    private bool NhaCCExists(short? mancc)
    {
        return _context.NhaCCs.Any(e => e.MaNCC == mancc);
    }
}
