using Ikagai.Dtos;

namespace Ikagai.Services.BloodBankOrderService
{
    public interface IBloodBankOrderServices
    {
        Task<List<OrderDetails>> GetBloodBankNewOrders(Guid bloodBankId);
        Task SetOrderStatus(Guid bloodBankId, Guid orderId, bool isBloodBankAccept, decimal price = 0, int quantity = 0);
        Task<bool> IsValidQuantity(Guid orderId, int quantity);
        Task SetCustomerStatus(Guid bloodBankId, Guid orderId, bool isAccept , int quantity = 0);
        Task<List<BloodBankDetails>> GetBloodBanksDetails(Guid customerId, bool? iscustomerAccept);
        Task<List<OrderDetails>> GetAcceptedOrderDetails(Guid bloodBankId);
   //     Task<int> Getvalidchange(Guid orderId, int newQuantity);
        Task<DeliveryDetails> GetDeliveryDetails(Guid orderId);
    }
}
