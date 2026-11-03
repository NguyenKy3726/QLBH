
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

public class SanPhamsController : Controller
{
    private readonly AppDbContext _context;

    public SanPhamsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: SANPHAMS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenLSP = _context.LoaiSP_DS()
            .ToDictionary(lsp => lsp.MaLSP, lsp => lsp.TenLSP);
        ViewBag.TenDVT = _context.DonViTinh_DS()
            .ToDictionary(dvt => dvt.MaDVT, dvt => dvt.TenDVT);
        return View(_context.SanPham_DS());
    }

    // GET: SANPHAMS/Details/5
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sanpham = _context.SanPham_CT(id);
        if (sanpham == null)
        {
            return NotFound();
        }
        ViewBag.TenLSP = _context.LoaiSP_DS()
            .ToDictionary(lsp => lsp.MaLSP, lsp => lsp.TenLSP);
        ViewBag.TenDVT = _context.DonViTinh_DS()
            .ToDictionary(dvt => dvt.MaDVT, dvt => dvt.TenDVT);
        return View(sanpham);
    }

    // GET: SANPHAMS/Create
    public IActionResult Create()
    {
        ViewBag.MaLSP = new SelectList(_context.LoaiSP_DS(), "MaLSP", "TenLSP");
        ViewBag.MaDVT = new SelectList(_context.DonViTinh_DS(), "MaDVT", "TenDVT");
        return View();
    }

    // POST: SANPHAMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenSP,DonGia,AnhSP,MoTaCT,MaLSP,MaDVT")] SanPham sanpham)
    {
        ModelState.Remove(nameof(SanPham.MaSP));
        if (ModelState.IsValid)
        {
            _context.SanPham_Them(sanpham);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaLSP = new SelectList(_context.LoaiSP_DS(), "MaLSP", "TenLSP", sanpham.MaLSP);
        ViewBag.MaDVT = new SelectList(_context.DonViTinh_DS(), "MaDVT", "TenDVT", sanpham.MaDVT);
        return View(sanpham);
    }

    // GET: SANPHAMS/Edit/5
    public async Task<IActionResult> Edit(string? masp)
    {
        if (masp == null)
        {
            return NotFound();
        }

        var sanpham = _context.SanPham_CT(masp);
        if (sanpham == null)
        {
            return NotFound();
        }
        ViewBag.MaLSP = new SelectList(_context.LoaiSP_DS(), "MaLSP", "TenLSP", sanpham.MaLSP);
        ViewBag.MaDVT = new SelectList(_context.DonViTinh_DS(), "MaDVT", "TenDVT", sanpham.MaDVT);
        return View(sanpham);
    }

    // POST: SANPHAMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? masp, [Bind("MaSP, TenSP,DonGia,AnhSP,MoTaCT,MaLSP,MaDVT")] SanPham sanpham)
    {
        if (masp != sanpham.MaSP)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(sanpham);
                _context.SanPham_Sua(sanpham);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanpham.MaSP))
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
        ViewBag.MaLSP = new SelectList(_context.LoaiSP_DS(), "MaLSP", "TenLSP", sanpham.MaLSP);
        ViewBag.MaDVT = new SelectList(_context.DonViTinh_DS(), "MaDVT", "TenDVT", sanpham.MaDVT);
        return View(sanpham);
    }

    // GET: SanPhams/Delete/261015001
    public IActionResult Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sanpham = _context.SanPham_CT(id);

        if (sanpham == null)
        {
            return NotFound();
        }

        return View(sanpham);
    }

    // POST: SanPhams/Delete/261015001
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var sanpham = _context.SanPham_CT(id);

        if (sanpham == null)
        {
            return NotFound();
        }

        try
        {
            _context.SanPham_Xoa(id);

            return RedirectToAction(nameof(Index));
        }
        catch (SqlException ex) when (ex.Number == 50000)
        {
            ViewBag.LoiXoa = ex.Message;

            return View("Delete", sanpham);
        }
    }

    private bool SanPhamExists(string? masp)
    {
        return _context.SanPhams.Any(e => e.MaSP == masp);
    }
}
