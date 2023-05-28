using EnumsNET;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetTemplate.Common.Enumerations
{
    public static class EnumExtensions
    {
        public static string GetDisplayName<T>(this T @enum) where T : struct, Enum
        {
            return @enum.GetAttributes().GetDisplayName();
        }

        public static string GetDescription<T>(this T @enum) where T : struct, Enum
        {
            return @enum.GetAttributes().GetDescription();
        }

        public static string GetDisplayName(this EnumsNET.AttributeCollection attr)
        {
            string displayName = attr.OfType<DisplayAttribute>().FirstOrDefault()?.Name;

            return displayName;
        }

        public static string GetDescription(this EnumsNET.AttributeCollection attr)
        {
            var description = attr.OfType<DescriptionAttribute>().FirstOrDefault()?.Description;

            if (description == null)
            {
                description = attr.OfType<DisplayAttribute>().FirstOrDefault()?.Description;
            }

            return description;
        }

        public static DisplayAttribute GetDisplay<T>(this T @enum) where T : struct, Enum
        {
            var displayAttr = @enum.GetAttributes()
                .OfType<DisplayAttribute>()
                .SingleOrDefault();

            return displayAttr;
        }

        public static string GetName<T>(this T enumVal) where T : struct, Enum
        {
            return Enums.GetName(enumVal);
        }

        public static string GetColumn<T>(this T enumVal) where T : struct, Enum
        {
            return enumVal.GetAttributes().OfType<ColumnAttribute>().SingleOrDefault()?.Name;
        }

        public static string ToStringF(this Enum enumVal)
        {
            return enumVal.ToString("F");
        }

        public static string ToStringG(this Enum enumVal)
        {
            return enumVal.ToString("G");
        }
    }
}
