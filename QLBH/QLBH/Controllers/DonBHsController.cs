
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;

[QLBH.Filters.TablePermissionAuthorize("DonBH")]
public class DonBHsController : Controller
{
    private readonly AppDbContext _context;

    public DonBHsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: DONBHS
    public async Task<IActionResult> Index()    
    {
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        ViewBag.TenNV = _context.NhanVien_DS()
            .ToDictionary(nv => nv.MaNV, nv => nv.TenNV);
        ViewBag.TenKH = _context.KhachHang_DS()
            .ToDictionary(kh => kh.MaKH, kh => kh.TenKH);
        return View(_context.DonBH_DS());
    }

    // GET: DONBHS/Details/5
    public async Task<IActionResult> Details(string? madbh)
    {
        if (madbh == null)
        {
            return NotFound();
        }

        var donbh = _context.DonBH_CT(madbh);
        if (donbh == null)
        {
            return NotFound();
        }
        ViewBag.TenXP = _context.XaPhuong_DS()
            .ToDictionary(xp => xp.MaXP, xp => xp.TenXP);
        ViewBag.TenNV = _context.NhanVien_DS()
            .ToDictionary(nv => nv.MaNV, nv => nv.TenNV);
        ViewBag.TenKH = _context.KhachHang_DS()
            .ToDictionary(kh => kh.MaKH, kh => kh.TenKH);
        ViewBag.LichSu = _context.DonBH_LichSu(madbh);
        return View(donbh);
    }

    // GET: DONBHS/Create
    public IActionResult Create()
    {
        ViewBag.MaXP = new SelectList(
            _context.XaPhuong_DS().Select(xp => new
            {
                xp.MaXP,
                HienThi = $"{xp.MaXP} - {xp.TenXP}"
            }),
            "MaXP",
            "HienThi"
        );
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.TenNV}"
            }),
            "MaNV",
            "HienThi"
        );
        ViewBag.MaKH = new SelectList(
            _context.KhachHang_DS().Select(kh => new
            {
                kh.MaKH,
                HienThi = $"{kh.MaKH} - {kh.TenKH}"
            }),
            "MaKH",
            "HienThi"
        );
        return View();
    }

    // POST: DONBHS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("NgayDat,NgayGiao,DCGH,MaXP,MaNV,MaKH")] DonBH donbh)
    {
        ModelState.Remove(nameof(DonBH.MaDBH));
        if (ModelState.IsValid)
        {
            _context.DonBH_Them(donbh);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.MaXP = new SelectList(
            _context.XaPhuong_DS().Select(xp => new
            {
                xp.MaXP,
                HienThi = $"{xp.MaXP} - {xp.MaXP}"
            }),
            "MaXP",
            "HienThi",
            donbh.MaXP
        );
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donbh.MaNV
        );
        ViewBag.MaKH = new SelectList(
            _context.KhachHang_DS().Select(kh => new
            {
                kh.MaKH,
                HienThi = $"{kh.MaKH} - {kh.TenKH}"
            }),
            "MaKH",
            "HienThi",
            donbh.MaKH
        );
        return View(donbh);
    }

    // GET: DONBHS/Edit/5
    public async Task<IActionResult> Edit(string? madbh)
    {
        if (madbh == null)
        {
            return NotFound();
        }

        var donbh = _context.DonBH_CT(madbh);
        if (donbh == null)
        {
            return NotFound();
        }
        if (donbh.TrangThai != "CHO_XAC_NHAN")
        {
            TempData["LoiDonBH"] = "Chỉ được sửa thông tin khi đơn còn chờ xác nhận.";
            return RedirectToAction(nameof(Details), new { madbh });
        }
        ViewBag.MaXP = new SelectList(
            _context.XaPhuong_DS().Select(xp => new
            {
                xp.MaXP,
                HienThi = $"{xp.MaXP} - {xp.TenXP}"
            }),
            "MaXP",
            "HienThi",
            donbh.MaXP
        );
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donbh.MaNV
        );
        ViewBag.MaKH = new SelectList(
            _context.KhachHang_DS().Select(kh => new
            {
                kh.MaKH,
                HienThi = $"{kh.MaKH} - {kh.TenKH}"
            }),
            "MaKH",
            "HienThi",
            donbh.MaKH
        );
        return View(donbh);
    }

    // POST: DONBHS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string? madbh, [Bind("MaDBH,NgayDat,NgayGiao,DCGH,MaXP,MaNV,MaKH")] DonBH donbh)
    {
        if (madbh != donbh.MaDBH)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(donbh);
                _context.DonBH_Sua(donbh);
            }
            catch (SqlException ex)
            {
                TempData["LoiDonBH"] = ex.Message;
                return RedirectToAction(nameof(Details), new { madbh });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonBHExists(donbh.MaDBH))
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
        ViewBag.MaXP = new SelectList(
            _context.XaPhuong_DS().Select(xp => new
            {
                xp.MaXP,
                HienThi = $"{xp.MaXP} - {xp.MaXP}"
            }),
            "MaXP",
            "HienThi",
            donbh.MaXP
        );
        ViewBag.MaNV = new SelectList(
            _context.NhanVien_DS().Select(nv => new
            {
                nv.MaNV,
                HienThi = $"{nv.MaNV} - {nv.TenNV}"
            }),
            "MaNV",
            "HienThi",
            donbh.MaNV
        );
        ViewBag.MaKH = new SelectList(
            _context.KhachHang_DS().Select(kh => new
            {
                kh.MaKH,
                HienThi = $"{kh.MaKH} - {kh.TenKH}"
            }),
            "MaKH",
            "HienThi",
            donbh.MaKH
        );
        return View(donbh);
    }

    // Hủy đơn bằng chuyển trạng thái để lưu lý do và lịch sử; không xóa vật lý.
    public IActionResult Delete(string? madbh)
    {
        if (string.IsNullOrWhiteSpace(madbh))
            return NotFound();

        TempData["LoiDonBH"] = "Đơn bán hàng không được xóa. Hãy hủy đơn và nhập lý do.";
        return RedirectToAction(nameof(Details), new { madbh });
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string? madbh)
    {
        if (string.IsNullOrWhiteSpace(madbh))
            return NotFound();

        TempData["LoiDonBH"] = "Đơn bán hàng không được xóa. Hãy hủy đơn và nhập lý do.";
        return RedirectToAction(nameof(Details), new { madbh });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChuyenTrangThai(string? madbh, string? trangThaiMoi, string? ghiChu)
    {
        if (string.IsNullOrWhiteSpace(madbh) || string.IsNullOrWhiteSpace(trangThaiMoi))
            return BadRequest();

        var maNV = HttpContext.Session.GetInt32("MaNV");
        if (maNV is null || maNV < byte.MinValue || maNV > byte.MaxValue)
            return Forbid();

        try
        {
            _context.DonBH_ChuyenTrangThai(
                madbh, trangThaiMoi, (byte)maNV.Value, ghiChu);
            TempData["ThongBaoDonBH"] = "Đã cập nhật trạng thái đơn hàng.";
        }
        catch (SqlException ex)
        {
            TempData["LoiDonBH"] = ex.Message;
        }

        return RedirectToAction(nameof(Details), new { madbh });
    }

    private bool DonBHExists(string? madbh)
    {
        return _context.DonBHs.Any(e => e.MaDBH == madbh);
    }
}
