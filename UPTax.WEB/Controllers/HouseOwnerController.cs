using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
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
        private readonly IGenderService _genderService;
        private readonly IReligionService _religionService;
        private readonly string subPath = "/assets/UserImages";
        private readonly string uploadFileName = "ho_";


        public HouseOwnerController(IHouseOwnerService houseOwnerService,
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
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();


            ViewBag.wardId = ward;
            ViewBag.villageId = village;

            if (ward == 0)
            {
                ViewBag.ward = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            }
            else
            {
                ViewBag.ward = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", ward);
            }

            if (village == 0)
            {
                ViewBag.village = new SelectList(_villageInfoService.GetDropdownItemListByWard(ward), "Id", "Name");
            }
            else
            {
                ViewBag.village = new SelectList(_villageInfoService.GetDropdownItemListByWard(ward), "Id", "Name", village);
            }

            var listData = _houseOwnerService.GetPagedList(name, ward, village, page, dataSize);
            return View(listData);
        }

        // GET: HouseOwner/Create
        [RapidAuthorization]
        public ActionResult Create()
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

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HouseOwner model)
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
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            //ViewBag.Genders = _genderService.GetAll();
            ViewBag.GenderId = new SelectList(_genderService.GetDropdownItemList(), "Id", "Name", model.GenderId);
            ViewBag.ReligionId = new SelectList(_religionService.GetDropdownItemList(), "Id", "Name", model.ReligionId);
            //ViewBag.Religions = _religionService.GetAll();
            ViewBag.IsTubeWell = new SelectList(_houseOwnerService.GetTubeWellDropdownItemList(), "IdStr", "Name", model.IsTubeWell);
            ViewBag.Sanitary = new SelectList(_houseOwnerService.GetSanitaryDropdownItemList(), "IdStr", "Name", model.Sanitary);
            ViewBag.LivingType = new SelectList(_houseOwnerService.GetLivingTypeDropdownItemList(), "IdStr", "Name", model.LivingType);

            var villages = _villageInfoService.GetByWardId(model.WardInfoId).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
            ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name", model.VillageInfoId);

            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HouseOwner model)
        {
            if (ModelState.IsValid)
            {
                var hasFile = Request.Files[0];
                if (hasFile != null && hasFile.ContentLength > 0)
                {
                    bool exists = Directory.Exists(Server.MapPath(subPath));
                    if (exists)
                    {
                        var imageName = String.IsNullOrEmpty(model.ImageName) ? "" : model.ImageName;
                        var filteredByFilename = Directory
                                .GetFiles(Server.MapPath(subPath))
                                .Select(f => Path.GetFileName(f))
                                .Where(f => f.Equals(imageName));

                        if (filteredByFilename != null)
                        {
                            foreach (var filname in filteredByFilename)
                            {
                                var path = Path.Combine(Server.MapPath(subPath), filname);
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                            }

                        }
                    }
                    //delete previous file
                    var imageUpDetails = UploadImage(Request);
                    model.ImagePath = imageUpDetails.ImagePath;
                    model.ImageName = imageUpDetails.ImageName;
                }

                var isExistingItem = _houseOwnerService.IsExistingItem(model.HoldingNo, model.Id);
                if (!isExistingItem)
                {
                    model.UpdatedBy = _userId;
                    model.UpdatedDate = DateTime.UtcNow;
                    _houseOwnerService.Update(model);
                    _message.update(this);
                    return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                }
                _message.custom(this, "এই হোল্ডিং নাম্বার আছে!");
            }
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);
            ViewBag.EducationInfoId = new SelectList(_educationInfoService.GetDropdownItemList(), "Id", "Name", model.EducationInfoId);
            ViewBag.ProfessionId = new SelectList(_professionInfoService.GetDropdownItemList(), "Id", "Name", model.ProfessionId);

            var socialBenifits = _socialBenefitService.GetDropdownItemList();
            ViewBag.SocialBenefitBeforeId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitBeforeId);
            ViewBag.SocialBenefitEligibleId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitEligibleId); ;
            ViewBag.SocialBenefitRunningId = new SelectList(socialBenifits, "Id", "Name", model.SocialBenefitRunningId); ;

            ViewBag.GenderId = new SelectList(_genderService.GetDropdownItemList(), "Id", "Name", model.GenderId);
            ViewBag.ReligionId = new SelectList(_religionService.GetDropdownItemList(), "Id", "Name", model.ReligionId);
            ViewBag.IsTubeWell = new SelectList(_houseOwnerService.GetTubeWellDropdownItemList(), "IdStr", "Name", model.IsTubeWell);
            ViewBag.Sanitary = new SelectList(_houseOwnerService.GetSanitaryDropdownItemList(), "IdStr", "Name", model.Sanitary);
            ViewBag.LivingType = new SelectList(_houseOwnerService.GetLivingTypeDropdownItemList(), "IdStr", "Name", model.LivingType);

            var villages = _villageInfoService.GetByWardId(model.WardInfoId).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
            ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name", model.VillageInfoId);

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

        #region Image Upload
        private VMImage UploadImage(HttpRequestBase httpRequest)
        {
            string filePath = "";
            string fileName = "";
            if (httpRequest.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileExtension = Path.GetExtension(Request.Files["file"].FileName);
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        fileName = uploadFileName + Guid.NewGuid().ToString() + fileExt;

                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(subPath));

                        var path = Path.Combine(Server.MapPath(subPath), fileName);
                        file.SaveAs(path);
                        filePath = subPath + "/" + fileName;
                    }
                }
            }
            return new VMImage { ImageName = fileName, ImagePath = filePath };
        }
        #endregion
    }
}