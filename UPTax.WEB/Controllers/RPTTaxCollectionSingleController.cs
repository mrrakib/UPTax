﻿using Microsoft.Reporting.WebForms;
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
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class RPTTaxCollectionSingleController : Controller
    {
        #region Global Variables
        private readonly IUnionParishadService _unionParishadService;

        private readonly int _unionId = RapidSession.UnionId;
        #endregion

        #region Constructor
        public RPTTaxCollectionSingleController(IUnionParishadService unionParishadService)
        {
            _unionParishadService = unionParishadService;
        }
        #endregion
        // GET: RPTTaxCollectionList
        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams commonParams)
        {
            List<VMCommonParams> parList = new List<VMCommonParams>();
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            reportViewer.PageCountMode = new PageCountMode();
            reportViewer.LocalReport.ReportPath = Request.MapPath("~/Reports/RDLC/Report1.rdlc");
            //reportViewer.LocalReport.SetParameters(GetReportParameter(data));
            commonParams.FinancialYearName = "2020-2021";
            reportViewer.LocalReport.SetParameters(GetReportParameter(commonParams));
            ReportDataSource A = new ReportDataSource("DataSet1", parList); //get actual data here

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
            paraList.Add(new ReportParameter("FinYear", commonParams.FinancialYearName));
            //paraList.Add(new ReportParameter("FromDate", commonParams.PaymentDate.Value.ToString("dd MMM, yyyy")));

            return paraList;
        }

        #endregion

    }
}