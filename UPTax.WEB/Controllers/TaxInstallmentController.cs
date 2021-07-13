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
    public class TaxInstallmentController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IHouseOwnerService _houseOwnerService;
        #endregion

        #region constructor
        public TaxInstallmentController(ITaxInstallmentService taxInstallmentService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService)
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
                    decimal actualDue = vm.vMTaxInstallmentDetails.InstallmentAmount - vm.vMTaxInstallmentDetails.DueAmount;
                    HouseOwner owner = _houseOwnerService.GetDetails(vm.vMTaxInstallmentDetails.HouseOwnerId);
                    //bool isPaid = actualDue > 0 ? false : true;
                    TaxInstallment model = new TaxInstallment
                    {
                        FinancialYearId = vm.FinancialYearId,
                        HoldingNo = vm.HoldingNo,
                        WardInfoId = owner.WardInfoId,
                        UnionId = _unionId,
                        TaxPaymentDate = vm.vMTaxInstallmentDetails.InstallmentDate,
                        TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount,
                        OutstandingAmount = (actualDue == vm.vMTaxInstallmentDetails.InstallmentAmount) ? vm.vMTaxInstallmentDetails.InstallmentAmount : (vm.vMTaxInstallmentDetails.InstallmentAmount - actualDue),
                        PenaltyAmount = vm.vMTaxInstallmentDetails.PenaltyAmount,
                        IsPaid = true,
                        IsDeleted = false,
                        CreatedBy = RapidSession.UserId,
                        CreatedDate = DateTime.Now
                    };
                    if (vm.Id > 0)
                    {
                        if (_taxInstallmentService.Delete(vm.Id))
                        {
                            model.IsPaid = true;
                        }
                    }
                    if (_taxInstallmentService.Add(model))
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

        [RapidAuthorization(All = true)]
        public ActionResult GenerateTaxInstallMent(string holdingNo, int finYearId)
        {
            if (_taxInstallmentService.IsExistingItem(holdingNo, finYearId, null))
            {
                string error = "exist";
                _message.custom(this, "দুঃখিত! এই হোল্ডিং নাম্বারের জন্য এই বছরে কর জমা করা হয়ে গেছে!");
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            VMTaxInstallment tax = _taxInstallmentService.GenerateSingleTaxInstallment(holdingNo, finYearId);

            return PartialView("~/Views/TaxInstallment/_partialTaxInstallment.cshtml", tax);
        }

    }
}