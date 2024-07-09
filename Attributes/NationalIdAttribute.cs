using System.ComponentModel.DataAnnotations;

namespace Ikagai.Attributes
{
    public class NationalIdAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Id = value as string;

            // Length check
            if (Id.Length != 14)
            {
                return new ValidationResult("National Id  must be 14 digits.");
            }

            // Format check (must contain only digits)
            foreach (char c in Id)
            {
                if (!char.IsDigit(c))
                {
                    return new ValidationResult("National Id number must contain only digits.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
