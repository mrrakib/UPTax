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
    public class InstituteController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IInstituteInfoService _instituteInfoService;
        public readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;

        public InstituteController(IInstituteInfoService instituteInfoService, IWardInfoService wardInfoService, IVillageInfoService villageInfoService)
        {
            _instituteInfoService = instituteInfoService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
        }

        // GET: InstituteInfo
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _instituteInfoService.GetPagedList(name, page, dataSize);
            return View(listData);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            var villages = _villageInfoService.GetByWardId(_unionId).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
            ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name");
            ViewBag.InstituteType = new SelectList(_instituteInfoService.GetInstituteTypeDropdownItemList(), "IdStr", "Name");

            return View(new InstituteInfo());
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstituteInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _instituteInfoService.IsExistingItem(model.HoldingNo, null);
                model.CreatedBy = _userId;

                ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);
                var villages = _villageInfoService.GetByWardId(model.WardInfoId ?? 0).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
                ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name", model.VillageInfoId);
                ViewBag.InstituteType = new SelectList(_instituteInfoService.GetInstituteTypeDropdownItemList(), "IdStr", "Name", model.InstituteType);

                if (!isExistingItem && _instituteInfoService.Add(model))
                {
                    _message.save(this);
                    return View();
                }

                _message.custom(this, "এই নামে একটি কলেজ / অফিসের নাম আছে!");
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            InstituteInfo model = _instituteInfoService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);

            var villages = _villageInfoService.GetByWardId(model.WardInfoId ?? 0).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
            ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name", model.VillageInfoId);
            ViewBag.InstituteType = new SelectList(_instituteInfoService.GetInstituteTypeDropdownItemList(), "IdStr", "Name", model.InstituteType);

            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstituteInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _instituteInfoService.IsExistingItem(model.HoldingNo, model.Id);
                if (isExistingItem)
                {
                    ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", model.WardInfoId);
                    var villages = _villageInfoService.GetByWardId(model.WardInfoId ?? 0).Select(a => new { Id = a.Id, Name = a.VillageName }).ToList();
                    ViewBag.VillageInfoId = new SelectList(villages, "Id", "Name", model.VillageInfoId);
                    ViewBag.InstituteType = new SelectList(_instituteInfoService.GetInstituteTypeDropdownItemList(), "IdStr", "Name", model.InstituteType);

                    _message.custom(this, "এই নামে একটি কলেজ / অফিসের নাম আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _instituteInfoService.Update(model);
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
            if (_instituteInfoService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}