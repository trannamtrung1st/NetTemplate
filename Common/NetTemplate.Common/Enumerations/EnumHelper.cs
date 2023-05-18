using EnumsNET;

namespace NetTemplate.Common.Enumerations
{
    public static class EnumHelper
    {
        public static IEnumerable<EnumType> GetListFrom<EnumType>() where EnumType : struct, Enum
            => Enums.GetMembers(typeof(EnumType)).Select(e => (EnumType)e.Value);
    }
}
