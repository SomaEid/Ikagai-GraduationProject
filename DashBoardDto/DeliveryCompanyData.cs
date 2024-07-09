namespace Ikagai.DashBoardDto
{
    public class DeliveryCompanyData
    {
        public Guid DeliveryCompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DeliveryCompanyStatus { get; set; }
        public string CustomerStatus { get; set; }
        public decimal DeliveryCost { get; set; }

    }
}
