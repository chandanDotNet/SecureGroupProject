using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualBasic;
using System;
using Microsoft.AspNetCore.Http;

namespace SecureGroup.Controllers
{
    public class ValidateSessionAttribute : ActionFilterAttribute
    {
      
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            int UserId = Convert.ToInt32(context.HttpContext.Session.GetString("UserID"));            
            if (UserId <= 0)
                
            context.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(new { action = "Logout", controller = "Home",Type=100 }));
        }
       
    }
}