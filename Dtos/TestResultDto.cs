namespace Ikagai.Dtos
{
    public class TestResultDto
    {
        public IFormFile ResultImg { get; set; }
        public string? Comments { get; set; }
        public bool DonateAgain { get; set; }
        public Guid DonationRequestId { get; set; }
        public Guid BloodBankId { get; set; }
    }
}
