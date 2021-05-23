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
            var users = _userService.GetAllForDropdown();
            ViewBag.ToAdminUserId = new SelectList(users, "IdStr", "Name");
            return View(new MessageInfo());
        }

        // GET: Message/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageInfo message)
        {
            if (ModelState.IsValid)
            {
                message.CreatedBy = _userId;
                message.ToSupperAdminUserId = _userId;
                var created = _messageInfoService.Add(message);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                else
                {
                    _message.custom(this, "পরে আবার চেষ্টা করুন");
                }
            }
            var union = _unionParishadService.GetAllForDropdown();
            ViewBag.UnionId = new SelectList(union, "Id", "Name");
            var users = _userService.GetAllForDropdown();
            ViewBag.ToAdminUserId = new SelectList(users, "IdStr", "Name");
            return View(message);
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
    }
}
