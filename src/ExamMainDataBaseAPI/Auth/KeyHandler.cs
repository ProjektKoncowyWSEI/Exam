using ExamMainDataBaseAPI.DAL;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Auth
{
    public class KeyHandler : AuthorizationHandler<KeyRequirement>
    {
        private IHttpContextAccessor contextAccessor;
        private readonly ApiKeyRepo repo;
        public KeyHandler(IHttpContextAccessor contextAccessor, ApiKeyRepo repo)
        {
            this.contextAccessor = contextAccessor;
            this.repo = repo;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, KeyRequirement requirement)
        {
            var roles = repo.GetDictionary();
            var httpContext = contextAccessor.HttpContext;
            string key = httpContext.Request.Headers[GlobalHelpers.ApiKey];
            if (key != null)
            {
                string role = roles.ContainsKey(key) ? roles[key] : RoleEnum.lack.ToString();
                if (role == requirement.Role.ToString())
                {
                    context.Succeed(requirement);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    httpContext.Response.WriteAsync(GlobalHelpers.AccesDenied);
                }
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.WriteAsync(GlobalHelpers.MissingApiKey);
            }
            return Task.CompletedTask;
        }
    }
}
