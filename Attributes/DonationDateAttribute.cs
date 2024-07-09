using System.ComponentModel.DataAnnotations;

namespace Ikagai.Attributes
{
    public class DonationDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            DateTime donationDate = Convert.ToDateTime(value);

            DateTime Now = DateTime.Now;

            if (donationDate.Date > Now.Date)
                return false;
            else
                return true;
        }
        public override string FormatErrorMessage(string name)
        {
            return "Donation Date Must be Less Than Date Now";
        }
    }
}
