using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FurnitureShop.Helpers
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var role = session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                context.Result = new RedirectToActionResult(
                    "Login", "Account", new { area = "" });
            }
        }
    }
}