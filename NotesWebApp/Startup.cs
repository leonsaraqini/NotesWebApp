using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotesWebApp.Startup))]
namespace NotesWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
