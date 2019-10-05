using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HighSchoolManagerAPI.Data;
using HighSchoolManagerAPI.Models;

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

            // services.Configure<CookiePolicyOptions>(options =>
            //     {
            //         options.CheckConsentNeeded = context => true;
            //         options.MinimumSameSitePolicy = SameSiteMode.None;
            //     });

            services.AddDbContext<HighSchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HoangConn")));

            // services.AddDefaultIdentity<Account>()
            //     .AddEntityFrameworkStores<HighSchoolContext>()
            //     .AddDefaultTokenProviders();
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<HighSchoolContext>();

            // services.AddIdentityServer()
            //     .AddApiAuthorization<IdentityUser, HighSchoolContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            // services.AddControllers();
            services.AddControllers()
                    .AddNewtonsoftJson();
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

            // app.UseIdentityServer();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
