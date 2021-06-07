using System;
using System.Linq;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class CreateHouseOwnerController : Controller
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
        private readonly IGenderService _genderService;
        private readonly IReligionService _religionService;

        public CreateHouseOwnerController(IHouseOwnerService houseOwnerService,
            IWardInfoService wardInfoService,
            IVillageInfoService villageInfoService,
            IEducationInfoService educationInfoService,
            IProfessionInfoService professionInfoService,
            ISocialBenefitService socialBenefitService,
            IInfrastructureInfoService infrastructureInfoService,
            IGenderService genderService,
            IReligionService religionService
            )
        {
            _houseOwnerService = houseOwnerService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
            _educationInfoService = educationInfoService;
            _professionInfoService = professionInfoService;
            _socialBenefitService = socialBenefitService;
            _infrastructureInfoService = infrastructureInfoService;
            _genderService = genderService;
            _religionService = religionService;
        }

        // GET: HouseOwner
        [RapidAuthorization]
        public ActionResult Index(string name, int ward = 0, int village = 0, int page = 1, int dataSize = 10)
        {
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");

            var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
            ViewBag.SocialBenefitBeforeId = socialBenifits;
            ViewBag.SocialBenefitEligibleId = socialBenifits;
            ViewBag.SocialBenefitRunningId = socialBenifits;

            ViewBag.ReligionId = new SelectList(_religionService.GetDropdownItemList(), "Id", "Name");
            ViewBag.GenderId = new SelectList(_genderService.GetDropdownItemList(), "Id", "Name");
            ViewBag.IsTubeWell = new SelectList(_houseOwnerService.GetTubeWellDropdownItemList(), "IdStr", "Name");
            ViewBag.Sanitary = new SelectList(_houseOwnerService.GetSanitaryDropdownItemList(), "IdStr", "Name");
            ViewBag.LivingType = new SelectList(_houseOwnerService.GetLivingTypeDropdownItemList(), "IdStr", "Name");

            return View();
        }

        // GET: HouseOwner/Create
        

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HouseOwner model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _houseOwnerService.IsExistingItem(model.HoldingNo);
                model.CreatedBy = _userId;
                if (!isExistingItem && _houseOwnerService.Add(model))
                {
                    _message.save(this);
                }
                else
                {
                    _message.custom(this, "এই হোল্ডিং নাম্বার আছে!");
                }
            }

            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            ViewBag.ReligionId = new SelectList(_religionService.GetDropdownItemList(), "Id", "Name", model.ReligionId);
            ViewBag.GenderId = new SelectList(_genderService.GetDropdownItemList(), "Id", "Name", model.GenderId);
            ViewBag.IsTubeWell = new SelectList(_houseOwnerService.GetTubeWellDropdownItemList(), "IdStr", "Name", model.IsTubeWell);
            ViewBag.Sanitary = new SelectList(_houseOwnerService.GetSanitaryDropdownItemList(), "IdStr", "Name", model.Sanitary);
            ViewBag.LivingType = new SelectList(_houseOwnerService.GetLivingTypeDropdownItemList(), "IdStr", "Name", model.LivingType);

            return View(model);
        }
    }
}