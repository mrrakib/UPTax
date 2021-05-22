using System.Web.Mvc;
using UPTax.Helper;

namespace UPTax.Controllers
{
    public class MessageController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        public MessageController()
        {

        }
        // GET: Message
        public ActionResult Index()
        {
            return View();
        }

        // GET: Message/Inbox
        public ActionResult Inbox()
        {
            return View();
        }
    }
}
