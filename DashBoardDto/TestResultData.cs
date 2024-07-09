using Ikagai.Core;

namespace Ikagai.DashBoardDto
{
    public class TestResultData
    {
        public Guid Id { get; set; }
        public byte[] ResultImg { get; set; }
        public string Comments { get; set; }
        public DateOnly ResultDate { get; set; }
        public bool DonateAgain { get; set; }
    }
}
