using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
using UPTax.Service.Services;
using UPTax.Service.Services.Permissions;

namespace UPTax.Controllers
{
    public class PersonalShortReportController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxInstallmentService _taxInstallmentService;
        #endregion

        #region constructor
        public PersonalShortReportController(ITaxInstallmentService taxInstallmentService)
        {
            _taxInstallmentService = taxInstallmentService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }


        [RapidAuthorization(All = true)]
        public ActionResult GetPersonalShortReport(string holdingNo)
        {
            VMPersonalShortReport tax = _taxInstallmentService.GetPersonalShortReport(_unionId, holdingNo);

            return PartialView("~/Views/PersonalShortReport/_partialPersonalShortReport.cshtml", tax);
        }

    }
}