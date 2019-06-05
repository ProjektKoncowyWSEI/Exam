using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Helpers
{
    public static class Extensions
    {
        public static string ListToString(this List<string> list)
        {
            if (list != null)
            {
                StringBuilder sb = new StringBuilder();
                list.ForEach(l =>
                {
                    sb.Append(l).Append(";");
                });
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return "";
        }
        public static string ToRoleString(this RoleEnum[] roles)
        {
            var result = "";       
            if (roles.Length > 0 && !roles.Any(r => r == RoleEnum.admin))
                result = $"{RoleEnum.admin.ToString()},";
            result += string.Join(",", roles);
            return result;
        }
        public static bool IsInRole(this ClaimsPrincipal user, RoleEnum role)
        {
            return user.IsInRole(role.ToString());
        }
    }
}
