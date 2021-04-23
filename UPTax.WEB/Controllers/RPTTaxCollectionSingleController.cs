using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.UnionDetails;
using UPTax.Model.ViewModels;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;
using UPTax.Utility;

namespace UPTax.Controllers
{
    public class RPTTaxCollectionSingleController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTaxCollectionSingleController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
        {
            _unionParishadService = unionParishadService;
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
        }
        #endregion
        // GET: RPTTaxCollectionList
        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMRPTTaxCollectionSingle> resultSet = new List<VMRPTTaxCollectionSingle>();
            VMRPTTaxCollectionSingle result = _taxInstallmentService.GetMRPTTaxCollectionSingle(commonParams.HoldingNo, commonParams.FinancialYearId, _unionId);
            if (result == null)
            {
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                return View();
            }
            resultSet.Add(result);

            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/Report1.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
            commonParams.FinancialYearName = result != null ? result.FinancialYearName : "N/A";
            commonParams.PaymentDate = result != null ? result.PaymentDate : (DateTime?)null;
            string numberToWord = "";
            if (result != null)
            {
                numberToWord = NumberToWordConverterNew.ConvertToWords(result.GrandTotalAmount, "BDT", "Taka");
            }
            result.GrandAmountStr = numberToWord;
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", resultSet); //get actual data here

            reportViewer.LocalReport.DataSources.Add(A);
            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/RPTTaxCollectionSingle/RPTTaxCollectionSingle.cshtml");

        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FromDate", commonParams.PaymentDate.Value.ToString("dd MMM, yyyy")));
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));

            return paraList;
        }

        #endregion

    }
}