using System.Web.Mvc;

namespace UPTax.Controllers
{
    public class MembersController : Controller
    {
        public MembersController()
        {

        }
        // GET: Members
        public ActionResult Index()
        {
            return View();
        }
    }
}