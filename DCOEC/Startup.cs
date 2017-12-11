using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DCOEC.Data;
using DCOEC.Models;
using DCOEC.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DCOEC
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
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            // enable dependency injection for context of OEC database
            // loosly couple context to the connection string
            services.AddDbContext<OECContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("OECConnection")));



            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();


            //services.AddCaching(); 
            //services.AddSession();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();



            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                // ref: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?tabs=aspnetcore2x

                options.Cookie.Name = ".OECFertilizers.Session";
                //options.IdleTimeout = TimeSpan.FromSeconds(5); //Clear session in 5 secs for degugging
                options.Cookie.HttpOnly = true;
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
        //init Session here
            app.UseSession();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //app.UseFileServer();
            //app.UseExceptionHandler("/error.html");

            app.UseAuthentication();

            app.UseMvc(routes =>
            {

            routes.MapRoute(
                name: "Default3Para",
                template: "{controller=Home}/{action=Index}/{id?}");


           //routes.MapRoute(
           //   name: "Default2Para",
           //   template: "{action=Index}/{id?}",
           //   defaults: new
           //   {
           //       controller = "DCCrops"
           //   });

            });


           
        }
    }
}
