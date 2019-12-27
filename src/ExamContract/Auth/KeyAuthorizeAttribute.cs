using Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.Auth
{
    public class KeyAuthorizeAttribute : TypeFilterAttribute
    {
        public KeyAuthorizeAttribute(params RoleEnum[] roles) : base(typeof(AuthorizeMultiplePolicyFilter))
        {
            Arguments = new object[] { roles };
        }
    }
}
