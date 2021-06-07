using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class SocialBenefitController : Controller
    {
        private readonly Message _message = new Message();
        private readonly ISocialBenefitService _SocialBenefitService;
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;

        public SocialBenefitController(ISocialBenefitService SocialBenefitService)
        {
            _SocialBenefitService = SocialBenefitService;
        }

        // GET: SocialBenefit
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _SocialBenefitService.GetPagedList(name, page, dataSize);
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
        public ActionResult Create(SocialBenefit model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _SocialBenefitService.IsExistingItem(model.Title, null);
                model.CreatedBy = _userId;
                if (!isExistingItem && _SocialBenefitService.Add(model))
                {
                    _message.save(this);
                }
                else
                {
                    _message.custom(this, "এই নামে একটি সামাজিক সুযোগ-সুবিধা আছে!");
                }
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            SocialBenefit model = _SocialBenefitService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SocialBenefit model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _SocialBenefitService.IsExistingItem(model.Title, model.Id);
                if (isExistingItem)
                {
                    _message.custom(this, "এই নামে একটি সামাজিক সুযোগ-সুবিধা আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _SocialBenefitService.Update(model);
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
            if (_SocialBenefitService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}