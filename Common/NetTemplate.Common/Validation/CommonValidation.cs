using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NetTemplate.Common.Validation
{
    public static class CommonValidation
    {
        static readonly EmailAddressAttribute _emailAddressAttribute = new EmailAddressAttribute();

        public static bool IsValidEmailAddress(string address)
            => address != null && _emailAddressAttribute.IsValid(address);

        public const string PhoneNumberRegex = "^[+]?[\\d]{6,17}$";

        public static bool IsValidPhoneNumber(string phoneNo)
            => phoneNo != null && Regex.IsMatch(phoneNo, PhoneNumberRegex);
    }
}
