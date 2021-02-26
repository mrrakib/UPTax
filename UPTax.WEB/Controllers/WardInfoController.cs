using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models.UnionDetails;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class WardInfoController : Controller
    {
        private readonly Message _message = new Message();
        private readonly IWardInfoService _wardInfoService;

        public WardInfoController(IWardInfoService wardInfoService)
        {
            _wardInfoService = wardInfoService;
        }
        // GET: WardInfo
        [RapidAuthorization(All = true)]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _wardInfoService.GetPagedList(wardNo: name, page, dataSize);
            return View(unionList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WardInfo model)
        {
            if (ModelState.IsValid)
            {
                if (_wardInfoService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        #region Update
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            WardInfo model = _wardInfoService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WardInfo model)
        {
            if (ModelState.IsValid)
            {
                _wardInfoService.Update(model);
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
            if (_wardInfoService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion
    }
}