namespace Ikagai.Dtos
{
    public class AllOrders
    {
        public Guid OrderId { get; set; }
        public string Order { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public decimal TotalCost { get; set; }
        public byte RequiredDuration { get; set; }
        public DateOnly Date { get; set; }
        public string IsFree { get; set; }
    }
}
