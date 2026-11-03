
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class DonViTinhsController : Controller
{
    private readonly AppDbContext _context;

    public DonViTinhsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DONVITINHS
    public async Task<IActionResult> Index()    
    {
        return View(_context.DonViTinh_DS());
    }

    // GET: DONVITINHS/Details/5
    //GET: DONVITINHS/Details/5 CÁCH THẦY
    //public async Task<IActionResult> Details(DonViTinh dvt)
    //{
    //    return View(_context.DonViTinh_CT(dvt.MaDVT));
    //}
    public async Task<IActionResult> Details(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donvitinh = _context.DonViTinh_CT(id.Value);
        if (donvitinh == null)
        {
            return NotFound();
        }

        return View(donvitinh);
    }

    // GET: DONVITINHS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: DONVITINHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MaDVT,TenDVT")] DonViTinh donvitinh)
    {
        if (ModelState.IsValid)
        {
            _context.DonViTinh_Them(donvitinh);
            return RedirectToAction(nameof(Index));
        }
        return View(donvitinh);
    }

    // GET: DONVITINHS/Edit/5
    public async Task<IActionResult> Edit(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donvitinh = _context.DonViTinh_CT(id.Value);
        if (donvitinh == null)
        {
            return NotFound();
        }
        return View(donvitinh);
    }

    // POST: DONVITINHS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(byte? madvt, [Bind("MaDVT,TenDVT")] DonViTinh donvitinh)
    {
        if (madvt != donvitinh.MaDVT)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(donvitinh);
                _context.DonViTinh_Sua(donvitinh);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonViTinhExists(donvitinh.MaDVT))
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
        return View(donvitinh);
    }

    // GET: DONVITINHS/Delete/5
    public async Task<IActionResult> Delete(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donvitinh = _context.DonViTinh_CT(id.Value);
        if (donvitinh == null)
        {
            return NotFound();
        }

        return View(donvitinh);
    }

    // POST: DONVITINHS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(byte? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var donvitinh = _context.DonViTinh_CT(id.Value);

        if (donvitinh == null)
        {
            return NotFound();
        }
        try
        {
            _context.DonViTinh_Xoa(id.Value);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", donvitinh);
        }
    }

    private bool DonViTinhExists(byte? madvt)
    {
        return _context.DonViTinhs.Any(e => e.MaDVT == madvt);
    }
}
