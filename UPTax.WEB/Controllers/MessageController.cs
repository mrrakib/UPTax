using System;
using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;
using UPTax.Service.Services.Autofac;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class MessageController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly string _roleName = RapidSession.RoleName;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IMessageInfoService _messageInfoService;
        private readonly IUnionParishadService _unionParishadService;
        private readonly IUserService _userService;

        public MessageController(IMessageInfoService messageInfoService, IUnionParishadService unionParishadService, IUserService userService)
        {
            _messageInfoService = messageInfoService;
            _unionParishadService = unionParishadService;
            _userService = userService;
        }

        // GET: Message
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();
            ViewBag.IsSupperAdmin = _roleName == "Super Admin" ? true : false;
            if (ViewBag.IsSupperAdmin)
            {
                var data = _messageInfoService.GetPagedList(name, page, dataSize);
                return View(data);
            }
            else
            {
                var data = _messageInfoService.GetPagedList(adminUserId: _userId, name, page, dataSize);
                return View(data);
            }
        }
        // GET: Message/Create
        public ActionResult Create()
        {
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name");
            return View(new MessageInfo());
        }

        // GET: Message/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageInfo model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = _userId;
                model.ToSupperAdminUserId = _userId;
                var created = _messageInfoService.Add(model);
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

        // GET: Message/Inbox
        public ActionResult Inbox()
        {
            return View();
        }

        public ActionResult GetAdminOrUser(int unionId = 0)
        {
            var model = _unionParishadService.GetAdminOrUserByUnionId(unionId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #region Update
        [HttpGet]
        public ActionResult Edit(int id)
        {
            MessageInfo model = _messageInfoService.GetDetails(id);
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
        public ActionResult Edit(MessageInfo model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = _userId;
                model.UpdatedDate = DateTime.UtcNow;
                _messageInfoService.Update(model);
                _message.update(this);
                return RedirectToAction("Index", "Message");
            }
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name", model.UnionId);
            return View(model);
        }
        #endregion
    }
}
