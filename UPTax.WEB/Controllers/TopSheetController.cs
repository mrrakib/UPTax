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
    public class TopSheetController : Controller
    {
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IUnionParishadService _unionParishadService;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly Message _message = new Message();

        public TopSheetController(IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService, IUnionParishadService unionParishadService)
        {
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
            _unionParishadService = unionParishadService;
        }

        // GET: TopSheet/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string financialYear = "")
        {
            var data = _taxInstallmentService.GetTopSheetReport(financialYear);
            ViewBag.FinYear = Convert.ToInt32(financialYear);

            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", financialYear);
            return View(data);
        }

        [HttpGet]
        public ActionResult Export(string financialYearId = "")
        {
            var modelPDF = _taxInstallmentService.GetTopSheetReport(financialYearId).ToList();

            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.ShowToolBar = false;
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTTopSheet.rdlc");

            VMCommonParams commonParams = new VMCommonParams
            {
                FinancialYearId = Convert.ToInt32(financialYearId)
            };
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", modelPDF); //get actual data here

            reportViewer.LocalReport.DataSources.Add(A);
            reportViewer.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            //return File(bytes, "application/pdf");
            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/TopSheet/RPTTopsheet.cshtml");
        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            string YearName = _financialYearService.GetNameById(commonParams.FinancialYearId);

            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FinYear", YearName));

            return paraList;
        }

        #endregion
    }
}
