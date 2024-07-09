using Microsoft.EntityFrameworkCore;

namespace Ikagai.Core
{
    [PrimaryKey(nameof(DeliveryCompanyId) , nameof(OrderId))]
    public class DeliveryCompanyServices
    {
        public Guid DeliveryCompanyId { get; set; }
        public DeliveryCompany DeliveryCompany { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public decimal DeliveryCost { get; set; }
        public bool? IsCustomerAccept { get; set; }
        public bool? IsDeliveryCompanyAccept { get; set; }
    }
}
