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
    public class RPTProfessionController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IProfessionInfoService _professionInfoService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTProfessionController(IUnionParishadService unionParishadService, IFinancialYearService financialYearService, IProfessionInfoService professionInfoService)
        {
            _unionParishadService = unionParishadService;
            _financialYearService = financialYearService;
            _professionInfoService = professionInfoService;
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
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMRPTProfession> resultList = _professionInfoService.GetRPTProfessionList(commonParams.ProfessionId, _unionId);
            if (resultList.Count == 0)
            {
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                return View();
            }

            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTProfession.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
            
            if (resultList.Count > 0)
            {
                foreach (var result in resultList)
                {
                    result.HoldingNo = HashingUtility.SwitchEngBan(result.HoldingNo);
                    result.MobileNo = HashingUtility.SwitchEngBan(result.MobileNo);
                }
                

            }
            
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", resultList); //get actual data here

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
            return View("~/Views/RPTProfession/RPTProfessionReport.cshtml");

        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            
            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));

            return paraList;
        }

        #endregion

    }
}