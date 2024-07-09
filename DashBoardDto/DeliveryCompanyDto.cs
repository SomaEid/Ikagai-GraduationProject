using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class DeliveryCompanyDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeliveryCompanyName { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string Location { get; set; }
        public string CommercialRegister { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public int NumberOfRecievedOrders { get; set; }
    }
}
