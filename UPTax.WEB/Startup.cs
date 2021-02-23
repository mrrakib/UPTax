using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(UPTax.Startup))]
namespace UPTax
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
