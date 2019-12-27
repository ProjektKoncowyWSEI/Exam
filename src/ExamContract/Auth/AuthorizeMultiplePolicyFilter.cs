using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamContract.Auth
{
    public class AuthorizeMultiplePolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ApiKeyRepo repo;

        public RoleEnum[] roles { get; private set; }
        public AuthorizeMultiplePolicyFilter(IHttpContextAccessor contextAccessor, ApiKeyRepo repo, params RoleEnum[] roles)
        {
            this.contextAccessor = contextAccessor;
            this.repo = repo;
            this.roles = roles;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Pomijamy autoryzację dla AllowAnonymous
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            var repoRoles = repo.GetDictionary();
            var httpContext = contextAccessor.HttpContext;
            string key = httpContext.Request.Headers[GlobalHelpers.ApiKey];
            if (key != null)
            {
                string role = repoRoles.ContainsKey(key) ? repoRoles[key] : RoleEnum.lack.ToString();
                if (roles.Select(a=>a.ToString()).Contains(role))                
                    return;                
                else
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync(GlobalHelpers.AccesDenied);
                }
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync(GlobalHelpers.MissingApiKey);
            }            
        }
    }
}
