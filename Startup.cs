using Hangfire;
using Hangfire.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PanOpticon.Data;
using PanOpticon.Email;
using PanOpticon.Notification;
using PanOpticon.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PanOpticon
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // ok
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IISServerOptions>(
                options => options.MaxRequestBodySize = int.MaxValue);
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<PanopticonUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SMSSender>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddHangfire(
                s => s.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection"))
               
                );
            services.AddHangfireServer(options=>options.WorkerCount = 100);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/dash");

            app.UseEndpoints(endpoints =>
            { 
                //endpoints.MapControllerRoute
                //(
                //    name: "calendar",
                //    pattern: "cal/{action=Index}/{id?}",
                //    defaults: new {controller = "Calendar", action = "Index"}
                //    ) ;
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
