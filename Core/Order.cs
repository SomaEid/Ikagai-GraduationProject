using System.ComponentModel.DataAnnotations;

namespace Ikagai.Core
{
    public class Order
    {
        public Order()
            => (Id , OrderDate) = (new Guid() , DateOnly.FromDateTime(DateTime.Now));

        [Key]
        public Guid Id { get; set; }
        public DateOnly OrderDate { get; set; } 
        public decimal OrderPrice { get; set; }
        public byte StatusId { get; set; } // 3 -> Completed , 4 -> Refused , 5 -> Is Processing
        public Status Status { get; set; }
        public byte CityId { get; set; }
        public City City { get; set; }
        public byte GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
        public string Location { get; set; }
        public bool IsFree { get; set; }
        public bool WithDelivery { get; set; }
        public byte BloodAndDerivativesId { get; set; }
        public BloodAndDerivatives BloodAndDerivatives { get; set; }
        public int Quantity { get; set; }
        public byte RequiredDuration { get; set; }
        public Guid? BloodBankId { get; set; }
        public BloodBank? BloodBank { get; set; }
        public Guid? PersonId { get; set; }
        public Person? Person { get; set; }

    }
}
