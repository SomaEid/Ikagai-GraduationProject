namespace Ikagai.Dtos
{
    public class CustomerDetails
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Order { get; set; }
        public int Quantity { get; set; }
        public DateOnly OrderDate { get; set; }
    }
}
