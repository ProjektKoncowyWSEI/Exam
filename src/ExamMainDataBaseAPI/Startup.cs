using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExamMainDataBaseAPI
{
    public class Startup
    {
        const string SQLite = "SQLite";
        const string SQL = "SQL";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string mainDbConnection = Environment.GetEnvironmentVariable("EXAM_MainDBConnection") ?? Configuration.GetConnectionString("SQLConnection");
            string mainDbConnectionSQLite = Environment.GetEnvironmentVariable("EXAM_MainDBConnectionSQLite") ?? Configuration.GetConnectionString("SQLiteConnection");
            switch (Configuration.GetSection("UseDatabase").Value)
            {
                case SQLite:
                    services.AddDbContext<Context>(o => o.UseSqlite(mainDbConnectionSQLite));
                    break;
                case SQL:
                    services.AddDbContext<Context>(o => o.UseSqlServer(mainDbConnection));
                    break;
            }
            services.AddHttpContextAccessor();

            services
               .AddMvcCore() //AddMVC przed autoryzacją
               .AddAuthorization(options =>
               {
                   options.AddPolicy(RoleEnum.admin.ToString(), policy =>
                       policy.Requirements.Add(new KeyRequirement(RoleEnum.admin)));
                   options.AddPolicy(RoleEnum.teacher.ToString(), policy =>
                       policy.Requirements.Add(new KeyRequirement(RoleEnum.teacher)));
                   options.AddPolicy(RoleEnum.student.ToString(), policy =>
                       policy.Requirements.Add(new KeyRequirement(RoleEnum.student)));
                   options.AddPolicy(RoleEnum.lack.ToString(), policy =>
                       policy.Requirements.Add(new KeyRequirement(RoleEnum.lack)));
               })
               .AddDataAnnotations()
               .AddJsonFormatters()
               .AddJsonOptions(o => o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented);

            services.AddTransient<Repository<Answer>>();
            services.AddTransient<Repository<Question>>();           
            services.AddTransient<Repository<Exam>>();
            services.AddTransient<Repository<User>>();
            services.AddTransient<UnitOfWork>();       
            services.AddTransient<ApproachesRepository>();
            services.AddTransient<ApiKeyRepo>();
            services.AddTransient<IAuthorizationHandler, KeyHandler>();
            services.AddTransient<DbContext, Context>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.Run(async (context) =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(GlobalHelpers.NotFound);
            });
        }
    }
}
