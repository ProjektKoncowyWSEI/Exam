using Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            this.configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string key = "Exam_mailApiKey";
            if (!context.Request.Headers.Keys.Contains(GlobalHelpers.ApiKey))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(GlobalHelpers.MissingApiKey);
                return;
            }
            else
            {
                var apiKey = Environment.GetEnvironmentVariable(key) ?? configuration.GetValue(typeof(string), key);
                var isValid = apiKey == context.Request.Headers[GlobalHelpers.ApiKey];
                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(GlobalHelpers.InvalidApiKey);
                    return;
                }
            }
            await next.Invoke(context);
        }
    }

    #region ExtensionMethod
    public static class ApiKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyMiddleware>();
            return app;
        }
    }
    #endregion
}
