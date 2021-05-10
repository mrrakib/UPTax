using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class DailyPostingReportController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IWardInfoService _wardInfoService;
        private readonly IFinancialYearService _financialYearService;

        public DailyPostingReportController(IWardInfoService wardInfoService, IFinancialYearService financialYearService)
        {
            _wardInfoService = wardInfoService;
            _financialYearService = financialYearService;
        }

        // GET: DailyPostingReport
        public ActionResult Index()
        {
            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");

            return View();
        }
    }
}
