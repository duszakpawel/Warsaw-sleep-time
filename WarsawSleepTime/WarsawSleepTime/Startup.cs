using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WarsawSleepTime.Startup))]
namespace WarsawSleepTime
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
