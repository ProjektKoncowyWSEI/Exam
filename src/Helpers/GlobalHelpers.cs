using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class GlobalHelpers
    {
        public static string GetShortGuid => Guid.NewGuid().ToString("D").Substring(0, 5);
        public const string ACTIVE = "OnlyActiveExams";
    }
}
