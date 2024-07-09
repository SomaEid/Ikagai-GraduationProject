using Ikagai.Core;
using Ikagai.Dtos;

namespace Ikagai.Services.DeliveryComapnyService
{
    public interface IDeliveryOrdersService
    {
        Task<List<OrderDetails>> GetOrderDetails(Guid deliveryCompanyId);
        Task SetOrderStatus(Guid deliveryCompanyId, Guid orderId, bool isDeliveryAccept, decimal deliveryCost = 0);
        Task<List<DeliveryDetails>> GetDeliveryDetails(Guid customerId, bool? iscustomerAccept);
        Task SetDeliverySatuts(Guid deliveryId, Guid orderId, bool isCustomerAccept);
        Task<List<OrderDetails>> GetAcceptedOrders(Guid deliveryComapnyId);
        Task<List<BloodBankOrderDetaills>> GetBloodBankOrdersDetails(Guid orderId);
        
    }
}
