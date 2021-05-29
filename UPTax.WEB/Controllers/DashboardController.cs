using UPTax.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Service.Services;
using UPTax.Helper;

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
            return View();
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