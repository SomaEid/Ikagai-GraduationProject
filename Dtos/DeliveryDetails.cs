namespace Ikagai.Dtos
{
    public class DeliveryDetails
    {
        public Guid OrderId { get; set; }
        public Guid DeliveryCompanyId { get; set; }
        public string Order { get; set; }
        public DateOnly OrderDate { get; set; }
        public int Quantity { get; set; }
        public Decimal OrderPrice { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public decimal DeliveryCost { get; set; }
    }
}
