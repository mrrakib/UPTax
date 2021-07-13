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
        private readonly ITaxGenerateInfoService _taxGenerateInfoService;
        #endregion

        #region constructor
        public TaxInstallmentEditController(ITaxInstallmentService taxInstallmentService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService, ITaxGenerateInfoService taxGenerateInfoService)
        {
            _taxInstallmentService = taxInstallmentService;
            _financialYearService = financialYearService;
            _houseOwnerService = houseOwnerService;
            _taxGenerateInfoService = taxGenerateInfoService;
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

                    if (vm.TaxGenerateId > 0)
                    {
                        TaxGenerateInfo modelUp = _taxGenerateInfoService.GetDetails(vm.TaxGenerateId);
                        if (modelUp != null)
                        {
                            modelUp.TotalTax = (double)vm.vMTaxInstallmentDetails.InstallmentAmount;
                            modelUp.UpdatedBy = RapidSession.UserId;
                            modelUp.UpdatedDate = DateTime.Now;

                            if (_taxGenerateInfoService.Update(modelUp))
                            {

                                TaxInstallment modelUpI = _taxInstallmentService.GetDetails(vm.Id);
                                if (modelUp != null)
                                {
                                    //decimal actualDue = modelUp.TaxAmount - vm.vMTaxInstallmentDetails.InstallmentAmount;
                                    HouseOwner owner = _houseOwnerService.GetDetails(vm.vMTaxInstallmentDetails.HouseOwnerId);
                                    //bool isPaid = false;
                                    modelUpI.TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
                                    modelUpI.OutstandingAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
                                    modelUpI.UpdatedBy = RapidSession.UserId;
                                    modelUpI.UpdatedDate = DateTime.Now;
                                    modelUpI.TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;

                                    if (_taxInstallmentService.Update(modelUpI))
                                    {
                                        _message.update(this);
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        _message.custom(this, "সেভ করতে সমস্যা হয়েছে!");
                                    }
                                }

                                _message.update(this);
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                _message.custom(this, "সেভ করতে সমস্যা হয়েছে!");
                            }
                        }

                        
                    }
                    else
                    {
                        TaxInstallment modelUp = _taxInstallmentService.GetDetails(vm.Id);
                        if (modelUp != null)
                        {
                            //decimal actualDue = modelUp.TaxAmount - vm.vMTaxInstallmentDetails.InstallmentAmount;
                            HouseOwner owner = _houseOwnerService.GetDetails(vm.vMTaxInstallmentDetails.HouseOwnerId);
                            //bool isPaid = false;
                            modelUp.TaxAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
                            modelUp.OutstandingAmount = vm.vMTaxInstallmentDetails.InstallmentAmount;
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