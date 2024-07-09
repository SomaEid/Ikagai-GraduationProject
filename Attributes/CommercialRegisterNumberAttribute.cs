using System.ComponentModel.DataAnnotations;

namespace Ikagai.Attributes
{
    public class CommercialRegisterNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var commercialRegisterNumber = value as string;

            if (string.IsNullOrEmpty(commercialRegisterNumber))
            {
                return ValidationResult.Success;
            }

            // Length check
            if (commercialRegisterNumber.Length != 9)
            {
                return new ValidationResult("Commercial register number must be 9 digits.");
            }

            // Format check (must contain only digits)
            foreach (char c in commercialRegisterNumber)
            {
                if (!char.IsDigit(c))
                {
                    return new ValidationResult("Commercial register number must contain only digits.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
