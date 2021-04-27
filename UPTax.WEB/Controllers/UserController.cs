using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using UPTax.Data;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.Account;
using UPTax.Model.ViewModels;
using UPTax.Service.Services.Autofac;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class UserController : Controller
    {
        #region Global Variables
        private AdminContext db = new AdminContext();
        private UserStore<ApplicationUser> store;
        private UserManager<ApplicationUser> UserManager;
        private Message _message = new Message();
        string Password = "UnionTax";

        private readonly IUserService _userService;
        private readonly IUnionParishadService _unionParishadService;
        #endregion

        #region constructor
        public UserController(IUserService userService, IUnionParishadService unionParishadService)
        {
            _userService = userService;
            _unionParishadService = unionParishadService;
        }
        #endregion
        // GET: User Rakib
        //[HttpGet]
        [RapidAuthorization]
        public ActionResult Index(int pageNo = 1, int dataSize = 10)
        {
            //List<string> rolesList = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();

            ViewBag.dataSize = dataSize;
            ViewBag.pageNo = pageNo;

            var userList = _userService.GetUserPaged(pageNo, dataSize);
            return View(userList);
        }

        #region Create
        [HttpGet]
        [RapidAuthorization]
        public ActionResult Create()
        {
            VMRegister vmRegister = new VMRegister { roles = _GetRoleList() };
            ViewBag.RoleName = new SelectList(_GetRoleList(), "Name", "Name");
            ViewBag.UnionId = new SelectList(_unionParishadService.GetAllForDropdown(), "Id", "Name");
            return View(vmRegister);
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMRegister model)
        {

            string errorMsg = "";
            ViewBag.RoleName = new SelectList(_GetRoleList(), "Name", "Name", model.RoleName);
            ViewBag.UnionId = new SelectList(_unionParishadService.GetAllForDropdown(), "Id", "Name", model.UnionId);

            if (ModelState.IsValid)
            {
                model.IsActive = true;
                //db = new EasyContext();
                this.store = new UserStore<ApplicationUser>(db);
                this.UserManager = new UserManager<ApplicationUser>(this.store);

                this.UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

                //check for username that already exist 

                var foundUser = from u in db.Users
                                where u.UserName == model.UserName
                                select u.Id;

                if (foundUser.Count() > 0)
                {
                    // already exist
                    _message.custom(this, "এই ব্যবহারকারীর নামটি ইতিমধ্যে বিদ্যমান!");

                    model.roles = _GetRoleList();
                    return View(model);
                }
                var user = model.GetUser();

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var idManager = new IdentityManager(store);

                    try
                    {
                        List<string> rolesList = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();

                        idManager.ClearUserRoles(user.Id, rolesList);
                    }
                    catch
                    {
                        _message.custom(this, "ব্যবহারকারীর রোল যোগ করার সময় সমস্যা দেখা দিয়েছে!");
                        RedirectToAction("Login");
                    }
                    IdentityResult isSuccess = new IdentityResult();

                    try
                    {
                        isSuccess = UserManager.AddToRole(user.Id, "Admin");
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (isSuccess.Succeeded)
                    {
                        ApplicationUser userDetails = (from u in db.Users where u.Id == user.Id select u).FirstOrDefault();
                        userDetails.EmployeeId = model.EmployeeId;
                        userDetails.IsActive = model.IsActive;
                        userDetails.PhoneNumber = model.ContactNo;
                        userDetails.UnionId = model.UnionId;
                        db.SaveChanges();
                        //_userService.SaveUser();

                        _message.success(this, "নিবন্ধন সফল হয়েছে!");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _message.custom(this, "ব্যবহারকারীর রোল যোগ করার সময় সমস্যা দেখা দিয়েছে!");
                        return RedirectToAction("Index");
                    }
                }
                else
                {

                    if (result.Errors != null)
                    {
                        foreach (var item in result.Errors)
                        {
                            errorMsg = errorMsg + "<br> " + item;
                        }
                    }
                }
            }
            _message.custom(this, "কিছু সমস্যার কারণে নিবন্ধন শেষ হচ্ছে না!" + "<br /> " + errorMsg);
            model.roles = _GetRoleList();
            return View(model);
        }
        #endregion

        #region Edit
        [HttpGet]
        [RapidAuthorization]
        public async Task<ActionResult> Edit(string id)
        {
            try
            {
                VMEditUser regViewModel = new VMEditUser { roles = _GetRoleNameByUserId(id) };
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var user = await manager.FindByIdAsync(id);
                if (user != null)
                {
                    ViewBag.RoleName = new SelectList(_GetRoleList(), "Name", "Name");
                    ViewBag.UnionId = new SelectList(_unionParishadService.GetAllForDropdown(), "Id", "Name", user.UnionId);

                    regViewModel.UserId = user.Id;
                    regViewModel.UserName = user.UserName;
                    regViewModel.Email = user.Email;
                    regViewModel.ContactNo = user.PhoneNumber;
                    regViewModel.IsActive = user.IsActive;
                    regViewModel.Name = user.FullName;
                    regViewModel.UnionId = user.UnionId;
                    regViewModel.Password = user.PasswordHash;
                    regViewModel.ConfirmPassword = user.PasswordHash;
                }
                else
                {
                    _message.custom(this, "কোন ইউজার পাওয়া যায়নি।");
                    return RedirectToAction("Index");
                }
                regViewModel.ConfirmPassword = Password;
                regViewModel.Password = Password;
                List<SelectListItem> RoleSelectedList = new List<SelectListItem>();
                var RoleList = _GetRoleList();
                if (regViewModel.roles == null)
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
                }
                else
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = regViewModel.roles.Contains(c.Name.ToString()) ? true : false }).ToList();
                }
                ViewBag.RoleName = RoleSelectedList;
                return View(regViewModel);
            }
            catch (Exception ex)
            {
                _message.custom(this, ex.ToString());
                return RedirectToAction("Index");
            }
        }


        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VMEditUser regViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int error = 0;
                    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                    manager.UserValidator = new UserValidator<ApplicationUser>(manager) { AllowOnlyAlphanumericUserNames = false };
                    regViewModel.UserName = regViewModel.UserName.Trim();
                    regViewModel.Password = regViewModel.Password.Trim();

                    var re = from u in db.Users
                             where u.UserName == regViewModel.UserName && u.Id != regViewModel.UserId
                             select u.Id;
                    if (re.Count() > 0)
                    {
                        _message.custom(this, "ইউজারনেম ইতিমধ্যে বিদ্যমান. দয়া করে একটি আলাদা ইউজারনেম ব্যবহার করুন।");
                        error++;
                    }

                    var user = await manager.FindByIdAsync(regViewModel.UserId);
                    if (user != null && error == 0)
                    {
                        if (user != null)
                        {
                            user.UserName = regViewModel.UserName;

                            if (regViewModel.Password != null && regViewModel.ConfirmPassword != null && regViewModel.Password != "" && regViewModel.ConfirmPassword != "")
                            {
                                if (regViewModel.Password != Password)
                                {
                                    user.PasswordHash = manager.PasswordHasher.HashPassword(regViewModel.Password);
                                }

                                user.Email = regViewModel.Email;
                                user.PhoneNumber = regViewModel.ContactNo;
                                user.IsActive = regViewModel.IsActive;
                                user.FullName = regViewModel.Name;
                                user.UnionId = regViewModel.UnionId;
                                //user.PasswordHash = manager.PasswordHasher.HashPassword(regViewModel.Password);
                            }
                        }
                        var rolesForUser = await manager.GetRolesAsync(user.Id);
                        try
                        {
                            using (var transaction = db.Database.BeginTransaction())
                            {
                                if (rolesForUser.Count() > 0)
                                {
                                    foreach (var item in rolesForUser.ToList())
                                    {
                                        // item should be the name of the role
                                        var RemoveFromRole = await manager.RemoveFromRoleAsync(user.Id, item);
                                    }
                                }
                                bool success = true;
                                var isSuccess = manager.AddToRole(user.Id, regViewModel.RoleName);
                                if (!isSuccess.Succeeded)
                                {
                                    success = false;
                                }
                                if (success)
                                {
                                    var resultUpdate = await manager.UpdateAsync(user);

                                    if (resultUpdate.Succeeded)
                                    {
                                        db.SaveChanges();
                                        transaction.Commit();
                                        _message.update(this);
                                        return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                                    }
                                    else
                                    {
                                        _message.custom(this, "ইউজার আপডেট করতে সমস্যা হয়েছে!");
                                    }
                                }
                                else
                                {
                                    _message.custom(this, "ইউজার আপডেট করতে সমস্যা হয়েছে!");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var message = ex.Message;
                            _message.custom(this, "ইউজার আপডেট করতে সমস্যা হয়েছে!");
                        }

                    }
                }

                regViewModel.roles = _GetRoleNameByUserId(regViewModel.UserId);
                List<SelectListItem> RoleSelectedList = new List<SelectListItem>();
                var RoleList = _GetRoleList();
                if (regViewModel.roles == null)
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
                }
                else
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = regViewModel.roles.Contains(c.Name.ToString()) ? true : false }).ToList();
                }
                ViewBag.RoleIdList = RoleSelectedList;

                return View(regViewModel);
            }
            catch (Exception ex)
            {
                _message.custom(this, ex.ToString());
                regViewModel.roles = _GetRoleNameByUserId(regViewModel.UserId);
                List<SelectListItem> RoleSelectedList = new List<SelectListItem>();
                var RoleList = _GetRoleList();
                if (regViewModel.roles == null)
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
                }
                else
                {
                    RoleSelectedList = RoleList.Select(c => new SelectListItem { Text = c.Name, Value = c.Name.ToString(), Selected = regViewModel.roles.Contains(c.Name.ToString()) ? true : false }).ToList();
                }
                ViewBag.RoleIdList = RoleSelectedList;

                return View(regViewModel);
            }
        }
        #endregion

        #region Get all Role
        private IEnumerable<IdentityRole> _GetRoleList()
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            var roles = roleMngr.Roles.ToList();
            return roles;
        }
        private List<string> _GetRoleNameByUserId(string Id)
        {
            List<string> roleList = new List<string>();
            string query = @"select r.Name from Roles r
                            inner join UserRoles ur on r.Id = ur.RoleId
                            inner join Users u on ur.UserId = u.Id where ur.UserId = '" + Id + "'";

            roleList = db.Database.SqlQuery<string>(query).ToList();
            return roleList;
        }
        #endregion

        #region Delete
        public async Task<ActionResult> Delete(string id)
        {
            db = new AdminContext();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            manager.UserValidator = new UserValidator<ApplicationUser>(manager) { AllowOnlyAlphanumericUserNames = false };

            var user = await manager.FindByIdAsync(id);

            var logins = user.Logins.ToList();
            var rolesForUser = await manager.GetRolesAsync(id);

            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {

                    foreach (var login in logins)
                    {
                        await manager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                    }

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await manager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    var delResult = await manager.DeleteAsync(user);

                    db.SaveChanges();

                    transaction.Commit();
                }

                _message.delete(this);
            }
            catch (Exception ex)
            {
                _message.custom(this, "ইউজার মুছে ফেলতে সমস্যা হয়েছে!");
            }

            return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
        }
        #endregion

    }
}