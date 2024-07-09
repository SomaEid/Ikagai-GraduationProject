namespace Ikagai.Dtos
{
    public class DeliveryCompanyData
    {
        public Guid DeliveryCompanyId { get; set; }
        public string DeliveryCompany { get; set; }
        public decimal deliveryCost { get; set; }
        public string PhoneNumber { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string City { get; set; }
        public string Goveronate { get; set; }
        public string Location { get; set; }
    }
}
