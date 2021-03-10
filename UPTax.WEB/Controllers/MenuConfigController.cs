using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class MenuConfigController : Controller
    {
        #region Global Variables
        private Message _message = new Message();

        private readonly IMenuConfigService _menuConfigService;
        private readonly IMenuCategoryService _menuCategoryService;
        #endregion

        #region constructor
        public MenuConfigController(IMenuConfigService menuConfigService, IMenuCategoryService menuCategoryService)
        {
            _menuConfigService = menuConfigService;
            _menuCategoryService = menuCategoryService;
        }
        #endregion
        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _menuConfigService.GetPagedList(name, page, dataSize);
            return View(unionList);
        }

        #region Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuConfig model)
        {
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name", model.CategoryId);
            if (ModelState.IsValid)
            {
                model.CreatedBy = RapidSession.UserId;
                var existingItem = _menuConfigService.IsExistingItem(model.MenuName, null);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি মেনু আছে!");
                    return View(model);
                }
                model.MenuName.Trim();
                if (_menuConfigService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                else
                {
                    _message.custom(this, "মেনু যোগ করতে সমস্যা হয়েছে!");
                }
                
                return View(model);
            }
            return View(model);
        }
        #endregion

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            MenuConfig model = _menuConfigService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name", model.CategoryId);
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuConfig model)
        {
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name", model.CategoryId);
            if (ModelState.IsValid)
            {
                model.UpdatedBy = RapidSession.UserId;
                model.UpdatedDate = DateTime.Now;
                var existingItem = _menuConfigService.IsExistingItem(model.MenuName, model.Id);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি মেনু আছে!");
                    return View(model);
                }
                model.MenuName.Trim();
                if (_menuConfigService.Update(model))
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
            if (_menuConfigService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}