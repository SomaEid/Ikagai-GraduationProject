namespace Ikagai.DashBoardDto
{
    public class PaiedOrder
    {
        public Guid BloodBankId { get; set; }
        public string BloodBank { get; set; }
        public string BloodBankStatus { get; set; }
        public string CustomerStatus { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
