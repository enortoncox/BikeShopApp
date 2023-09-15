using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BikeShopApp.Core.Attributes
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _roles;

        public CustomAuthorizeAttribute(string roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedObjectResult(new { detail = "Unauthenticated User! Please Log In", status = 401, title = "Unauthorized" });
            }
            else if (_roles != "")
            {
                string[] roleList = _roles.Split(",");
                bool roleAccepted = false;

                foreach (var role in roleList)
                {
                    if (context.HttpContext.User.IsInRole(role))
                    {
                        roleAccepted = true;
                    }
                }

                if (roleAccepted == false) 
                {
                    context.Result = new UnauthorizedObjectResult(new { detail = $"Forbidden! You are not {_roles}.", status = 403, title = "Forbidden" });
                }
            }
        }
    }
}
