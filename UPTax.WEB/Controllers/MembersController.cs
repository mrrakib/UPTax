using System.Web.Mvc;
using UPTax.Helper;
using UPTax.Service.Services;

namespace UPTax.Controllers
{
    public class MembersController : Controller
    {
        private readonly Message _message = new Message();
        private readonly string _userId = RapidSession.UserId;
        private readonly int _unionId = RapidSession.UnionId;
        private readonly IMemberService _memberService;
        private readonly IRelationshipService _relationshipService;
        private readonly IEducationInfoService _educationInfoService;
        private readonly IProfessionInfoService _professionInfoService;
        private readonly ISocialBenefitService _socialBenefitService;
        private readonly IGenderService _genderService;

        public MembersController(IMemberService memberService,
            IRelationshipService relationshipService,
            IEducationInfoService educationInfoService,
            IProfessionInfoService professionInfoService,
            ISocialBenefitService socialBenefitService,
            IGenderService genderService

            )
        {
            _memberService = memberService;
            _relationshipService = relationshipService;
            _educationInfoService = educationInfoService;
            _professionInfoService = professionInfoService;
            _socialBenefitService = socialBenefitService;
            _genderService = genderService;
        }

        // GET: Members
        public ActionResult Index()
        {
            return View();
        }
    }
}