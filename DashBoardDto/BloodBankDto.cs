using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class BloodBankDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BloodBankName { get; set; }
        public TimeOnly OpenHour { get; set; }
        public TimeOnly ClosedHour { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public int  NumberOfOrders { get; set; }
        public int NumberOfResivedOrders { get; set; }
        public int NumberOfResivedDonationRequests { get; set; }
    }
}
