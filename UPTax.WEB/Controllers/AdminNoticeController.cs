using System;
using System.Linq;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
using UPTax.Service.Services;
using UPTax.Service.Services.Autofac;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class AdminNoticeController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly string _roleName = RapidSession.RoleName;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IAdminNoticeService _adminNoticeService;
        private readonly IUnionParishadService _unionParishadService;
        private readonly IUserService _userService;

        public AdminNoticeController(IAdminNoticeService adminNoticeService,
            IUnionParishadService unionParishadService,
            IUserService userService)
        {
            _adminNoticeService = adminNoticeService;
            _unionParishadService = unionParishadService;
            _userService = userService;
        }

        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();

            var data = _adminNoticeService.GetPagedList(name, page, dataSize);
            return View(data);

        }
        // GET: Message/Create
        [RapidAuthorization]
        public ActionResult Create()
        {
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name");
            return View();
        }

        // GET: Message/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RapidAuthorization]
        public ActionResult Create(AdminNotice model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = _userId;
                var created = _adminNoticeService.Add(model);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index", "Message");
                }
                else
                {
                    _message.custom(this, "পরে আবার চেষ্টা করুন");
                }
            }
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name");
            return View(model);
        }
        [RapidAuthorization(All = true)]
        public ActionResult GetAdminOrUser(int unionId = 0)
        {
            var model = _unionParishadService.GetAdminOrUserByUnionId(unionId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #region Update
        [HttpGet]
        [RapidAuthorization]
        public ActionResult Edit(int id)
        {
            AdminNotice model = _adminNoticeService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name", model.UnionId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RapidAuthorization]
        public ActionResult Edit(AdminNotice model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _adminNoticeService.Update(model);
                _message.update(this);
                return RedirectToAction("Index", "Message");
            }
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name", model.UnionId);
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_adminNoticeService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}
