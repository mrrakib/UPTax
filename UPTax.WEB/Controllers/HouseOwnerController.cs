using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class HouseOwnerController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IHouseOwnerService _houseOwnerService;
        private readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;
        private readonly IEducationInfoService _educationInfoService;
        private readonly IProfessionInfoService _professionInfoService;
        private readonly ISocialBenefitService _socialBenefitService;
        private readonly IInfrastructureInfoService _infrastructureInfoService;

        public HouseOwnerController(IHouseOwnerService houseOwnerService, IWardInfoService wardInfoService, IVillageInfoService villageInfoService, IEducationInfoService educationInfoService, IProfessionInfoService professionInfoService, ISocialBenefitService socialBenefitService, IInfrastructureInfoService infrastructureInfoService)
        {
            _houseOwnerService = houseOwnerService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
            _educationInfoService = educationInfoService;
            _professionInfoService = professionInfoService;
            _socialBenefitService = socialBenefitService;
            _infrastructureInfoService = infrastructureInfoService;
        }
        // GET: HouseOwner
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _houseOwnerService.GetPagedList(holdingNo: name, page, dataSize);
            return View(listData);
        }

        // GET: HouseOwner/Create
        [RapidAuthorization]
        public ActionResult Create()
        {
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.InfrastructureTypeId = new SelectList(_infrastructureInfoService.GetDropdownItemList(), "Id", "Name");

            var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
            ViewBag.SocialBenefitBeforeId = socialBenifits;
            ViewBag.SocialBenefitEligibleId = socialBenifits;
            ViewBag.SocialBenefitRunningId = socialBenifits;

            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HouseOwner model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _houseOwnerService.IsExistingItem(model.HoldingNo, null);
                model.CreatedBy = _userId;
                if (!isExistingItem && _houseOwnerService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
                ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
                ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
                ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
                ViewBag.InfrastructureTypeId = new SelectList(_infrastructureInfoService.GetDropdownItemList(), "Id", "Name");

                var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
                ViewBag.SocialBenefitBeforeId = socialBenifits;
                ViewBag.SocialBenefitEligibleId = socialBenifits;
                ViewBag.SocialBenefitRunningId = socialBenifits;
                _message.custom(this, "এই নামে একটি সামাজিক সুযোগ-সুবিধা আছে!");
                return View(model);
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            HouseOwner model = _houseOwnerService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.InfrastructureTypeId = new SelectList(_infrastructureInfoService.GetDropdownItemList(), "Id", "Name");

            var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
            ViewBag.SocialBenefitBeforeId = socialBenifits;
            ViewBag.SocialBenefitEligibleId = socialBenifits;
            ViewBag.SocialBenefitRunningId = socialBenifits;
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HouseOwner model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _houseOwnerService.IsExistingItem(model.HoldingNo, model.Id);
                if (isExistingItem)
                {
                    ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
                    ViewBag.VillageInfoId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
                    ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
                    ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
                    ViewBag.InfrastructureTypeId = new SelectList(_infrastructureInfoService.GetDropdownItemList(), "Id", "Name");

                    var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
                    ViewBag.SocialBenefitBeforeId = socialBenifits;
                    ViewBag.SocialBenefitEligibleId = socialBenifits;
                    ViewBag.SocialBenefitRunningId = socialBenifits;
                    _message.custom(this, "এই নামে একটি সামাজিক সুযোগ-সুবিধা আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _houseOwnerService.Update(model);
                _message.update(this);
                return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
            }
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_houseOwnerService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}