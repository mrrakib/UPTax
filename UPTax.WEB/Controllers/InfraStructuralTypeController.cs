using System;
using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Model.Models;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class InfraStructuralTypeController : Controller
    {
        #region Global Variables
        private Message _message = new Message();
        private readonly int _unionId = RapidSession.UnionId;

        private readonly IInfraStructuralTypeService _infraStructuralTypeService;
        #endregion

        #region constructor
        public InfraStructuralTypeController(IInfraStructuralTypeService infraStructuralTypeService)
        {
            _infraStructuralTypeService = infraStructuralTypeService;
        }
        #endregion

        [RapidAuthorization]
        public ActionResult Index(string name, int page = 1, int dataSize = 10)
        {
            ViewBag.dataSize = dataSize;
            ViewBag.page = page;
            ViewBag.name = name;

            var unionList = _infraStructuralTypeService.GetPagedList(name, page, dataSize);
            return View(unionList);
        }

        #region Create
        [RapidAuthorization]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [RapidAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InfraStructuralType model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = RapidSession.UserId;
                var existingItem = _infraStructuralTypeService.IsExistingItem(model.TypeName, null);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি অবকাঠামোর ধরন আছে!");
                    return View(model);
                }
                model.TypeName = model.TypeName.Trim();
                if (_infraStructuralTypeService.Add(model))
                {
                    _message.save(this);
                }
                else
                {
                    _message.custom(this, "অবকাঠামোর ধরন যোগ করতে সমস্যা হয়েছে!");
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
            InfraStructuralType model = _infraStructuralTypeService.GetDetails(id);
            if (model == null)
            {
                return PartialView("_Error");
            }
            return View(model);
        }

        [RapidAuthorization]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InfraStructuralType model)
        {
            if (ModelState.IsValid)
            {
                model.UpdatedBy = RapidSession.UserId;
                model.UpdatedDate = DateTime.Now;
                var existingItem = _infraStructuralTypeService.IsExistingItem(model.TypeName, model.Id);
                if (existingItem)
                {
                    _message.custom(this, "এই নামে একটি অবকাঠামোর ধরন আছে!");
                    return View(model);
                }
                model.TypeName = model.TypeName.Trim();
                if (_infraStructuralTypeService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index", new { page = TempData["page"] ?? 1, size = TempData["size"] ?? 10 });
                }
                else
                {
                    _message.custom(this, "অবকাঠামোর ধরন হালনাগাদ করতে সমস্যা হয়েছে!");
                }


            }
            return View(model);
        }
        #endregion

        #region Delete
        [RapidAuthorization]
        public ActionResult Delete(int id)
        {
            if (_infraStructuralTypeService.Delete(id))
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            return PartialView("_Error");
        }
        #endregion

        public ActionResult Rent_Interest(int building = 0, int semiBuilding = 0, int rawHouse = 0)
        {
            var rawHouseInfo = _infraStructuralTypeService.GetByStaticId(1);
            var buildingInfo = _infraStructuralTypeService.GetByStaticId(2);
            var semiBuildingInfo = _infraStructuralTypeService.GetByStaticId(3);

            double rentAmount = building * buildingInfo.YearlyRent;
            rentAmount += semiBuilding * semiBuildingInfo.YearlyRent;
            rentAmount += rawHouse * rawHouseInfo.YearlyRent;

            double rateValue = 0;
            int executeValue = 0;
            if (building > 0)
            {
                rateValue += buildingInfo.InterestRate;
                executeValue++;
            }
            if (semiBuilding > 0)
            {
                rateValue += semiBuildingInfo.InterestRate;
                executeValue++;
            }
            if (rawHouse > 0)
            {
                rateValue += rawHouseInfo.InterestRate;
                executeValue++;
            }

            rateValue = rateValue > 0 ? rateValue / executeValue : 0;

            return Json(new { rent = rentAmount, rate = rateValue }, JsonRequestBehavior.AllowGet);
        }
    }
}