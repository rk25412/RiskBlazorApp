using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clear.Risk.Controllers
{
    public class AuthFilterForHangfireDashboard : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var claim = ((ClaimsIdentity)httpContext.User.Identity).FindFirst(ClaimTypes.Role);
                string role = claim.Value;

                return role == "System Administrator";
            }

            return false;
        }
    }
}
