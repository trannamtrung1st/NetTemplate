namespace NetTemplate.Shared.WebApi.Common
{
    public static class Constants
    {
        public static class Environment
        {
            public const string VariableName = "ASPNETCORE_ENVIRONMENT";
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
        }

        public static class Messages
        {
            public const string SwaggerInstruction = "Please go to /swagger for API documentation.";
        }
    }
}
