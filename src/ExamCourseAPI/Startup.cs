using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            switch (Configuration.GetSection("UseDatabase").Value)
            {
                case SQLite:
                    services.AddDbContext<Context>(o => o.UseSqlite(Configuration.GetConnectionString("SQLiteConnection")));
                    break;
                case SQL:
                    services.AddDbContext<Context>(o => o.UseSqlServer(Configuration.GetConnectionString("SQLConnection")));
                    break;
            }
            services.AddTransient<Repository<Course>>();
            services.AddTransient<Repository<User>>();
            //services.AddTransient<Repository<TutorialCourse>>();
            //services.AddTransient<Repository<ExamCourse>>();
            services
                .AddMvcCore()
                .AddDataAnnotations()
                .AddJsonFormatters()
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented);
                //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(Configuration);            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
