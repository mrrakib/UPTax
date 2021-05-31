using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class DashboardController : Controller
    {
        #region Global
        private readonly IAdminNoticeService _adminNoticeService;
        #endregion

        #region constructor
        public DashboardController(IAdminNoticeService adminNoticeService)
        {
            _adminNoticeService = adminNoticeService;
        }
        #endregion

        [HttpGet]
        [RapidAuthorization(All = true)]
        public ActionResult Index()
        {
            RapidSession.Notice = _adminNoticeService.GetAllNoticeByToday(RapidSession.UnionId);

            var dashboardInfo = _adminNoticeService.GetDashboardInfo();
            return View(dashboardInfo);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}