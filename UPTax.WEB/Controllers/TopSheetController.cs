using Microsoft.Reporting.WebForms;
using System.IO;
using System.Web.Mvc;
using UPTax.Model;
using UPTax.Service.Services;
using UPTax.WEB;

namespace UPTax.Controllers
{
    public class TopSheetController : Controller
    {
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;

        public TopSheetController(IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
        {
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
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

            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", financialYear);
            return View(data);
        }

        [HttpGet]
        public ActionResult Export(string financialYearId = "")
        {
            var modelPDF = _taxInstallmentService.GetTopSheetReport(financialYearId);
            var results = new ReportProperty<dynamic>
            {
                ReportTitle = "Top Sheet Report",
                ReportViewName = "Top Sheet Report",
                ReportPath = Path.Combine(HttpContext.Server.MapPath("~/Reports/RDLC/"), "TopSheetReport.rdlc"),
                ReportBody = modelPDF.ToDataTable()
            };
            var reportViewer = new LocalReport
            {
                EnableExternalImages = true,
                ReportPath = results.ReportPath,
            };

            var header = new ReportHeader
            {
                Name = "Top Sheet Report",
                Address = "Natore, Bangladesh",
                //Logo = new Uri(HttpContext.Server.MapPath("~/Image/logo.png")).AbsoluteUri
            };
            var rptHead = new ReportDataSource("ReportHeader", header.ToDataTable());
            reportViewer.DataSources.Add(rptHead);

            var rptDs = new ReportDataSource("TopSheetReportBody", results.ReportBody);
            reportViewer.DataSources.Add(rptDs);

            string mimeType;
            var renderedBytes = ReportUtility.RenderedReportViewer(reportViewer, "PDF", out mimeType);
            return File(renderedBytes, mimeType);
        }
    }
}
