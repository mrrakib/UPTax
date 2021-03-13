using System.Web.Mvc;
using UPTax.Data.Repository;
using UPTax.Filter;
using UPTax.Helper;

namespace UPTax.Controllers
{
    public class HouseOwnerController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IHouseOwnerRepository _houseOwnerRepository;

        public HouseOwnerController(IHouseOwnerRepository houseOwnerRepository)
        {
            _houseOwnerRepository = houseOwnerRepository;
        }
        // GET: HouseOwner
        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }
    }
}