using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exam.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Exam.Services;
using Exam.Filters;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using Exam.Areas.Identity.Pages.Account;
using ExamContract.MainDbModels;
using Exam.Data.UnitOfWork;
using Exam.Areas.Identity.Pages.Account.Manage;
using ExamContract.CourseModels;
using Exam.Controllers;
using ExamContract.TutorialModels;

namespace Exam
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            string userConnection = Environment.GetEnvironmentVariable("EXAM_UsersConnection") ?? Configuration.GetConnectionString("UsersConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(userConnection));

            services.AddDbContext<Logger.Data.LoggerDbContext>(options =>
               options.UseSqlite(
                   Configuration.GetConnectionString("LoggerConnection")));

            services.AddDefaultIdentity<IdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = false; // TODO jesli uznamy że konieczne jest potwierdzenie E-mail
            })
              .AddRoles<IdentityRole>()
              .AddDefaultUI(UIFramework.Bootstrap4)
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<EmailSender>();
            services.AddTransient<UserInitializer>();
            services.AddScoped<ILogger, Logger.ExamLogger>();
            services.AddScoped<ILogger<LoginModel>, Logger.ExamLogger<LoginModel>>();
            services.AddScoped<ILogger<ChangePasswordModel>, Logger.ExamLogger<ChangePasswordModel>>();            
            services.AddScoped<ILogger<ExamsController>, Logger.ExamLogger<ExamsController>>();            
            services.AddTransient<WebApiClient<Tutorial>, TutorialsApiClient>();
            services.AddTransient<WebApiClient<ExamContract.MainDbModels.Exam>, ExamsApiClient>();
            services.AddTransient<WebApiClient<Question>, QuestionsApiClient>();
            services.AddTransient<WebApiClient<Answer>, AnswersApiClient>();             
            services.AddTransient<WebApiClient<ExamContract.MainDbModels.User>, UsersMainDbApiClient>();
            services.AddTransient<WebApiClient<ExamContract.CourseModels.User>, UsersCoursesApiClient>(); 
            services.AddTransient<WebApiClient<ExamContract.TutorialModels.User>, UsersTutorialsClient>(); 
            services.AddTransient<WebApiClient<Course>, CoursesApiClient>();
            services.AddTransient<CourseTwoKeyApiClient<ExamCourse>, CourseExamApiClient>();
            services.AddTransient<CourseTwoKeyApiClient<TutorialCourse>, CourseTutorialApiClient>();            
            services.AddTransient<ExamsQuestionsAnswersApiClient>(); //Wymagamy klasy konkretnej
            services.AddTransient<ExamApproachesApiClient>();
            services.AddTransient<Exams>();
            services.AddTransient<Courses>();
            services.AddTransient<Tutorials>();
            services.AddTransient<ApiStrarter>();


            services.ConfigureApplicationCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });

            services.AddHttpContextAccessor();
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization(options => options
                    .DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pl-PL")
                };

                options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
                options.SupportedCultures = supportCultures;
                options.SupportedUICultures = supportCultures;
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "exam",
                   template: "StartExam/{code?}",
                   defaults: new { controller = "ExamApproaches", action = "Index" });
                routes.MapRoute(
                   name: "egzamin",
                   template: "Egzamin/{code?}",
                   defaults: new { controller = "ExamApproaches", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var serviceProvider = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider;
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var roleMenager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var initializer = new UserInitializer(roleMenager, userManager, Configuration);
            initializer.CreateRolesAsync().Wait();
            initializer.CreateDefaultUser().Wait();

        }
    }
}