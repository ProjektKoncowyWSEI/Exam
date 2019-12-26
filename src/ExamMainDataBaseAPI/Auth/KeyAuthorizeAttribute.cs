using Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Auth
{
    public class KeyAuthorizeAttribute : AuthorizeAttribute
    {   
        public KeyAuthorizeAttribute(RoleEnum policy)
        {
            Policy = policy.ToString();
        }
    }
}
