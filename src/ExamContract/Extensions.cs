using ExamContract.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ExamContract
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
        public static string CurrentRole(this ClaimsPrincipal user)
        {
            if (user.IsInRole(RoleEnum.admin))
            {
                return RoleEnum.admin.ToString();
            }
            if (user.IsInRole(RoleEnum.teacher))
            {
                return RoleEnum.teacher.ToString();
            }
            if (user.IsInRole(RoleEnum.student))
            {
                return RoleEnum.student.ToString();
            }
            return "";
        }
        public static RoleEnum CurrentRoleEnum(this ClaimsPrincipal user)
        {
            if (user.IsInRole(RoleEnum.admin))
            {
                return RoleEnum.admin;
            }
            if (user.IsInRole(RoleEnum.teacher))
            {
                return RoleEnum.teacher;
            }
            if (user.IsInRole(RoleEnum.student))
            {
                return RoleEnum.student;
            }
            return RoleEnum.lack;
        }
    }
}
