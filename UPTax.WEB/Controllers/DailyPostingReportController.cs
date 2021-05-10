using System;
using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Model.ViewModels;
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
        private readonly IHouseOwnerService _houseOwnerService;

        public DailyPostingReportController(IWardInfoService wardInfoService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService)
        {
            _wardInfoService = wardInfoService;
            _financialYearService = financialYearService;
            _houseOwnerService = houseOwnerService;
        }

        // GET: DailyPostingReport
        public ActionResult Index()
        {
            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            var model = new VMDailyPostingReport() { StartDate = DateTime.Now, EndDate = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VMDailyPostingReport model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardId);
                ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", model.FinancialYearId);
                return View(model);
            }
            var data = _houseOwnerService.GetDailyPostingReport(model);

            if (model.ReportType == "pdf")
            {
                //Write Report Code Here...
            }
            model.DailyPostingReports = data;

            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardId);
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", model.FinancialYearId);
            return View(model);
        }
    }
}
