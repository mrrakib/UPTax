using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.UnionDetails;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class UnionParishadController : Controller
    {
        #region Global Variables
        private Message _message = new Message();

        private readonly IUnionParishadService _unionParishadService;
        #endregion

        #region constructor
        public UnionParishadController(IUnionParishadService unionParishadService)
        {
            _unionParishadService = unionParishadService;
        }
        #endregion

        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _unionParishadService.GetPaged(name, page, dataSize);
            return View(unionList);
        }

        #region Add
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UnionParishad model)
        {
            if (ModelState.IsValid)
            {
                if (_unionParishadService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        #endregion

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            UnionParishad model = _unionParishadService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UnionParishad model)
        {
            if (ModelState.IsValid)
            {
                _unionParishadService.Update(model);
                _message.update(this);
                return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
            }
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_unionParishadService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}