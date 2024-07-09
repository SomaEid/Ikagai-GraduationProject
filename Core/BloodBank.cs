using System.ComponentModel.DataAnnotations;

namespace Ikagai.Core
{
    public class BloodBank
    {
        public BloodBank()
            => Id = new Guid();

        [Key]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodBankName { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string Location { get; set; }
        public byte StatusId { get; set; }
        public Status Status { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public byte CityId { get; set; }
        public City City { get; set; }
        public byte GovernorateId { get; set; }
        public Governorate Governorate { get; set; }

    }
}
