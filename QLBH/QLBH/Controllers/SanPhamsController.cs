
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QLBH.Data;
using QLBH.Models;
using QLBH.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class SanPhamsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public SanPhamsController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("/uploads/{folder}/{fileName}")]
    public IActionResult UploadedImage(string folder, string fileName)
    {
        if (folder is not ("products" or "employees" or "customers") ||
            string.IsNullOrWhiteSpace(fileName) || fileName != Path.GetFileName(fileName))
            return NotFound();

        var contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            _ => null
        };
        if (contentType is null)
            return NotFound();

        var webRoot = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRoot))
            webRoot = Path.Combine(_environment.ContentRootPath, "wwwroot");

        var path = Path.Combine(webRoot, "uploads", folder, fileName);
        return System.IO.File.Exists(path) ? PhysicalFile(path, contentType) : NotFound();
    }

    // GET: SANPHAMS
    public IActionResult Index(string? category, string? budget, string? q, string? sort, int page = 1)
    {
        if (EmployeeCatalogAccessDenied())
            return RedirectToAction("AccessDenied", "Account");

        var isEmployee = HttpContext.Session.GetString("Role") == "NhanVien";
        var employeeId = HttpContext.Session.GetInt32("MaNV");
        bool CanManage(string operation) => isEmployee && employeeId is >= byte.MinValue and <= byte.MaxValue
            && _context.NhanVien_CoQuyenBang((byte)employeeId.Value, "SanPham", operation);
        ViewBag.CanCreate = CanManage("C");
        ViewBag.CanEdit = CanManage("U");
        ViewBag.CanDelete = CanManage("D");

        var productTypes = _context.LoaiSP_DS();
        ViewBag.TenLSP = productTypes
            .ToDictionary(lsp => lsp.MaLSP, lsp => lsp.TenLSP);
        ViewBag.TenDVT = _context.DonViTinh_DS()
            .ToDictionary(dvt => dvt.MaDVT, dvt => dvt.TenDVT);

        var products = _context.SanPham_DS();
        if (!string.IsNullOrWhiteSpace(category))
        {
            var selectedType = productTypes.FirstOrDefault(item =>
                string.Equals(item.TenLSP, category, StringComparison.OrdinalIgnoreCase));
            if (selectedType != null)
                products = products.Where(item => item.MaLSP == selectedType.MaLSP).ToList();
        }

        if (!string.IsNullOrWhiteSpace(budget))
        {
            var priceRange = budget.Split('-', 2);
            var minimum = 0m;
            var maximum = 0m;
            var hasMinimum = decimal.TryParse(priceRange[0], System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out minimum);
            var hasMaximum = priceRange.Length > 1 && decimal.TryParse(priceRange[1], System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture, out maximum);
            products = products.Where(item => (!hasMinimum || item.DonGia >= minimum)
                && (!hasMaximum || item.DonGia <= maximum)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            products = products.Where(item =>
                (item.TenSP?.Contains(q, StringComparison.CurrentCultureIgnoreCase) ?? false)
                || (item.MoTaCT?.Contains(q, StringComparison.CurrentCultureIgnoreCase) ?? false)).ToList();
        }

        // Lọc và sắp xếp toàn bộ kết quả trước khi lấy 10 sản phẩm của trang.
        var orderedProducts = sort switch
        {
            "price-asc" => products.OrderBy(item => item.DonGia).ThenBy(item => item.MaSP),
            "price-desc" => products.OrderByDescending(item => item.DonGia).ThenBy(item => item.MaSP),
            "name-asc" => products.OrderBy(item => item.TenSP).ThenBy(item => item.MaSP),
            _ => products.OrderBy(item => item.MaSP)
        };
        const int pageSize = 10;
        var totalProducts = products.Count;
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalProducts / (double)pageSize));
        page = Math.Clamp(page, 1, totalPages);
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalProducts = totalProducts;
        ViewBag.PageSize = pageSize;

        return View(orderedProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList());
    }

    // GET: SANPHAMS/Details/5
    public IActionResult Details(string? id)
    {
        if (EmployeeCatalogAccessDenied())
            return RedirectToAction("AccessDenied", "Account");

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]

    public async Task<IActionResult> Create(
        [Bind("TenSP,DonGia,MoTaCT,MaLSP,MaDVT")] SanPham sanpham,
        IFormFile? fileAnh,
        CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(SanPham.MaSP));
        if (fileAnh is { Length: > 0 })
        {
            var validationError = await ImageUploadService.ValidateAsync(fileAnh, cancellationToken);
            if (validationError is not null)
                ModelState.AddModelError("fileAnh", validationError);
            else if (ModelState.IsValid)
            {
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "products", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    sanpham.AnhSP = upload.RelativePath;
            }
        }

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]

    public async Task<IActionResult> Edit(
        string? masp,
        [Bind("MaSP,TenSP,DonGia,MoTaCT,MaLSP,MaDVT")] SanPham sanpham,
        IFormFile? fileAnh,
        CancellationToken cancellationToken)
    {
        if (masp != sanpham.MaSP)
        {
            return NotFound();
        }

        var currentProduct = _context.SanPham_CT(masp);
        if (currentProduct is null)
            return NotFound();

        sanpham.AnhSP = currentProduct.AnhSP;
        if (fileAnh is { Length: > 0 })
        {
            var validationError = await ImageUploadService.ValidateAsync(fileAnh, cancellationToken);
            if (validationError is not null)
                ModelState.AddModelError("fileAnh", validationError);
            else if (ModelState.IsValid)
            {
                var upload = await ImageUploadService.SaveAsync(fileAnh, _environment, "products", cancellationToken);
                if (upload.Error is not null)
                    ModelState.AddModelError("fileAnh", upload.Error);
                else
                    sanpham.AnhSP = upload.RelativePath;
            }
        }

        if (ModelState.IsValid)
        {
            try
            {

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]

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
    [QLBH.Filters.TablePermissionAuthorize("SanPham")]
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

    private bool EmployeeCatalogAccessDenied()
    {
        if (HttpContext.Session.GetString("Role") != "NhanVien")
            return false;

        var maNV = HttpContext.Session.GetInt32("MaNV");
        return maNV is null || maNV < byte.MinValue || maNV > byte.MaxValue
            || !_context.NhanVien_CoQuyenBang((byte)maNV.Value, "SanPham", "R");
    }
}
