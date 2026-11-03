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
            if (string.IsNullOrEmpty(user))
            {
                // build returnUrl (đầy đủ path + query)
                var request = http.Request;
                var returnUrl = request.Path + request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = returnUrl });
                return;
            }
            base.OnActionExecuting(context);
        }
    }

}
