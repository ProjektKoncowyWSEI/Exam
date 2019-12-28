﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.DAL;
using ExamTutorialsAPI.Helpers;
using ExamTutorialsAPI.Models;
using ExamContract.Auth;
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
using Helpers;

namespace ExamTutorialAPI
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

        public void ConfigureServices(IServiceCollection services)
        {            
            string connectionSQL = Environment.GetEnvironmentVariable("EXAM_TutorialsConnection") ?? Configuration.GetConnectionString("SQLConnection");
            string connectionSQLite = Environment.GetEnvironmentVariable("EXAM_TutorialsConnectionSQLite") ?? Configuration.GetConnectionString("SQLiteConnection");
            switch (Configuration.GetSection("UseDatabase").Value)
            {
                case SQLite:
                    services.AddDbContext<Context>(o => o.UseSqlite(connectionSQLite));
                    break;
                case SQL:
                    services.AddDbContext<Context>(o => o.UseSqlServer(connectionSQL));
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
            services.AddSingleton(Configuration);      
            services.AddTransient<Repository<Tutorial>>();
            services.AddTransient<Repository<User>>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<ApiKeyRepo>();
            services.AddTransient<DbContext, Context>();
        }
                
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
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
