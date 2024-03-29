﻿using System;
using ExamContract.Auth;
using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Helpers;

namespace ExamCourseAPI
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
            string connectionSQL = Environment.GetEnvironmentVariable("EXAM_CoursesConnection") ?? Configuration.GetConnectionString("SQLConnection");
            string connectionSQLite = Environment.GetEnvironmentVariable("EXAM_CoursesConnectionSQLite") ?? Configuration.GetConnectionString("SQLiteConnection");
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
                .AddMvcCore() 
                .AddDataAnnotations()
                .AddJsonFormatters()
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented);
            services.AddSingleton(Configuration);
            services.AddTransient<Repository<Course>>();
            services.AddTransient<Repository<User>>();
            services.AddTransient<TwoKeysRepository<TutorialCourse>>();
            services.AddTransient<TwoKeysRepository<ExamCourse>>();
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
