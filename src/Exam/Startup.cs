using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Exam
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                    options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            

            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddDefaultUI(UIFramework.Bootstrap4)
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            
            services.Configure<IdentityOptions>(options =>
            {
                    
                    options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                   
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                   
                    options.User.RequireUniqueEmail = true;
            });

           
            services.ConfigureApplicationCookie(options =>
            {
                   
                    options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login"; 
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied"; 
                    options.SlidingExpiration = true;
            });

            var connection = @"Server=ZALNET-PC\SQLCOURSE2017;Database=ETDatabase;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<ETDatabaseContext>(options => options.UseSqlServer(connection));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAdminAccess", policy => policy.RequireRole("Admin"));
            });
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateUserRoles(services).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "User", "Manager" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                   
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            ApplicationUser user = await UserManager.FindByEmailAsync("beata@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "beata@gmail.com",
                    Email = "beata@gmail.com",
                };
                await UserManager.CreateAsync(user, "Pa55w.rd");
            }
            await UserManager.AddToRoleAsync(user, "Admin");
            ApplicationUser user1 = await UserManager.FindByEmailAsync("daria@gmail.com");
            if (user1 == null)
            {
                user1 = new ApplicationUser()
                {
                    UserName = "daria@gmail.com",
                    Email = "daria@gmail.com",
                };
                await UserManager.CreateAsync(user1, "Pa55w.rd");
            }
            await UserManager.AddToRoleAsync(user1, "User");
            ApplicationUser user2 = await UserManager.FindByEmailAsync("romek@gmail.com");
            if (user2 == null)
            {
                user2 = new ()
                {
                    UserName = "romek@gmail.com",
                    Email = "romek@gmail.com",
                };
                await UserManager.CreateAsync(user2, "Pa55w.rd");
            }
            await UserManager.AddToRoleAsync(user2, "Manager");
        }
    }
}


//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        // This method gets called by the runtime. Use this method to add services to the container.
//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.Configure<CookiePolicyOptions>(options =>
//            {
//                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//                options.CheckConsentNeeded = context => true;
//                options.MinimumSameSitePolicy = SameSiteMode.None;
//            });

//            services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(
//                    Configuration.GetConnectionString("UsersConnection")));
//            services.AddDefaultIdentity<IdentityUser>()
//                .AddDefaultUI(UIFramework.Bootstrap4)
//                .AddEntityFrameworkStores<ApplicationDbContext>();

//            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
//        }

//        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//                app.UseDatabaseErrorPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();
//            app.UseCookiePolicy();

//            app.UseAuthentication(); // tu trzeba przekazać w opcjach aby używał ról w autoryzacji

//            app.UseMvc(routes =>
//            {
//                routes.MapRoute(
//                    name: "default",
//                    template: "{controller=Home}/{action=Index}/{id?}");
//            });
//        }
//    }
//}
