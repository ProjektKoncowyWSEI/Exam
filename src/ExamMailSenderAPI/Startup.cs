using ExamContract.MailingModels;
using ExamMailSenderAPI.Data;
using ExamMailSenderAPI.Middleware;
using ExamMailSenderAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExamMailSenderAPI
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

            services
                .AddMvcCore()
                .AddDataAnnotations()
                .AddJsonFormatters()
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton(Configuration);
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<Repository<MailModel>>();
            services.AddTransient<Repository<Attachment>>();
            services.AddTransient<UnitOfWork>();
        }
              
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ApplyApiKeyValidation(); //walidacja na podstawie klucza
            app.UseMvc();
        }
    }
}
