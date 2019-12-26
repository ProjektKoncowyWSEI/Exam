using Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ExamContract.Auth
{
    public class KeyAuthorizeAttribute : AuthorizeAttribute
    {   
        public KeyAuthorizeAttribute(RoleEnum policy)
        {
            Policy = policy.ToString();
        }
    }
}
