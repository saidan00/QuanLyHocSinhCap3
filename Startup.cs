using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyHocSinhCap3.Startup))]
namespace QuanLyHocSinhCap3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
