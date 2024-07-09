namespace Ikagai.Dtos
{
    public class BloodBankData
    {
        public Guid BloodBankId { get; set; }
        public string BloodBank { get; set; }
        public decimal Price { get; set; }
        public int AcceptedQuantity { get; set; }
        public string PhoneNumber { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string City { get; set; }
        public string Goveronate { get; set; }
        public string Location { get; set; }
    }
}
