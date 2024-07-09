using Ikagai.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Ikagai.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "NationalId is Required")]
        [NationalId(ErrorMessage = "InValid National Id")]
        public string NationalId { get; set; }


        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [Compare("Password", ErrorMessage = "Password Not Match")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "InValid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PhoneNumber is Required")]
        [MaxLength(11, ErrorMessage = "PhoneNumber Cannot be more than 11 number"), MinLength(11, ErrorMessage = "PhoneNumber Cannot be Less than 11 number")]
        [EgyptionPhone(ErrorMessage = "InValid Phone Numer")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Role Name is Required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Location is Required")]
        [MaxLength(200, ErrorMessage = "Location Cannot be more than 200 character")]
        public string Location { get; set; }

        [Required(ErrorMessage = "City is Required")]
        public byte CityId { get; set; }

        [Required(ErrorMessage = "governorate is Required")]
        public byte GovernorateId { get; set; }

        //Person
        [BirthOfDate(ErrorMessage = "BirthOfDate")]
        public DateTime? BirthDate { get; set; }
        public byte? BloodId { get; set; }
        public bool? Gender { get; set; } // True -> Male  ,  False -> Female

        //BloodBank Or DeliveryCompany
        public TimeSpan? OpenHour { get; set; } // Formate 00:00:00
        public TimeSpan? ClosedHour { get; set; } // Formate 00:00:00 

        //BloodBank
        public byte? StatusId { get; set; }// Private facility -> 1  (OR) Government facility -> 2
        public string? BloodBankName { get; set; }

        // DeliveryCompany
        [CommercialRegisterNumber(ErrorMessage = "CommercialRegisterNumber")]
        public string? CommercialRegister { get; set; } = null;
        public string? CompanyName { get; set; }
    }

}
