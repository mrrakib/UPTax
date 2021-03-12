using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
using UPTax.Service.Services;
using UPTax.Service.Services.Permissions;

namespace UPTax.Controllers
{
    public class MenuPermissionController : Controller
    {
        #region Global Variables
        private Message _message = new Message();

        private readonly IMenuConfigService _menuConfigService;
        private readonly IMenuCategoryService _menuCategoryService;
        private readonly IMenuPermissionService _menuPermissionService;
        private readonly IRoleService _roleService;
        #endregion

        #region constructor
        public MenuPermissionController(IMenuConfigService menuConfigService, IMenuCategoryService menuCategoryService, IMenuPermissionService menuPermissionService, IRoleService roleService)
        {
            _menuConfigService = menuConfigService;
            _menuCategoryService = menuCategoryService;
            _menuPermissionService = menuPermissionService;
            _roleService = roleService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index()
        {
            ViewBag.RoleId = new SelectList(_roleService.GetAll(), "Id", "Name");
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name");
            return View();
        }

        #region Post Index

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(VMMenuPermission vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in vm.MenuPermissionDetails)
                    {
                        MenuPermission permission = new MenuPermission();
                        permission.RoleId = vm.RoleId;
                        permission.MenuConfigId = item.MenuConfigId;
                        permission.IsViewPermitted = item.IsViewPermit;
                        permission.IsAddPermitted = item.IsAddPermit;
                        permission.IsEditPermitted = item.IsEditPermit;
                        permission.IsDeletePermitted = item.IsDeletePermit;
                        _menuPermissionService.Add(permission);
                    }
                    if (_menuPermissionService.DeleteAllPermittedMenues(vm.RoleId, vm.CategoryId))
                    {
                        _menuPermissionService.Save();
                        _message.save(this);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    _message.custom(this, ex.Message.ToString());
                }
            }
            ViewBag.RoleId = new SelectList(_roleService.GetAll(), "Id", "Name", vm.RoleId);
            ViewBag.CategoryId = new SelectList(_menuCategoryService.GetMenuCategoryDDL(), "Id", "Name", vm.CategoryId);
            return View(vm);
        }
        #endregion

        public ActionResult GetMenuesForPermission(string roleId, int categoryId)
        {
            VMMenuPermission menuPermission = new VMMenuPermission();
            List<VMMenuPermissionDetails> detailMenuList = new List<VMMenuPermissionDetails>();
            var menuList = _menuConfigService.GetAllMenuByCatId(categoryId);
            var permissionList = _menuPermissionService.GetAllPermittedMenues(roleId, categoryId);
            foreach (var menu in menuList)
            {
                var view = permissionList.Where(mp => mp.MenuConfigId == menu.Id).FirstOrDefault();
                detailMenuList.Add(new VMMenuPermissionDetails
                {
                    MenuConfigId = menu.Id,
                    MenuName = menu.MenuName,
                    IsViewPermit = view == null ? false : view.IsViewPermitted,
                    IsAddPermit = view == null ? false : view.IsAddPermitted,
                    IsEditPermit = view == null ? false : view.IsEditPermitted,
                    IsDeletePermit = view == null ? false : view.IsDeletePermitted
                });
            }
            menuPermission.MenuPermissionDetails = detailMenuList;
            return PartialView("~/Views/MenuPermission/_partialPermission.cshtml", menuPermission);
        }
    }
}