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
            if (ModelState.IsValid)
            {
                try
                {
                    //foreach (var item in vm.MenuPermissionDetails)
                    //{
                    //    MenuPermission permission = new MenuPermission();
                    //    permission.RoleId = vm.RoleId;
                    //    permission.MenuConfigId = item.MenuConfigId;
                    //    permission.IsViewPermitted = item.IsViewPermit;
                    //    permission.IsAddPermitted = item.IsAddPermit;
                    //    permission.IsEditPermitted = item.IsEditPermit;
                    //    permission.IsDeletePermitted = item.IsDeletePermit;

                    //    permission.CreatedBy = RapidSession.UserId;
                    //    _menuPermissionService.Add(permission);
                    //}
                    //if (_menuPermissionService.DeleteAllPermittedMenues(vm.RoleId, vm.CategoryId))
                    //{
                    //    _menuPermissionService.Save();
                    //    _message.save(this);
                    //    return RedirectToAction("Index");
                    //}
                }
                catch (Exception ex)
                {
                    _message.custom(this, ex.Message.ToString());
                }
            }
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", vm.FinancialYearId);
            return View(vm);
        }
        #endregion

        [RapidAuthorization]
        public ActionResult GenerateTax(string holdingNo, int finYearId)
        {
            int houseOwnerId = _houseOwnerService.GetIdByHoldingNum(holdingNo, _unionId);
            if (_taxGenerateInfoService.IsExistingItem(finYearId, houseOwnerId, null))
            {
                _message.custom(this, "দুঃখিত! এই হোল্ডিং নাম্বারের জন্য এই বছরে কর জেনারেট করা হয়ে গেছে!");
                return RedirectToAction("Index");
            }
            VMTaxGenerator tax = _taxGenerateInfoService.GenerateSingleTax(holdingNo);
            tax.FinancialYearId = finYearId;

            return PartialView("~/Views/MenuPermission/_partialPermission.cshtml", tax);
        }
    }
}