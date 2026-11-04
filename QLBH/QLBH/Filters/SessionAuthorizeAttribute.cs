namespace QLBH.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var http = context.HttpContext;
            var user = http.Session.GetString("Username");
            var role = http.Session.GetString("Role");
            var maNV = http.Session.GetInt32("MaNV");

            if (string.IsNullOrEmpty(user) || role != "NhanVien" || maNV is null)
            {
                var request = http.Request;
                var returnUrl = request.Path + request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
