using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
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
    public class RPTTaxByVillageController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IVillageInfoService _villageInfoService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTaxByVillageController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService, IVillageInfoService villageInfoService)
        {
            _unionParishadService = unionParishadService;
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
            _villageInfoService = villageInfoService;
        }
        #endregion
        // GET: RPTTaxCollectionList
        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            return View();
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            //ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMWordOrVillWiseTaxReport> result = _taxInstallmentService.GetRPTTaxInfoByVillage(commonParams.VillageInfoId, commonParams.FinancialYearId, _unionId);
            if (result.Count == 0)
            {
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);
                ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name", commonParams.VillageInfoId);
                return View();
            }

            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.ShowToolBar = false;
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/TaxInfoByVillage.rdlc");


            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", result); //get actual data here

            reportViewer.LocalReport.DataSources.Add(A);
            reportViewer.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            //return File(bytes, "application/pdf");
            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/RPTTaxReceiptPrint/RPTTaxReceipt.cshtml");


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