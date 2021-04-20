using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPTax.Filter;

namespace UPTax.Controllers
{
    public class RPTTaxCollectionSingleController : Controller
    {
        // GET: RPTTaxCollectionList
        [RapidAuthorization]
        public ActionResult Index()
        {
            return View();
        }

        [RapidAuthorization]
        [HttpPost]
        public ActionResult Index(string HoldingNo)
        {
            return View();
        }

    }
}