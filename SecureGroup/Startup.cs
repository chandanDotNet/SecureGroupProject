using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SecureGroup.Controllers;
using SecureGroup.DBContexts;
//using SecureGroup.Concrete;
//using SecureGroup.Interface;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureGroup
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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            // services.AddSession(); //Add this line to register the session service
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(50);
            });

            services.AddMvc();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<MsDBContext>(option => option.UseSqlServer(connectionString));
            //  services.AddSingleton<IHR, HRConcrete>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
                    //pattern: "{controller=Order}/{action=Abc}/{id?}");
            });
        }
    }
}
