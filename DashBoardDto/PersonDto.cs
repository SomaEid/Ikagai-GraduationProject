using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Gender { get; set; } // true -> Male , false -> Female
        public string RoleName { get; set; }
        public string BloodType { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Governorate { get; set; }
        public int NumberOfOrders { get; set; }
        public int NumberOfDonationRequests { get; set; }
    }
}
