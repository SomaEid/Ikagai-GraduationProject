using System.ComponentModel.DataAnnotations;

namespace Ikagai.Core
{
    public class  Person
    {
        public Person()
            => Id = new Guid();

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public bool Gender { get; set; } // true -> Male , false -> Female
        public string RoleName { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public byte BloodAndDerivativesId { get; set; }
        public BloodAndDerivatives BloodAndDerivatives { get; set; }
        public string Location { get; set; } 
        public byte CityId { get; set; }
        public City City { get; set; }
        public byte GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
    }
}
