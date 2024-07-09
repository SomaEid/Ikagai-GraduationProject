using Microsoft.AspNetCore.Mvc.Infrastructure;
using Org.BouncyCastle.Utilities.IO.Pem;

namespace Ikagai.Dtos
{
    public class OrderDetails
    {
        public Guid OrderId { get; set; }
        public string Order { get; set; }
        public DateOnly OrderDate { get; set; }
        public int Quantity { get; set; }
        public byte RequiredDuration { get; set; }
        public string Location { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public decimal OrderPrice { get; set; }
        public int AcceptedQuantity { get; set; }
    }
}
