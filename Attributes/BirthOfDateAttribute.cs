using System.ComponentModel.DataAnnotations;

namespace Ikagai.Attributes
{
    public class BirthOfDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value != null)
            {
                if (string.IsNullOrEmpty(value.ToString()))
                    return true;

                DateTime birthDate = Convert.ToDateTime(value);
                DateTime Now = DateTime.Now;

                if (birthDate < Now)
                {
                    var year = Now.Year;
                    var useryear = birthDate.Year;

                    var old = year - useryear;

                    if (old >= 18)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "InValid Birth Date , Bith Date Must be less Than Date Now and Your Age Must Be More Than Or Equal 18 Year";
        }
    }
}
