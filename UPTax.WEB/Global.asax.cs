using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UPTax.App_Start;
using UPTax.Utility;

namespace UPTax.WEB
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
