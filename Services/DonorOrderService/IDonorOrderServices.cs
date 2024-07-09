using Ikagai.Dtos;

namespace Ikagai.Services.DonorOrderService
{
    public interface IDonorOrderServices
    {
        Task<List<OrderDetails>> GetDonorOrders(Guid donorId);
        Task SetOrderStatus(Guid orderId, Guid personId, bool isAccept);
        Task<List<DonorDetails>> GetAcceptedDonors(Guid customerId, bool? isCustomerAccept);
        Task SetCustomerOrderStatus(Guid orderId, Guid personId, bool isAccept);
        Task<List<OrderDetails>> GetAcceptedOrders(Guid donorId);
    }
}
