using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Carsport.Backend.Startup))]
namespace Carsport.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
