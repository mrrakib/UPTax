using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class MessageController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly string _roleName = RapidSession.RoleName;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IMessageInfoService _messageInfoService;

        public MessageController(IMessageInfoService messageInfoService)
        {
            _messageInfoService = messageInfoService;
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

        // GET: Message/Inbox
        public ActionResult Inbox()
        {
            return View();
        }
        // GET: Message/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: Message/Create
        [HttpPost]
        public ActionResult Create(MessageInfo message)
        {
            return View();
        }
    }
}
