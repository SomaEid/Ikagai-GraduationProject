using Microsoft.EntityFrameworkCore;

namespace Ikagai.Core
{
    [PrimaryKey(nameof(PersonId), nameof(OrderId))]
    public class DonorOrder
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid PersonId { get; set; }
        public Person Person { get; set; }
        public bool? IsDonorAccept { get; set; }
        public bool? IsCustomerAccept { get; set; }
    }
}
