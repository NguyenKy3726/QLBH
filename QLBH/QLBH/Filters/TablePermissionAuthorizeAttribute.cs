using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QLBH.Data;

namespace QLBH.Filters;

/// <summary>
/// Gắn quyền theo bảng vào controller. Các action CRUD chuẩn được ánh xạ
/// tự động sang C/R/U/D; action chưa khai báo sẽ bị từ chối theo nguyên tắc fail-closed.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class TablePermissionAuthorizeAttribute : TypeFilterAttribute
{
    public TablePermissionAuthorizeAttribute(string tenBang)
        : base(typeof(TablePermissionAuthorizeFilter))
    {
        Arguments = new object[] { tenBang };
    }
}

public sealed class TablePermissionAuthorizeFilter : IAsyncActionFilter
{
    private readonly AppDbContext _db;
    private readonly string _tenBang;

    public TablePermissionAuthorizeFilter(AppDbContext db, string tenBang)
    {
        _db = db;
        _tenBang = tenBang;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var http = context.HttpContext;
        var username = http.Session.GetString("Username");
        var maNV = http.Session.GetInt32("MaNV");

        if (string.IsNullOrWhiteSpace(username) || maNV is null
            || maNV < byte.MinValue || maNV > byte.MaxValue)
        {
            var returnUrl = http.Request.Path + http.Request.QueryString;
            context.Result = new RedirectToActionResult(
                "Login", "Account", new { returnUrl });
            return;
        }

        var actionName = context.ActionDescriptor.RouteValues.TryGetValue("action", out var routeAction)
            ? routeAction
            : null;
        var thaoTac = MapActionToOperation(actionName);
        if (thaoTac is null
            || !_db.NhanVien_CoQuyenBang((byte)maNV.Value, _tenBang, thaoTac))
        {
            context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            return;
        }

        await next();
    }

    private static string? MapActionToOperation(string? actionName) =>
        actionName?.ToLowerInvariant() switch
        {
            "index" or "details" => "R",
            "create" => "C",
            "edit" => "U",
            "delete" => "D",
            _ => null
        };
}
