using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebBasketDev.Startup))]
namespace WebBasketDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
