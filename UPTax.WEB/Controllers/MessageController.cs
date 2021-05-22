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
        public ActionResult Index()
        {
            ViewBag.IsAdmin = _roleName == "Admin" ? true : false;

            var data = _messageInfoService.GetAll();
            return View();
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
