namespace Ikagai.Dtos
{
    public class BloodBanksDonation
    {
        public Guid BloodBankId { get; set; }
        public string BloodBankName { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Location { get; set; }
    }
}
