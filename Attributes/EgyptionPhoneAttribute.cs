using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Ikagai.Attributes
{
    public class EgyptionPhoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return false;

            string regexPattern = @"^(010|011|012|015)\d{8}$";

            return Regex.IsMatch(value.ToString(), regexPattern);
        }
        public override string FormatErrorMessage(string name)
        {
            return "Invalid Egyptian phone number format";
        }
    }
}
