using Microsoft.EntityFrameworkCore;

namespace Ikagai.Core
{
    [PrimaryKey(nameof(OrderId) , nameof(BloodBankId))]
    public class BloodBankOrder
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid BloodBankId { get; set; }
        public BloodBank BloodBank { get; set; }
        public bool? IsBloodBankAccept { get; set; }
        public bool? IsCustomerAccept { get; set; }
        public decimal price { get; set; }
        public int Quanitty { get; set; }
    }
}
