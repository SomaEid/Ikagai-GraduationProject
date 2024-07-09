namespace Ikagai.Dtos
{
    public class DonorResult
    {
        public Guid ResultId { get; set; }
        public Guid RequestId { get; set; }
        public DateOnly ResultDate { get; set; }
        public byte[] ResultImg { get; set; }
        public string Comments { get; set; }
        public string DonateAgain { get; set; }
        public string ResultFrom { get; set; }
        public Guid BloodBankId { get; set; }
    }
}
