using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductBacklogForProjects.Startup))]
namespace ProductBacklogForProjects
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
