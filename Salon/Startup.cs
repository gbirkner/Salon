using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Salon.Startup))]
namespace Salon
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
