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
    public class RPTTranController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IWardInfoService _wardInfoService;
        private readonly Message _message = new Message();

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTranController(IUnionParishadService unionParishadService, IWardInfoService wardInfoService, IFinancialYearService financialYearService)
        {
            _unionParishadService = unionParishadService;
            _wardInfoService = wardInfoService;
            _financialYearService = financialYearService;
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
            //ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);

            List<VMTranInfo> result = _wardInfoService.GetTranInfo(_unionId);
            if (result.Count == 0)
            {
                ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", commonParams.FinancialYearId);
                _message.custom(this, "দুঃখিত! কোন তথ্য পাওয়া যায়নি।");
                return View();
            }
            int GrandHouseOwnerCount = result.Sum(g => g.TotalHouseOwner);
            int GrandMaleCount = result.Sum(g => g.TotalMale);
            int GrandFeMaleCount = result.Sum(g => g.TotalFemale);
            int GrandSocialCount = result.Sum(g => g.TotalSocialBenefitTakingCount);
            int GrandPoorCount = result.Sum(g => g.TotalPoor);
            int GrandSemiPoorCount = result.Sum(g => g.TotalMidPoor);
            int GrandRichCount = result.Sum(g => g.TotalRich);
            int GrandPopulationCount = result.Sum(g => g.TotalPopulation);
            foreach (var item in result)
            {
                item.TotalHouseOwnerStr = HashingUtility.SwitchEngBan(item.TotalHouseOwner.ToString());
                item.TotalMaleStr = HashingUtility.SwitchEngBan(item.TotalMale.ToString());
                item.TotalFemaleStr = HashingUtility.SwitchEngBan(item.TotalFemale.ToString());
                item.TotalSocialBenefitTakingCountStr = HashingUtility.SwitchEngBan(item.TotalSocialBenefitTakingCount.ToString());
                item.TotalPoorStr = HashingUtility.SwitchEngBan(item.TotalPoor.ToString());
                item.TotalMidPoorStr = HashingUtility.SwitchEngBan(item.TotalMidPoor.ToString());
                item.TotalRichStr = HashingUtility.SwitchEngBan(item.TotalRich.ToString());
                item.TotalPopulationStr = HashingUtility.SwitchEngBan(item.TotalPopulation.ToString());

                item.GrandTotalMaleStr = HashingUtility.SwitchEngBan(GrandMaleCount.ToString());
                item.GrandTotalFemaleStr = HashingUtility.SwitchEngBan(GrandFeMaleCount.ToString());
                item.GrandTotalSocialBenefitTakingCountStr = HashingUtility.SwitchEngBan(GrandSocialCount.ToString());
                item.GrandTotalPoorStr = HashingUtility.SwitchEngBan(GrandPoorCount.ToString());
                item.GrandTotalMidPoorStr = HashingUtility.SwitchEngBan(GrandSemiPoorCount.ToString());
                item.GrandTotalRichStr = HashingUtility.SwitchEngBan(GrandRichCount.ToString());
                item.GrandTotalPopulationStr = HashingUtility.SwitchEngBan(GrandPopulationCount.ToString());
                item.GrandTotalHouseOwnerStr = HashingUtility.SwitchEngBan(GrandHouseOwnerCount.ToString());

            }

            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.ShowToolBar = false;
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/RPTTran.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));

            commonParams.FinancialYearName = "2020-2021";

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
            //string deviceInf = "<DeviceInfo><PageHeight>14in</PageHeight><PageWidth>9.5in</PageWidth></DeviceInfo>";
            //byte[] bytes = reportViewer.LocalReport.Render("PDF", deviceInf, out mimeType, out encoding, out extension, out streamIds, out warnings);


            //return File(bytes, "application/pdf");
            ViewBag.ReportViewer = reportViewer;
            return View("~/Views/RPTTran/RPTTran.cshtml");


        }

        #region GetReportParameter

        public List<ReportParameter> GetReportParameter(VMCommonParams commonParams)
        {
            UnionParishad union = _unionParishadService.GetDetails(_unionId);
            var year = _financialYearService.GetNameById(commonParams.FinancialYearId);
            year = HashingUtility.SwitchEngBan(year);

            List<ReportParameter> paraList = new List<ReportParameter>();
            paraList.Add(new ReportParameter("UnionName", union.Name));
            paraList.Add(new ReportParameter("UnionAddress", union.Description));
            paraList.Add(new ReportParameter("FinYear", year));

            return paraList;
        }

        #endregion


    }
}