namespace NetTemplate.Shared.ApplicationCore.Common
{
    public static class Constants
    {
        public static class Constraints
        {
            public const int MaxStringLength = 255;
            public const int IdMaxLength = 50;
        }

        public static class Filter
        {
            public const int Take = 10;
        }

        public static class Messages
        {
            public static readonly string InvalidMaxLength = $"Field(s) has invalid length ({Constraints.MaxStringLength})";
            public const string MissingRequiredFields = "Missing required fields";
            public const string ObjectResult = "Object result";
            public const string NotFound = "Not found";
            public const string BadRequest = "Bad request";
            public const string AccessDenied = "Access is denied";
            public const string UnknownError = "Unknown error";
            public const string PersistenceError = "Persistence error";
            public const string InvalidPagination = "Invalid pagination";
            public const string InvalidData = "Invalid data";
            public const string IsDescAndSortByMismatch = "IsDesc and SortBy data mismatch";
        }

        public static class ViewPreservedKeys
        {
            public const string Version = "@version";
        }
    }
}
