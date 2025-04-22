using Hangfire.Dashboard;

namespace Dr_Home.BackgroundJobs
{
    public class AllowAllDashboardAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true; 
        }
    }

}
