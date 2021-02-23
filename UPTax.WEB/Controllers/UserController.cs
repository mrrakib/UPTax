using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

namespace UPTax.Controllers
{
    public class UserController : Controller
    {
        #region Global Variables
        private readonly AdminContext db = new AdminContext();
        private UserStore<ApplicationUser> store;
        private UserManager<ApplicationUser> UserManager;
        private Message message = new Message();

        private readonly IUserService _userService;
        #endregion

        #region constructor
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        // GET: User Rakib
        //[HttpGet]
        [RapidAuthorization(All = true)]
        public ActionResult Index(int pageNo = 1, int dataSize = 10)
        {
            //List<string> rolesList = ((ClaimsIdentity)System.Web.HttpContext.Current.User.Identity).Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value).ToList();

            ViewBag.dataSize = dataSize;
            ViewBag.pageNo = pageNo;

            var userList = _userService.GetUserPaged(pageNo, dataSize);
            return View(userList);
        }
        #region Register GET
        [HttpGet]
        [RapidAuthorization]
        public ActionResult Register()
        {
            VMRegister vmRegister = new VMRegister { roles = _GetRoleList() };
            return View(vmRegister);
        }
        #endregion

        #region Register Post
        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(VMRegister model)
        {

            string errorMsg = "";

            if (ModelState.IsValid)
            {
                //db = new EasyContext();
                this.store = new UserStore<ApplicationUser>(db);
                this.UserManager = new UserManager<ApplicationUser>(this.store);

                this.UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

                //check for username that already exist 

                var re = from u in db.Users
                         where u.UserName == model.UserName
                         select u.Id;

                if (re.Count() > 0)
                {
                    // already exist
                    message.custom(this, "This username already exist. Please choose a new one.");


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
                        message.custom(this, "Problem occured while creating user role!");
                        RedirectToAction("Login");
                    }

                    var isSuccess = UserManager.AddToRole(user.Id, model.RoleName);
                    if (isSuccess.Succeeded)
                    {

                        ApplicationUser userDetails = (from u in db.Users where u.Id == user.Id select u).FirstOrDefault(); //_userService.GetUser(user.Id);
                        //userDetails.PlainPassword = model.Password;
                        //userDetails.Sha1Password = HashingUtility.GetSha1HashString(model.Password);
                        //userDetails.Md5Password = HashingUtility.GetMD5HashString(model.Password);
                        userDetails.EmployeeId = model.EmployeeId;
                        //userDetails.IsLocalPermitedUser = model.IsLocalPermitedUser;
                        db.SaveChanges();
                        //_userService.SaveUser();


                        message.success(this, "Registered successfully");



                        return RedirectToAction("Index");
                    }
                    else
                    {
                        message.custom(this, "Problem has occured while creating user role!");
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
            message.custom(this, "Registration not complete due to some problem!" + "<br> " + errorMsg);
            model.roles = _GetRoleList();
            return View(model);
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
        #endregion

    }
}