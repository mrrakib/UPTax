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
    public class TaxInstallmentDeleteController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IHouseOwnerService _houseOwnerService;
        #endregion

        #region constructor
        public TaxInstallmentDeleteController(ITaxInstallmentService taxInstallmentService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService)
        {
            _taxInstallmentService = taxInstallmentService;
            _financialYearService = financialYearService;
            _houseOwnerService = houseOwnerService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        #region Post Index

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMTaxInstallment vm)
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", vm.FinancialYearId);

            if (ModelState.IsValid)
            {
                try
                {
                    if (_taxInstallmentService.Delete(vm.Id))
                    {
                        _message.delete(this);
                        return RedirectToAction("Index");
                    }
                    return PartialView("_Error");


                }
                catch (Exception ex)
                {
                    _message.custom(this, ex.Message.ToString());
                }
            }

            return View(vm);
        }
        #endregion

        [RapidAuthorization(All = true)]
        public ActionResult GetTaxInstallMent(string holdingNo, int finYearId)
        {
            VMTaxInstallment tax = _taxInstallmentService.GeteSingleTaxInstallment(holdingNo, finYearId);

            return PartialView("~/Views/TaxInstallmentDelete/_partialTaxInstallmentDelete.cshtml", tax);
        }

    }
}