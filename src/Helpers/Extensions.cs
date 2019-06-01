using System;
using System.Collections.Generic;
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
    }
}
