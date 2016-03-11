using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebServer.Startup))]
namespace WebServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
