using book_store.Service;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace book_store.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboard;
        private readonly IReviewService revService;
        public DashboardController(IDashboardService dashboard, IReviewService revService)
        {
            this.dashboard = dashboard;
            this.revService = revService;
        }

        public IActionResult Index()
        {

            if (revService.GetAll().Count > 0)
            {
                var all = revService.SummaryReviews();
                var ret = dashboard.homeAdmin();
                ret.avgRev = all.avgRatings;
                return View(ret);
            }
            else
            {

                return View(dashboard.homeAdmin());
            }

        }
    }
}
