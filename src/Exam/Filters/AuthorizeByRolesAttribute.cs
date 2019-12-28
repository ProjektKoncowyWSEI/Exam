using ExamContract.Auth;
using ExamContract;
using Microsoft.AspNetCore.Authorization;

namespace Exam
{
    public class AuthorizeByRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeByRolesAttribute(params RoleEnum[] roles)
        {
            if (roles.Length > 0)
                Roles = roles.ToRoleString();
        }
    }
}
