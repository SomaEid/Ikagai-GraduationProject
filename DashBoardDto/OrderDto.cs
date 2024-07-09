using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public decimal OrderPrice { get; set; }
        public string Status { get; set; }   // 3 -> Completed , 4 -> Refused , 5 -> Is Processing
        public string City { get; set; }
        public string Governorate { get; set; }
        public string Location { get; set; }
        public bool IsFree { get; set; }
        public bool WithDelivery { get; set; }
        public string BloodAndDerivatives { get; set; }
        public int Quantity { get; set; }
        public byte RequiredDuration { get; set; }
        public string CustomerName { get; set; }
    }
}
