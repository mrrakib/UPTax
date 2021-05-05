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
    public class RPTTaxReceiptPrintController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTaxReceiptPrintController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService, IWardInfoService wardInfoService, IVillageInfoService villageInfoService)
        {
            _unionParishadService = unionParishadService;
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
        }
        #endregion
        // GET: RPTTaxCollectionList
        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            return View();
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            //ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMRPTTaxReceipt> result = _taxInstallmentService.GetRPTTaxReceipt(commonParams.VillageInfoId, commonParams.WardInfoId, commonParams.FinancialYearId, _unionId);
            if (result.Count == 0)
            {
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);
                ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", commonParams.WardInfoId);
                ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name", commonParams.VillageInfoId);
                return View();
            }

            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/TaxReceipt.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));

            commonParams.FinancialYearName = result.FirstOrDefault().YearName;

            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", result); //get actual data here

            reportViewer.LocalReport.DataSources.Add(A);
            reportViewer.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            //reportViewer.ShowToolBar = false;
            string deviceInf = "<DeviceInfo><PageHeight>14in</PageHeight><PageWidth>9.5in</PageWidth></DeviceInfo>";
            byte[] bytes = reportViewer.LocalReport.Render("PDF", deviceInf, out mimeType, out encoding, out extension, out streamIds, out warnings);


            return File(bytes, "application/pdf");


        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));

            return paraList;
        }

        #endregion


    }
}