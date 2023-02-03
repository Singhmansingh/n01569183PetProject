using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(n01569183PetProject.Startup))]
namespace n01569183PetProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
