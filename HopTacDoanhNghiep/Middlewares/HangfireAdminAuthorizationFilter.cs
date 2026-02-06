using Hangfire.Dashboard;

namespace HopTacDoanhNghiep.Middlewares
{
    public class HangfireAdminAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (!httpContext.User.Identity?.IsAuthenticated ?? true)
                return false;

            return httpContext.User.IsInRole("Admin");
        }
    }
}
