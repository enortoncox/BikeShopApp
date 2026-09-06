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
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new { detail = "Unauthenticated User! Please Log In", status = 401, title = "Unauthorized" });

                return;
            }

            if (!string.IsNullOrWhiteSpace(_roles))
            {
                var roleList = _roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                var roleAccepted = roleList.Any(role => context.HttpContext.User.IsInRole(role.Trim()));

                if (!roleAccepted) 
                {
                    context.Result = new UnauthorizedObjectResult(new { detail = $"Forbidden! You are not {_roles}.", status = 403, title = "Forbidden" });
                }
            }
        }
    }
}
