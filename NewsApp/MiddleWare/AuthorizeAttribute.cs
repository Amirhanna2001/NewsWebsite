using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace NewsApp.MiddleWare
{
    public class AuthorizeAttribute:Attribute
    {
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var user = (IdentityUser?)context.HttpContext.Items["User"];
        //    if (user == null)
        //    {
        //        // not logged in
        //        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        //    }
        //}
    }
}
