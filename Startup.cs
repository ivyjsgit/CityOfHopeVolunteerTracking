using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CoHO
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsAdmin", policy => policy.RequireClaim("super"));
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/ManageTables", "IsAdmin");
                    options.Conventions.AuthorizePage("/AdminIndex", "IsAdmin");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/ResetPassword", "IsAdmin");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Register", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/Disabilities", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/EducationLevels", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/Initiatives", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/Races", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/Skills", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/ValueOfHours", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/VolunteerActivities", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/Volunteers", "IsAdmin");
                    options.Conventions.AuthorizeFolder("/VolunteerTypes", "IsAdmin");

                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });


            ApplicationDbInitializer.SeedUsers(userManager);
        }
    }
}
