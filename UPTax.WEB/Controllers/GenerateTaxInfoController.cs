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
    public class GenerateTaxInfoController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxGenerateInfoService _taxGenerateInfoService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IHouseOwnerService _houseOwnerService;
        #endregion

        #region constructor
        public GenerateTaxInfoController(ITaxGenerateInfoService taxGenerateInfoService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService)
        {
            _taxGenerateInfoService = taxGenerateInfoService;
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
        public ActionResult Index(VMTaxGenerator vm)
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", vm.FinancialYearId);

            if (ModelState.IsValid)
            {
                try
                {
                    TaxGenerateInfo model = new TaxGenerateInfo
                    {
                        FinancialYearId = vm.FinancialYearId,
                        HoldingNo = vm.HoldingNo,
                        HouseOwnerId = vm.VMTaxGeneratorDetails.HouseOwnerId,
                        UnionId = _unionId,
                        TaxPercentage = vm.YearlyTaxRate,
                        TotalTax = vm.VMTaxGeneratorDetails.TotalYearlyTax,
                        IsDeleted = false,
                        CreatedBy = RapidSession.UserId,
                        CreatedDate = DateTime.Now
                    };
                    if (_taxGenerateInfoService.Add(model))
                    {
                        _message.save(this);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _message.custom(this, "সেভ করতে সমস্যা হয়েছে!");
                    }

                }
                catch (Exception ex)
                {
                    _message.custom(this, ex.Message.ToString());
                }
            }
            
            return View(vm);
        }
        #endregion

        [RapidAuthorization]
        public ActionResult GenerateTax(string holdingNo, int finYearId)
        {
            int houseOwnerId = _houseOwnerService.GetIdByHoldingNum(holdingNo, _unionId);
            if (_taxGenerateInfoService.IsExistingItem(finYearId, houseOwnerId, null))
            {
                string error = "exist";
                _message.custom(this, "দুঃখিত! এই হোল্ডিং নাম্বারের জন্য এই বছরে কর জেনারেট করা হয়ে গেছে!");
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            VMTaxGenerator tax = _taxGenerateInfoService.GenerateSingleTax(holdingNo);
            tax.FinancialYearId = finYearId;

            return PartialView("~/Views/GenerateTaxInfo/_partialTaxInfo.cshtml", tax);
        }

        [RapidAuthorization]
        public ActionResult GetTaxRate(string holdingNo)
        {
            double houseOwnerTaxRate = _houseOwnerService.GetTaxRateByHoldingNum(holdingNo, _unionId);

            return Json(houseOwnerTaxRate, JsonRequestBehavior.AllowGet);
        }
    }
}