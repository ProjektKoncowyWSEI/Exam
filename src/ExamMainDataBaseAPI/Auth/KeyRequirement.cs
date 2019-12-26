using Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.Auth
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
