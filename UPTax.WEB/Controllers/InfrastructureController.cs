using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class InfrastructureController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IInfrastructureInfoService _infrastructureInfoService;

        public InfrastructureController(IInfrastructureInfoService infrastructureInfoService)
        {
            _infrastructureInfoService = infrastructureInfoService;
        }
        // GET: Infrastructure
        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var listData = _infrastructureInfoService.GetPagedList(degree: name, page, dataSize);
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
        public ActionResult Create(InfrastructureInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _infrastructureInfoService.IsExistingItem(model.HoldingNo);
                model.CreatedBy = _userId;
                if (!isExistingItem && _infrastructureInfoService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                _message.custom(this, "এই নামে একটি পেশা আছে!");
                return View(model);
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            InfrastructureInfo model = _infrastructureInfoService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InfrastructureInfo model)
        {
            if (ModelState.IsValid)
            {
                var isExistingItem = _infrastructureInfoService.IsExistingItem(model.HoldingNo);
                if (isExistingItem)
                {
                    _message.custom(this, "এই নামে একটি পেশা আছে!");
                    return View(model);
                }
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _infrastructureInfoService.Update(model);
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
            if (_infrastructureInfoService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}