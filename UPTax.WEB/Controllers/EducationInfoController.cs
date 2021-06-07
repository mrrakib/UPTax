using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class EducationInfoController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IEducationInfoService _educationInfoService;

        public EducationInfoController(IEducationInfoService educationInfoService)
        {
            _educationInfoService = educationInfoService;
        }

        // GET: EducationInfo
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _educationInfoService.GetPagedList(name, page, dataSize);
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
        public ActionResult Create(EducationInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _educationInfoService.IsExistingItem(model.Degree, null);
                model.CreatedBy = _userId;
                if (!isExistingItem && _educationInfoService.Add(model))
                {
                    _message.save(this);
                }
                else
                {
                    _message.custom(this, "এই নামে একটি শিক্ষাগত যোগ্যতা আছে!");
                }
                return View(model);
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EducationInfo model = _educationInfoService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EducationInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _educationInfoService.IsExistingItem(model.Degree, model.Id);
                if (isExistingItem)
                {
                    _message.custom(this, "এই নামে একটি শিক্ষাগত যোগ্যতা আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _educationInfoService.Update(model);
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
            if (_educationInfoService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}