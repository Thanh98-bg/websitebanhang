using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAnI.Startup))]
namespace DoAnI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
