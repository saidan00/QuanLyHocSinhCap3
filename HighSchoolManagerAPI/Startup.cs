using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HighSchoolManagerAPI.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HighSchoolManagerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
                    {
                        options.AddPolicy(MyAllowSpecificOrigins,
                        builder =>
                        {
                            builder.WithOrigins("http://127.0.0.1:3000",
                                                "http://localhost:3000")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials();
                        });
                    });

            // change context to match with your database
            services.AddDbContext<HighSchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HuyContext")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<HighSchoolContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/api/Account/AccessDenied";
                options.LoginPath = "/api/Account/NotLoggedIn";
                //     ReturnUrlParameter requires 
                //     using Microsoft.AspNetCore.Authentication.Cookies;
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
