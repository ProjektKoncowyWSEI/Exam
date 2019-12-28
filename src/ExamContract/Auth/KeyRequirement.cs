using ExamContract.Auth;
using Microsoft.AspNetCore.Authorization;

namespace ExamContract.Auth
{
    public class KeyRequirement : IAuthorizationRequirement
    {
        public RoleEnum Role { get; private set; }
        public KeyRequirement(RoleEnum Role)
        {
            this.Role = Role;
        }
    }
}
