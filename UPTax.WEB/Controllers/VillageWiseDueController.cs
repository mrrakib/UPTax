using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;
using Message = UPTax.Helper.Message;

namespace UPTax.Controllers
{
    public class VillageWiseDueController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;
        private readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;
        private readonly IInfraStructuralTypeService _infraStructuralTypeService;

        public VillageWiseDueController(IFinancialYearService financialYearService,
            ITaxInstallmentService taxInstallmentService,
            IWardInfoService wardInfoService,
            IVillageInfoService villageInfoService,
            IInfraStructuralTypeService infraStructuralTypeService)
        {
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
            _infraStructuralTypeService = infraStructuralTypeService;
        }

        // GET: VillageWiseDue/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.VillageId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name");
            ViewBag.InfrastructureType = new SelectList(_infraStructuralTypeService.GetAllForDropdown(), "IdStr", "Name");
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int wardId, int villageId, string infrastructureType, int financialYearId)
        {
            var data = _wardInfoService.GetWardVillageWiseReport(wardId, villageId, infrastructureType, financialYearId);

            ViewBag.WardId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name", wardId);
            ViewBag.VillageId = new SelectList(_villageInfoService.GetDropdownItemList(_unionId), "Id", "Name", villageId);
            ViewBag.InfrastructureType = new SelectList(_infraStructuralTypeService.GetAllForDropdown(), "IdStr", "Name", infrastructureType);
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", financialYearId);
            return View();
        }
    }
}
