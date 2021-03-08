using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class InstituteController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IInstituteInfoService _instituteInfoService;

        public InstituteController(IInstituteInfoService instituteInfoService)
        {
            _instituteInfoService = instituteInfoService;
        }
        // GET: InstituteInfo
        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _instituteInfoService.GetPagedList(degree: name, page, dataSize);
            return View(listData);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InstituteInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _instituteInfoService.IsExistingItem(model.NameOfInstitute);
                model.CreatedBy = _userId;
                if (!isExistingItem && _instituteInfoService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                _message.custom(this, "এই নামে একটি কলেজ / অফিসের নাম আছে!");
                return View(model);
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
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InstituteInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _instituteInfoService.IsExistingItem(model.NameOfInstitute);
                if (isExistingItem)
                {
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