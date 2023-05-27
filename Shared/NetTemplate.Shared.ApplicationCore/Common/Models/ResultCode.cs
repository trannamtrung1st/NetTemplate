namespace NetTemplate.Shared.ApplicationCore.Common.Models
{
    public struct ResultCode
    {
        public ResultCode(int code, string description, string group)
        {
            Code = code;
            Description = description;
            Group = group;
        }

        public int Code { get; }
        public string Description { get; }
        public string Group { get; }
    }
}
