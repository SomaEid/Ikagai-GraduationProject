using Ikagai.HelperModels;

namespace Ikagai.Core
{
    public class City : Base
    {
        public byte GovernorateId { get; set; }
        public Governorate Governorate { get; set; }
    }
}
