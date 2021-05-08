using System.Web.Mvc;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class VillageWiseDueController : Controller
    {
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;

        public VillageWiseDueController(IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
        {
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
        }

        // GET: VillageWiseDue/Index
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.WardId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.VillageId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.InfrastructureType = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.FinancialYearId = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string wardId, string villageId, string infrastructureType, string financialYearId)
        {
            ViewBag.Wards = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.Villages = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.InfrastructureType = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }
    }
}
