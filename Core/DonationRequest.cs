namespace Ikagai.Core
{
    public class DonationRequest
    {
        public DonationRequest()
        {
            Id = new Guid();
            RequestDate = DateOnly.FromDateTime(DateTime.Now);
        }
        public Guid Id { get; set; }
        public DateOnly LastDonationDate { get; set; }
        public DateOnly RequestDate { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public Guid? BloodBankId { get; set; }
        public BloodBank? BloodBank { get; set; }
        public byte StatusId { get; set; } // Accept -> 7 , Refuse -> 4 , Delayed -> 8
        public Status Status { get; set; }
        public byte GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
        public byte CityId { get; set; }
        public City City { get; set; }
        public string Location { get; set; }
    }
}
