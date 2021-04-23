using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class MembersController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IMemberService _memberService;
        private readonly IRelationshipService _relationshipService;
        private readonly IEducationInfoService _educationInfoService;
        private readonly IProfessionInfoService _professionInfoService;
        private readonly ISocialBenefitService _socialBenefitService;
        private readonly IGenderService _genderService;
        private readonly IHouseOwnerService _houseOwnerService;
        private readonly IReligionService _religionService;

        public MembersController(IMemberService memberService,
            IRelationshipService relationshipService,
            IEducationInfoService educationInfoService,
            IProfessionInfoService professionInfoService,
            ISocialBenefitService socialBenefitService,
            IGenderService genderService,
            IHouseOwnerService houseOwnerService,
            IReligionService religionService
            )
        {
            _memberService = memberService;
            _relationshipService = relationshipService;
            _educationInfoService = educationInfoService;
            _professionInfoService = professionInfoService;
            _socialBenefitService = socialBenefitService;
            _genderService = genderService;
            _houseOwnerService = houseOwnerService;
            _religionService = religionService;
        }

        // GET: Members
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _memberService.GetPagedList(holdingNo: name, page, dataSize);
            return View(listData);
        }

        // GET: Members/Create
        [RapidAuthorization]
        public ActionResult Create()
        {
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name");
            ViewBag.RelationshipId = new SelectList(_relationshipService.GetDropdownItemList(), "Id", "Name");

            var socialBenifits = new SelectList(_socialBenefitService.GetDropdownItemList(), "Id", "Name");
            ViewBag.SocialBenefitBeforeId = socialBenifits;
            ViewBag.SocialBenefitEligibleId = socialBenifits;
            ViewBag.SocialBenefitRunningId = socialBenifits;

            ViewBag.Genders = _genderService.GetAll();
            ViewBag.Religions = _religionService.GetAll();

            return View(new Member());
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member model)
        {
            if (ModelState.IsValid)
            {
                var isExistHoldingNo = _houseOwnerService.IsExistingItem(model.HoldingNo);
                if (isExistHoldingNo)
                {
                    model.CreatedBy = _userId;
                    _memberService.Add(model);
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                _message.custom(this, "এই হোল্ডিং নাম্বার পাওয়া যায় নাই!");
            }

            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);
            ViewBag.RelationshipId = new SelectList(_relationshipService.GetDropdownItemList(), "Id", "Name", model.RelationshipId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            ViewBag.Genders = _genderService.GetAll();
            ViewBag.Religions = _religionService.GetAll();

            return View(model);
        }
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Member model = _memberService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);
            ViewBag.RelationshipId = new SelectList(_relationshipService.GetDropdownItemList(), "Id", "Name", model.RelationshipId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            ViewBag.Genders = _genderService.GetAll();
            ViewBag.Religions = _religionService.GetAll();

            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _memberService.Update(model);
                _message.update(this);
                return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
            }
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);
            ViewBag.RelationshipId = new SelectList(_relationshipService.GetDropdownItemList(), "Id", "Name", model.RelationshipId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            ViewBag.Genders = _genderService.GetAll();
            ViewBag.Religions = _religionService.GetAll();

            return View(model);
        }

        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_memberService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
    }
}