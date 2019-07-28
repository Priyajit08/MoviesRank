using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieRank.Startup))]
namespace MovieRank
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
