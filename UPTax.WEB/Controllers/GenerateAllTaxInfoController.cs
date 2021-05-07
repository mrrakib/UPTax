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
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class GenerateAllTaxInfoController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly ITaxGenerateInfoService _taxGenerateInfoService;
        private readonly IFinancialYearService _financialYearService;
        private readonly IHouseOwnerService _houseOwnerService;
        private readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;
        #endregion

        #region constructor
        public GenerateAllTaxInfoController(ITaxGenerateInfoService taxGenerateInfoService, IFinancialYearService financialYearService, IHouseOwnerService houseOwnerService, IWardInfoService wardInfoService, IVillageInfoService villageInfoService)
        {
            _taxGenerateInfoService = taxGenerateInfoService;
            _financialYearService = financialYearService;
            _houseOwnerService = houseOwnerService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            return View();
        }

        #region Post Index

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMCommonParams vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<VMAllTaxGenerator> allTaxInfoList = _taxGenerateInfoService.GenerateAllTax(vm.VillageInfoId, vm.WardInfoId, vm.FinancialYearId, _unionId);
                    List<TaxGenerateInfo> result = new List<TaxGenerateInfo>();

                    if (allTaxInfoList.Count > 0)
                    {
                        foreach (var item in allTaxInfoList)
                        {
                            TaxGenerateInfo model = new TaxGenerateInfo
                            {
                                FinancialYearId = vm.FinancialYearId,
                                HoldingNo = item.HoldingNo,
                                HouseOwnerId = item.HouseOwnerId,
                                UnionId = _unionId,
                                TaxPercentage = item.YearlyTaxRate,
                                TotalTax = item.TotalYearlyTax,
                                IsDeleted = false,
                                CreatedBy = RapidSession.UserId,
                                CreatedDate = DateTime.Now
                            };
                            result.Add(model);
                        }
                        if (_taxGenerateInfoService.AddRange(result))
                        {
                            _message.save(this);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _message.custom(this, "সেভ করতে সমস্যা হয়েছে!");
                        }
                    }
                    else
                    {
                        _message.custom(this, "এই অর্থবছরের সকল কর জেনারেট করা হয়ে গেছে।");
                    }
                    

                }
                catch (Exception ex)
                {
                    _message.custom(this, ex.Message.ToString());
                }
            }


            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", vm.FinancialYearId);
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", vm.WardInfoId);
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name", vm.VillageInfoId);

            return View(vm);
        }
        #endregion


        [RapidAuthorization(All = true)]
        public ActionResult GetTaxRate(string holdingNo)
        {
            double houseOwnerTaxRate = _houseOwnerService.GetTaxRateByHoldingNum(holdingNo, _unionId);

            return Json(houseOwnerTaxRate, JsonRequestBehavior.AllowGet);
        }
    }
}