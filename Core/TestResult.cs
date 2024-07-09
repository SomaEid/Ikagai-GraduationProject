using System.ComponentModel.DataAnnotations;

namespace Ikagai.Core
{
    public class TestResult
    {
        public TestResult() => (Id, ResultDate) = (new Guid(), DateOnly.FromDateTime(DateTime.Now));

        [Key]
        public Guid Id { get; set; }
        public byte[] ResultImg { get; set; }
        public string? Comments { get; set; }
        public DateOnly ResultDate { get; set; }
        public bool DonateAgain { get; set; }
        public Guid DonationRequestId { get; set; }
        public DonationRequest DonationRequest { get; set; }
        public Guid BloodBankId { get; set; }
        public BloodBank BloodBank { get; set; }
    }
}
