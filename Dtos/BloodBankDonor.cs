namespace Ikagai.Dtos
{
    public class BloodBankDonor
    {
        public Guid DonationRquestId { get; set; }
        public Guid DonorId { get; set; }
        public string Donor { get; set; }
        public DateOnly RequestDate { get; set; }
        public string BloodType { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    }
}
