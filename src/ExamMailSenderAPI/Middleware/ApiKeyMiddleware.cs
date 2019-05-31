using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string key = "api-key";
            if (!context.Request.Headers.Keys.Contains(key))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Missing Api Key");
                return;
            }
            else
            {
                var isValid = configuration.GetValue(typeof(string), key) == context.Request.Headers[key];
                if (!isValid)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid Api Key");
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
