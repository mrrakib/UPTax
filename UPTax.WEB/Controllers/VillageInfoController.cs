using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class VillageInfoController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IVillageInfoService _VillageInfoService;
        private readonly IUnionParishadService _unionParishadService;

        public VillageInfoController(IVillageInfoService VillageInfoService, IUnionParishadService unionParishadService)
        {
            _VillageInfoService = VillageInfoService;
            _unionParishadService = unionParishadService;
        }

        // GET: VillageInfo
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _VillageInfoService.GetPagedList(keyName: name, page, dataSize);
            return View(listData);
        }

        [HttpGet]
        [RapidAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VillageInfo model)
        {
            model.UnionId = _unionId;
            if (ModelState.IsValid)
            {
                var isExistingItem = _VillageInfoService.IsExistingItem(model.VillageName, null);
                if (isExistingItem)
                {
                    _message.custom(this, "এই নামে একটি গ্রাম আছে!");
                    return View(model);
                }
                model.CreatedBy = _userId;
                _VillageInfoService.Add(model);
                _message.save(this);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            VillageInfo model = _VillageInfoService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VillageInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _VillageInfoService.IsExistingItem(model.VillageName, model.Id);
                if (isExistingItem)
                {
                    _message.custom(this, "এই নামে একটি গ্রাম আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _VillageInfoService.Update(model);
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
            if (_VillageInfoService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}