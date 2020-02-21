using System;
using ExamContract.Auth;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Helpers;

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
            string connectionSQL = Environment.GetEnvironmentVariable("EXAM_MainDBConnection") ?? Configuration.GetConnectionString("SQLConnection");
            string connectionSQLite = Environment.GetEnvironmentVariable("EXAM_MainDBConnectionSQLite") ?? Configuration.GetConnectionString("SQLiteConnection");
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

            services.AddTransient<Repository<Answer>>();
            services.AddTransient<Repository<Question>>();           
            services.AddTransient<Repository<Exam>>();
            services.AddTransient<Repository<User>>();
            services.AddTransient<UnitOfWork>();       
            services.AddTransient<ApproachesRepository>();
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
