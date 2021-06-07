using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UPTax.Helper;
using UPTax.Model.Models.UnionDetails;
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
        private readonly IUnionParishadService _unionParishadService;

        public DailyPostingReportController(IWardInfoService wardInfoService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService, IUnionParishadService unionParishadService)
        {
            _wardInfoService = wardInfoService;
            _financialYearService = financialYearService;
            _houseOwnerService = houseOwnerService;
            _unionParishadService = unionParishadService;
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
            if (data.Count == 0)
            {
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardId);
                ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", model.FinancialYearId);
                return View();
            }

            if (model.ReportType == "pdf")
            {
                VMCommonParams commonParams = new VMCommonParams();
                List<VMCommonParams> parList = new List<VMCommonParams>();
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.SizeToReportContent = true;
                reportViewer.Width = Unit.Percentage(100);
                reportViewer.Height = Unit.Percentage(100);
                reportViewer.PageCountMode = new PageCountMode();
                reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTDailyPosting.rdlc");
                //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
                commonParams.FinancialYearName = data.Count > 0 ? data.First().YearName : "N/A";
                commonParams.PaymentDate = model.StartDate;
                commonParams.PrintDate = model.EndDate;
                reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
                ReportDataSource A = new ReportDataSource("DataSet1", data); //get actual data here

                reportViewer.LocalReport.DataSources.Add(A);
                reportViewer.LocalReport.Refresh();

                reportViewer.ShowToolBar = false;

                ViewBag.ReportViewer = reportViewer;
                return View("~/Views/DailyPostingReport/RPTDailyPosting.cshtml");
            }
            model.DailyPostingReports = data;

            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardId);
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", model.FinancialYearId);
            return View(model);
        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);

            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FromDate", commonParams.PaymentDate.Value.ToString("dd MMM, yyyy")));
            paraList.Add(new ReportParameter("ToDate", commonParams.PrintDate.Value.ToString("dd MMM, yyyy")));
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));

            return paraList;
        }

        #endregion
    }
}
