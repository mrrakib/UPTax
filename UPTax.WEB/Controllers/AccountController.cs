using UPTax.Data;
using UPTax.Helper;
using UPTax.Model.Models.Account;
using UPTax.Model.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;

namespace UPTax.Controllers
{
    public class AccountController : Controller
    {
        #region Global
        private UserManager<ApplicationUser> _userManager;
        private UserStore<ApplicationUser> _store;
        private readonly Message _message = new Message();
        #endregion
        public AccountController()
        {

        }

        private IAuthenticationManager _authnManager;
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                if (_authnManager == null)
                    _authnManager = HttpContext.GetOwinContext().Authentication;
                return _authnManager;
            }
            set { _authnManager = value; }
        }

        // GET: Account
        public ActionResult Register()
        {
            return View();
        }


        private IEnumerable<IdentityRole> _GetRoleList(AdminContext db)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            var roles = roleMngr.Roles.ToList();
            return roles;
        }

        #region Login Get
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string returnUrl, string id)
        {
            AuthenticationManager.SignOut();
            RapidSession.Clear();
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        #endregion

        #region Login Post
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Index(VMLogin model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            AdminContext db = new AdminContext();
            _store = new UserStore<ApplicationUser>(db);
            _userManager = new UserManager<ApplicationUser>(this._store);
            var user = _userManager.Find(model.UserName, model.Password);

            string contextNamedb = "Admin" + "_Context";
            RapidSession.Con = contextNamedb;

            if (user != null)
            {
                await SignInAsync(user, model.RememberMe, db);
                var role = _userManager.FindById(user.Id).Roles.Select(r => r.RoleId).FirstOrDefault();
                role.ToString();

                var userRole = db.Database.SqlQuery<UserRole>("SELECT TOP(1) ur.RoleId,r.Name FROM UserRoles AS ur INNER JOIN Roles AS r on r.Id=ur.RoleId WHERE UserId='" + user.Id + "'").FirstOrDefault();
                RapidSession.RoleId = userRole.RoleId;
                RapidSession.RoleName = userRole.Name;
                RapidSession.UserId = user.Id;
                RapidSession.UnionId = 1;
                RapidSession.UserFullName = user.FullName;
                return RedirectToAction("Index", "Dashboard");
                //return RedirectToRoute("Home", new { controller = "Home", action = "Index" });
            }

            ModelState.AddModelError("", @"ইউজারনেম অথবা পাসওয়ার্ড ভুল হয়েছে!");
            return View(model);

        }
        #endregion

        #region Log off
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            RapidSession.Clear();
            return RedirectToAction("Index", "Account");
        }
        #endregion

        private async Task SignInAsync(ApplicationUser user, bool isPersistent, AdminContext db)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            _store = new UserStore<ApplicationUser>(db);
            _userManager = new UserManager<ApplicationUser>(this._store);
            var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);

        }

        [RapidAuthorization(All = true)]
        [HttpGet]
        public ActionResult GetMenuSessionReady()
        {
            AdminContext db = new AdminContext();
            List<Menu> menues = GetMenues(RapidSession.RoleId, db);
            Session["Menues"] = menues;
            return PartialView("_sideBarHtml");
        }

        [RapidAuthorization(All = true)]
        [HttpPost]
        public ActionResult GetMenuSessionReady(string menu)
        {
            try
            {
                Session["htmlMenues"] = menu;
                return Json(new { success = true, responseText = "ok" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }


        #region Get all permitted menues
        private List<Menu> GetMenues(string roleId, AdminContext db)
        {
            var AuthorList1 = (Dictionary<string, string>)Session["APL"];
            Session.Remove("APL");
            Session.Remove("Menues");

            var PermittedMenues = (from t in db.MenuPermissions where (t.RoleId == roleId && t.IsViewPermitted == true) select t).ToList();

            var groupedResult = PermittedMenues.OrderBy(s => s.MenuConfig.MenuCategory.OrderNo).ThenBy(s => s.MenuConfig.OrderNo).GroupBy(s => s.MenuConfig.MenuCategory.Name).ToList();
            Dictionary<string, string> AuthorList = new Dictionary<string, string>();
            List<Menu> menuList = new List<Menu>();
            foreach (var item in groupedResult)
            {
                List<MenuItem> menuItems = new List<MenuItem>();
                string icon = "";
                foreach (var singleMenu in item)
                {
                    icon = singleMenu.MenuConfig.MenuCategory.Icon;
                    menuItems.Add(new MenuItem
                    {
                        ControllerName = singleMenu.MenuConfig.ControllerName,
                        ActionName = "Index",
                        MenuName = singleMenu.MenuConfig.MenuName
                    });

                    if (singleMenu.IsViewPermitted)
                    {
                        if (!AuthorList.ContainsKey(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "index"))
                        {
                            AuthorList.Add(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "index", "");
                        }

                    }
                    if (singleMenu.IsAddPermitted)
                    {
                        if (!AuthorList.ContainsKey(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "create"))
                        {
                            AuthorList.Add(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "create", "");
                        }

                    }
                    if (singleMenu.IsEditPermitted)
                    {
                        if (!AuthorList.ContainsKey(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "edit"))
                        {
                            AuthorList.Add(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "edit", "");

                        }

                    }
                    if (singleMenu.IsDeletePermitted)
                    {
                        if (!AuthorList.ContainsKey(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "delete"))
                        {
                            AuthorList.Add(singleMenu.MenuConfig.ControllerName.ToLower() + "_" + "delete", "");
                        }
                    }

                }
                menuList.Add(new Menu
                {
                    CategoryName = item.Key,
                    Icon = icon,
                    MenuList = menuItems,
                });
            }

            Session["APL"] = AuthorList;

            return menuList.ToList();
        }
        #endregion

        #region Unauthorized request view
        [AllowAnonymous]
        public ActionResult UnAuthorized()
        {
            return View();
        }
        #endregion

    }
}