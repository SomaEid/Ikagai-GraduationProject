namespace Ikagai.Dtos
{
    public class BloodBankDetails
    {
        public Guid BloodBankId { get; set; }
        public Guid OrderId { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Order { get; set; }
        public int Quantity { get; set; }
        public string BloodBankName { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }

    }
}
