using System;

namespace Helpers
{
    public static class GlobalHelpers
    {
        public static string GetShortGuid => Guid.NewGuid().ToString("D").Substring(0, 5);
        public const string ACTIVE = "OnlyActiveExams";
        public const string AccesDenied = "Access denied";
        public const string MissingApiKey = "Missing api-key";
        public const string InvalidApiKey = "Invalid api-key";
        public const string NotFound = "Bad request - 404 not found";
        public const string ApiKey = "api-key";
    }
}
