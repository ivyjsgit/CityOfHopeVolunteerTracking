using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using CoHO.Data;
using FluffySpoon.AspNet.LetsEncrypt;
using FluffySpoon.AspNet.LetsEncrypt.Certes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.AddFluffySpoonLetsEncryptRenewalService(new LetsEncryptOptions()
            {
                Email = "ivyjshdx@gmail.com", //LetsEncrypt will send you an e-mail here when the certificate is about to expire
                UseStaging = false, //switch to true for testing
                Domains = new[] { "" },//Enter your Domain Here
                TimeUntilExpiryBeforeRenewal = TimeSpan.FromDays(30), //renew automatically 30 days before expiry
                TimeAfterIssueDateBeforeRenewal = TimeSpan.FromDays(7), //renew automatically 7 days after the last certificate was issued
                CertificateSigningRequest = new CsrInfo() //these are your certificate details
                {
                    CountryName = "United States",
                    Locality = "US",
                    Organization = "City of Hope"
                }
            });

            //the following line tells the library to persist the certificate to a file, so that if the server restarts, the certificate can be re-used without generating a new one.
            services.AddFluffySpoonLetsEncryptFileCertificatePersistence();

//the following line tells the library to persist challenges in-memory. challenges are the "/.well-known" URL codes that LetsEncrypt will call.
            services.AddFluffySpoonLetsEncryptMemoryChallengePersistence();
            

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager,ILoggerFactory loggerFactory)
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
            loggerFactory.AddFile("Logs/CoHo-{Date}.txt");
            // app.UseHttpsRedirection();
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
		app.UseFluffySpoonLetsEncryptChallengeApprovalMiddleware();


            ApplicationDbInitializer.SeedUsers(userManager);
        }
    }
}
