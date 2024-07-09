using Ikagai.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Ikagai.Dtos
{
    public class DonationRequestDto
    {
        [DonationDate(ErrorMessage = "Donation Date Must Be Less Than Date Now")]
        public DateTime LastDonationDate { get; set; }


        [Required(ErrorMessage = "Governorate Field is Required")]
        public byte GovernorateId { get; set; }


        [Required(ErrorMessage = "City Field is Required")]
        public byte CityId { get; set; }


        [Required(ErrorMessage = "Location Field is Required")]
        public string Location { get; set; }

        [Required(ErrorMessage = "status Field is Required")]
        public List<byte> StatusId { get; set; } // Accept -> 7 , Refuse -> 4 , Delayed -> 8


        [Required(ErrorMessage = "Person Field is Required")]
        public Guid PersonId { get; set; }

    }
}
