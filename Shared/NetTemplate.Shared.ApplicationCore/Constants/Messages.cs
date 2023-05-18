namespace NetTemplate.Shared.ApplicationCore.Constants
{
    public static class Messages
    {
        public static class Common
        {
            public static readonly string InvalidMaxLength = $"Field(s) has invalid length ({Constraints.Common.MaxStringLength})";
            public const string MissingRequiredFields = "Missing required fields";
            public const string ObjectResult = "Object result";
            public const string NotFound = "Not found";
            public const string BadRequest = "Bad request";
            public const string AccessDenied = "Access is denied";
            public const string UnknownError = "Unknown error";
            public const string PersistenceError = "Persistence error";
            public const string InvalidPagination = "Invalid pagination";
            public const string InvalidData = "Invalid data";
        }
    }
}
