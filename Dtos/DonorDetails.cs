namespace Ikagai.Dtos
{
    public class DonorDetails
    {
        public Guid OrderId { get; set; }
        public Guid DonorId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string BloodType { get; set; }
        public string Order { get; set; }
        public DateOnly OrderDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
