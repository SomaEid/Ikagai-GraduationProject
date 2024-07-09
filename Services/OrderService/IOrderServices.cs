using Ikagai.Core;
using Ikagai.Dtos;

namespace Ikagai.Services.OrderService
{
    public interface IOrderServices
    {
        Task CreateOrderAsync(OrderDto dto);
        Task<List<AllOrders>> GetAllOrders(Guid customerId, bool isFree);
        Task<List<DonorData>> GetDonorDetails(Guid orderId);
        Task<List<BloodBankData>> GetBloodBankData(Guid orderId);
        Task<List<DeliveryCompanyData>> GeteliveryData(Guid orderId);
    }
}
