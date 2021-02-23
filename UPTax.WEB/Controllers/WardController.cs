using System.Web.Mvc;
using System.Web.Services.Description;
using UPTax.Filter;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class WardController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly IUnionParishadService _unionParishadService;

        #endregion

        #region constructor
        public WardController(IUnionParishadService unionParishadService)
        {
            _unionParishadService = unionParishadService;
        }
        #endregion

        // GET: Ward
        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _unionParishadService.GetPaged(name, page, dataSize);
            return View(unionList);
        }
    }
}