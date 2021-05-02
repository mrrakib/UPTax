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
    public class TaxInstallmentEditController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IHouseOwnerService _houseOwnerService;
        #endregion

        #region constructor
        public TaxInstallmentEditController(ITaxInstallmentService taxInstallmentService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService)
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
                    TaxInstallment modelUp = _taxInstallmentService.GetDetails(vm.Id);
                    if (modelUp != null)
                    {
                        decimal actualDue = modelUp.TaxAmount - vm.vMTaxInstallmentDetails.InstallmentAmount;
                        HouseOwner owner = _houseOwnerService.GetDetails(vm.vMTaxInstallmentDetails.HouseOwnerId);
                        bool isPaid = actualDue > 0 ? false : true;
                        if (actualDue > 0)
                        {
                            modelUp.OutstandingAmount = vm.vMTaxInstallmentDetails.InstallmentAmount - actualDue;
                        }
                        else
                        {
                            modelUp.OutstandingAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
                        }
                        modelUp.IsPaid = isPaid;
                        modelUp.UpdatedBy = RapidSession.UserId;
                        modelUp.UpdatedDate = DateTime.Now;
                        modelUp.TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
                        if (_taxInstallmentService.Update(modelUp))
                        {
                            _message.update(this);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _message.custom(this, "সেভ করতে সমস্যা হয়েছে!");
                        }
                    }
                    
                    
                    //TaxInstallment model = new TaxInstallment
                    //{
                    //    FinancialYearId = vm.FinancialYearId,
                    //    HoldingNo = vm.HoldingNo,
                    //    WardInfoId = owner.WardInfoId,
                    //    UnionId = _unionId,
                    //    TaxPaymentDate = vm.vMTaxInstallmentDetails.InstallmentDate,
                    //    TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount,
                    //    OutstandingAmount = vm.vMTaxInstallmentDetails.InstallmentAmount - actualDue,
                    //    PenaltyAmount = vm.vMTaxInstallmentDetails.PenaltyAmount,
                    //    IsPaid = isPaid,
                    //    IsDeleted = false,
                    //    CreatedBy = RapidSession.UserId,
                    //    CreatedDate = DateTime.Now
                    //};
                    

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

            return PartialView("~/Views/TaxInstallmentEdit/_partialTaxInstallmentEdit.cshtml", tax);
        }

    }
}