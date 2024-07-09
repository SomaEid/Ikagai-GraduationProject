using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class DonationRequestData
    {
        public Guid RequestId { get; set; }
        public DateOnly LastDonationDate { get; set; }
        public DateOnly RequestDate { get; set; }
        public string Donor { get; set; }
        public string BloodBank { get; set; }
        public string Status { get; set; } // Accept -> 7 , Refuse -> 4 , Delayed -> 8
        public string Governorate { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public string BloodType { get; set; }

    }
}
