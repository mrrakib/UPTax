using UPTax.App_Start;
using UPTax.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace UPTax
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.RegisterComponents();
            ModelBinders.Binders.DefaultBinder = new TrimModelBinder();
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            //GlobalFilters.Filters.Add(new EncryptedActionParameterAttribute());
        }
    }
}
