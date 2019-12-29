using Microsoft.AspNetCore.Mvc;

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
