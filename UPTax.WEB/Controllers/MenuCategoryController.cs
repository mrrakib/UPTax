using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class MenuCategoryController : Controller
    {
        #region Global Variables
        private Message _message = new Message();

        private readonly IMenuCategoryService _menuCategoryService;
        #endregion

        #region constructor
        public MenuCategoryController(IMenuCategoryService menuCategoryService)
        {
            _menuCategoryService = menuCategoryService;
        }
        #endregion
        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _menuCategoryService.GetPagedList(name, page, dataSize);
            return View(unionList);
        }

        #region Create
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuCategory model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = RapidSession.UserId;
                var existingItem = _menuCategoryService.IsExistingItem(model.Name, null);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি ক্যাটাগরি আছে!");
                    return View(model);
                }
                model.Name.Trim();
                if (_menuCategoryService.Add(model))
                {
                    _message.save(this);
                }
                else
                {
                    _message.custom(this, "ক্যাটাগরি যোগ করতে সমস্যা হয়েছে!");
                }
            }
            return View(model);
        }
        #endregion

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            MenuCategory model = _menuCategoryService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuCategory model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = RapidSession.UserId;
                model.UpdatedDate = DateTime.Now;
                var existingItem = _menuCategoryService.IsExistingItem(model.Name, model.Id);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি ক্যাটাগরি আছে!");
                    return View(model);
                }
                model.Name.Trim();
                if (_menuCategoryService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                }
                else
                {
                    _message.custom(this, "ক্যাটাগরি হালনাগাদ করতে সমস্যা হয়েছে!");
                }


            }
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_menuCategoryService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}