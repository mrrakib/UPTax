using System.Web.Mvc;
using UPTax.Filter;

namespace UPTax.Controllers
{
    public class HouseOwnerController : Controller
    {
        // GET: HouseOwner
        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }
    }
}