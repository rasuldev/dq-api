using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DrinqWeb.Startup))]
namespace DrinqWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
