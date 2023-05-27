namespace NetTemplate.Shared.Infrastructure.Persistence
{
    public static class Constants
    {
        public static class SqlServerColumnTypes
        {
            public const string ntext = nameof(ntext);
            public const string text = nameof(text);
            public const string date = nameof(date);

            public static readonly IEnumerable<string> TextColumnTypes = new[]
            {
                text,
                ntext
            };
        }

        public static class ConstraintNames
        {
            public const string NoRestrictForeignKeyConstraintPostfix = "__NoRestrict__";
        }
    }
}
