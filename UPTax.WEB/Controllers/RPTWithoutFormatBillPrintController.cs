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
    public class RPTWithoutFormatBillPrintController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTWithoutFormatBillPrintController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
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
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTWithoutFormatBillPrint.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
            
            string numberToWord = "";
            if (result != null)
            {
                int grandTotal = (int)result.GrandTotalAmount;
                CultureInfo culture = new CultureInfo("bn-BD");
                numberToWord = grandTotal.ToWords(culture);
                numberToWord += " টাকা মাত্র।";
                result.HoldingNo = HashingUtility.SwitchEngBan(result.HoldingNo);
                result.MobileNo = HashingUtility.SwitchEngBan(result.MobileNo);
                result.LastPaymentDateStr = HashingUtility.SwitchEngBan(commonParams.LastInstallmentDate.Value.ToString("dd/MM/yyyy"));
                //numberToWord = NumberToWordConverterNew.ConvertToWords(result.GrandTotalAmount, "BDT", "Taka");
                result.FinancialYearName = HashingUtility.SwitchEngBan(result.FinancialYearName);
                result.CurrentAmountStr = HashingUtility.SwitchEngBan(result.CurrentAmount.ToString());
                result.CurrentDueAmountStr = HashingUtility.SwitchEngBan(result.CurrentDueAmount.ToString());
                result.PrevDueAmountStr = HashingUtility.SwitchEngBan(result.PrevDueAmount.ToString());
                result.PrevAmountStr = HashingUtility.SwitchEngBan(result.PrevAmount.ToString());
                result.PenaltyAmountStr = HashingUtility.SwitchEngBan(result.PenaltyAmount.ToString());
                result.PenaltyDueAmountStr = HashingUtility.SwitchEngBan(result.PenaltyDueAmount.ToString());
                result.HoldingTotalStr = HashingUtility.SwitchEngBan(result.HoldingTotal.ToString());
                result.PrevTotalStr = HashingUtility.SwitchEngBan(result.PrevTotal.ToString());
                result.TotalPenaltyStr = HashingUtility.SwitchEngBan(result.TotalPenalty.ToString());
                result.GrandTotalAmountStr = HashingUtility.SwitchEngBan(result.GrandTotalAmount.ToString());

            }
            commonParams.FinancialYearName = result != null ? result.FinancialYearName : "N/A";
            commonParams.PaymentDateStr = result != null ? result.LastPaymentDateStr : "";
            result.GrandAmountStr = numberToWord;
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
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

            //PageSettings pg = new PageSettings();
            //pg.Margins.Top = 0;
            //pg.Margins.Bottom = 0;
            //pg.Margins.Left = 0;
            //pg.Margins.Right = 0;
            //System.Drawing.Printing.PaperSize size = new PaperSize();
            //size.RawKind = (int)PaperKind.A5;
            //pg.PaperSize = size;
            //pg.Landscape = true;
            //reportViewer.SetPageSettings(pg);
            //this.reportViewer.ReportRefresh();

            //return File(bytes, "application/pdf");

            //Type tip = reportViewer.GetType();
            //FieldInfo[] pr = tip.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //System.Drawing.Printing.PageSettings ps = new System.Drawing.Printing.PageSettings();
            //ps.Landscape = true;
            /////ps......


            //foreach (FieldInfo item in pr)
            //{
            //    if (item.Name == "m_pageSettings")
            //    {
            //        item.SetValue(reportViewer, ps);

            //    }
            //}

            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/RPTFormatBillPrint/RPTFormatBillPrint.cshtml");

        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FromDate", commonParams.PaymentDateStr));
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));

            return paraList;
        }

        #endregion

    }
}