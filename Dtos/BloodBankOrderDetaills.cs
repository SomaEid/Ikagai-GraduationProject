using System.Reflection.Metadata.Ecma335;

namespace Ikagai.Dtos
{
    public class BloodBankOrderDetaills
    {
        public Guid BloodBankId { get; set; }
        public string BloodBankName { get; set; }
        public string BloodBankLocation { get; set; }
        public decimal BloodPrice { get; set; }
        public int Quantity { get; set; }
        public string PhoneNumber { get; set; }

    }
}
