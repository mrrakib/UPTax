using System.Web.Mvc;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class TopSheetController : Controller
    {
        private readonly IFinancialYearService _financialYearService;
        private readonly ITaxInstallmentService _taxInstallmentService;

        public TopSheetController(IFinancialYearService financialYearService, ITaxInstallmentService taxInstallmentService)
        {
            _financialYearService = financialYearService;
            _taxInstallmentService = taxInstallmentService;
        }

        // GET: TopSheet/Index
        public ActionResult Index()
        {
            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string financialYear = "")
        {
            var data = _taxInstallmentService.GetTopSheetReport(financialYear);

            ViewBag.FinancialYear = new SelectList(_financialYearService.GetAllForDropdown(), "Id", "Name", financialYear);
            return View(data);
        }

        [HttpGet]
        public ActionResult Export(string financialYearId = "")
        {
            var data = _taxInstallmentService.GetTopSheetReport(financialYearId);

            return View(data);
        }
    }
}
