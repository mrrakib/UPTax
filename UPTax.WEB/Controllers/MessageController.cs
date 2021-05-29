using System;
using System.Linq;
using System.Web.Mvc;
using UpTax.Utilities.Enum;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
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
        private readonly IMessageReplyService _messageReplyService;

        public MessageController(IMessageInfoService messageInfoService,
            IUnionParishadService unionParishadService,
            IUserService userService,
            IMessageReplyService messageReplyService)
        {
            _messageInfoService = messageInfoService;
            _unionParishadService = unionParishadService;
            _userService = userService;
            _messageReplyService = messageReplyService;
        }

        // GET: Message/Index
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name?.Trim();
            ViewBag.UserId = _userId;
            if (_roleName == "Super Admin")
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
            ViewBag.IsUser = _roleName == RoleEnum.Admin.ToString() ? true : false;
            if (ViewBag.IsUser)
            {
                ViewBag.ToSupperAdminUserId = new SelectList(_messageInfoService.GetSuperAdminDropDownList(), "IdStr", "Name");
            }
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
                if (_roleName == RoleEnum.Admin.ToString())
                {
                    model.ToAdminUserId = _userId;
                }
                else
                {
                    model.ToSupperAdminUserId = _userId;
                }
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
            ViewBag.UnionId = new SelectList(union, "Id", "Name", model.UnionId);
            ViewBag.IsUser = _roleName == RoleEnum.Admin.ToString() ? true : false;
            if (ViewBag.IsUser)
            {
                ViewBag.ToSupperAdminUserId = new SelectList(_messageInfoService.GetSuperAdminDropDownList(), "IdStr", "Name", model.ToSupperAdminUserId);
            }
            return View(model);
        }

        // GET: Message/Inbox
        public ActionResult Inbox()
        {
            return View();
        }

        // GET: Message/Reply
        public ActionResult Reply(int id = 0)
        {
            var message = _messageInfoService.GetDetails(id);
            if (message == null)
            {
                return View("_Error");
            }
            var replies = _messageReplyService.GetAllByMessageId(id).ToList();

            var messageDetails = new VMMessageInfo()
            {
                Id = message.Id,
                MessageReply = replies,
                Message = message.Message
            };
            return View(messageDetails);
        }

        [HttpPost]
        public ActionResult Reply(VMMessageInfo model)
        {
            var reply = new MessageReply() { MessageInfoId = model.Id, ReplyerUserId = _userId, ReplyMessage = model.ReplyMessage, CreatedDate = DateTime.Now };
            var replied = _messageReplyService.Add(reply);
            if (replied)
                return RedirectToAction("Reply", "Message", new { id = model.Id });

            var replies = _messageReplyService.GetAllByMessageId(model.Id).ToList();
            model.MessageReply = replies;
            return View(model);
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
            ViewBag.IsUser = _roleName == "Admin" ? true : false;
            if (ViewBag.IsUser)
            {
                ViewBag.ToSupperAdminUserId = new SelectList(_messageInfoService.GetSuperAdminDropDownList(), "IdStr", "Name", model.ToSupperAdminUserId);
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
            ViewBag.IsUser = _roleName == "Admin" ? true : false;
            if (ViewBag.IsUser)
            {
                ViewBag.ToSupperAdminUserId = new SelectList(_messageInfoService.GetSuperAdminDropDownList(), "IdStr", "Name", model.ToSupperAdminUserId);
            }

            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name", model.UnionId);
            return View(model);
        }
        #endregion
    }
}
