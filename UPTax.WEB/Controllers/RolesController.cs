using System.Web.Mvc;
using UPTax.Data.Infrastructure;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.Account;
using UPTax.Service.Services.Permissions;

namespace UPTax.Controllers
{
    public class RolesController : Controller
    {
        #region Global Variables
        private readonly IRoleService _roleService;
        private Message _message = new Message();
        #endregion

        #region constructor
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;
            return View(_roleService.GetPageList(new Page(page, dataSize), name));
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
        public ActionResult Create(ApplicationRole model)
        {
            bool isError = false;
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                if (!_roleService.CheckIfExist(model.Name))
                {
                    isError = !(_roleService.Add(model));
                }
                else
                {
                    isError = true;
                    msg = "এই নামে একটি রোল আছে!";
                }
                    
                if (!isError)
                {
                    _message.save(this);
                    return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                }
                else
                {
                    _message.custom(this, msg);
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ApplicationRole model = _roleService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }

            if (model.Name.Equals("Super Admin"))
            {
                _message.custom(this, "এই রোল হালনাগাদযোগ্য নয়!");
                return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
            }

            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationRole model)
        {
            bool isError = false;
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                if (!_roleService.CheckIfExistForUpdate(model.Name, model.Id))
                {
                    _roleService.Update(model);
                }
                else
                {
                    isError = true;
                    msg = "এই নামে একটি রোল আছে!";
                }
            }

            if (!isError)
            {
                _message.update(this);
                return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                
            }
            else
            {
                _message.custom(this, msg);
                return View(model);
            }
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(string id)
        {
            if (_roleService.Delete(id))
            {  
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion

    }
}