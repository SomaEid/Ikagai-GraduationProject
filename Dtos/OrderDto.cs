using System.ComponentModel.DataAnnotations;

namespace Ikagai.Dtos
{
    public class OrderDto
    {
        [Required(ErrorMessage = "City Field is required")]
        public byte CityId { get; set; }

        [Required(ErrorMessage = "Governorate Field is required")]
        public byte GovernorateId { get; set; }

        [Required(ErrorMessage = "Location Field is required")]
        [MaxLength(200, ErrorMessage = "Location Cannot be More Than 200 Character")]
        public string Location { get; set; }
        public bool IsFree { get; set; }
        public bool WithDelivery { get; set; }

        [Required(ErrorMessage = "Blood And Derivatives Field is required")]
        public byte BloodAndDerivativesId { get; set; }


        [Required(ErrorMessage = "Quantity Field is required")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "Duration Field is required")]
        public byte RequiredDuration { get; set; }
        public Guid? PersonId  { get; set; }
        public Guid? BloodBankId { get; set; }

    }
}
