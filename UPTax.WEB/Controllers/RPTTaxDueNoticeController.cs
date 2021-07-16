using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
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
using Humanizer;

namespace UPTax.Controllers
{
    public class RPTTaxDueNoticeController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTaxDueNoticeController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
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
            VMCommonParams model = new VMCommonParams
            {
                LastInstallmentDate = DateTime.Now,
                PrintDate = DateTime.Now
            };
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMRPTTaxCollectionSingle> resultSet = new List<VMRPTTaxCollectionSingle>();
            VMRPTTaxCollectionSingle result = _taxInstallmentService.GetRPTTaxDueNotice(commonParams.HoldingNo, commonParams.FinancialYearId, _unionId);
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
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTTaxDueNotice.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
            
            if (result != null)
            {
                result.HoldingNo = HashingUtility.SwitchEngBan(result.HoldingNo);
                result.PrintDateStr = HashingUtility.SwitchEngBan(commonParams.PrintDate.Value.ToString("dd/MM/yyyy"));
                //numberToWord = NumberToWordConverterNew.ConvertToWords(result.GrandTotalAmount, "BDT", "Taka");
                result.FinancialYearName = HashingUtility.SwitchEngBan(result.FinancialYearName);
                result.CurrentAmountStr = HashingUtility.SwitchEngBan(result.Amount.ToString());

            }
            commonParams.FinancialYearName = result != null ? result.FinancialYearName : "N/A";
            commonParams.PaymentDateStr = result != null ? result.PrintDateStr : "";
            
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams, result));
            ReportDataSource A = new ReportDataSource("DataSet1", resultSet); //get actual data here

            reportViewer.LocalReport.DataSources.Add(A);
            reportViewer.LocalReport.Refresh();
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            reportViewer.ShowToolBar = false;
            string deviceInf = "<DeviceInfo><PageHeight>9.5in</PageHeight><PageWidth>14in</PageWidth></DeviceInfo>";
            byte[] bytes = reportViewer.LocalReport.Render("PDF", deviceInf, out mimeType, out encoding, out extension, out streamIds, out warnings);


            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/RPTTaxDueNotice/RPTTaxDueNotice.cshtml");

        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams, VMRPTTaxCollectionSingle result)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("PrintDate", commonParams.PaymentDateStr));
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));
            paraList.Add(new ReportParameter("Parent", result.ParentName));
            paraList.Add(new ReportParameter("Owner", result.OwnerName));
            paraList.Add(new ReportParameter("Holding", result.HoldingNo));
            paraList.Add(new ReportParameter("Ward", result.WardNo));
            paraList.Add(new ReportParameter("Village", result.BillAddress));
            paraList.Add(new ReportParameter("Amount", result.CurrentAmountStr));

            return paraList;
        }

        #endregion

    }
}