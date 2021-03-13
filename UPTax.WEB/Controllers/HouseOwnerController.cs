using System.Web.Mvc;
using UPTax.Filter;
using UPTax.Helper;
using UPTax.Service.Services;
using UPTax.Service.Services.UPDetails;

namespace UPTax.Controllers
{
    public class HouseOwnerController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IHouseOwnerService _houseOwnerService;
        private readonly IWardInfoService _wardInfoService;
        private readonly IVillageInfoService _villageInfoService;
        private readonly IProfessionInfoService _professionInfoService;
        private readonly ISocialBenefitService _socialBenefitService;

        public HouseOwnerController(IHouseOwnerService houseOwnerService, IWardInfoService wardInfoService, IVillageInfoService villageInfoService, IProfessionInfoService professionInfoService, ISocialBenefitService socialBenefitService)
        {
            _houseOwnerService = houseOwnerService;
            _wardInfoService = wardInfoService;
            _villageInfoService = villageInfoService;
            _professionInfoService = professionInfoService;
            _socialBenefitService = socialBenefitService;
        }
        // GET: HouseOwner
        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }
        // GET: HouseOwner/Create
        [RapidAuthorization]
        public ActionResult Create()
        {
            ViewBag.WardInfoId = new SelectList(_wardInfoService.GetDropdownItemList(_unionId), "Id", "Name");

            return View();
        }
    }
}