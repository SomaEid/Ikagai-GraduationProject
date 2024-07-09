using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Services.BaseService;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;

namespace Ikagai.Services.DeliveryComapnyService
{
    public class DeliveryOrdersService : IDeliveryOrdersService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices _baseServices;

        public DeliveryOrdersService(ApplicationDbContext context, IBaseServices baseServices)
            => (_context, _baseServices) = (context, baseServices);
        public async Task<List<OrderDetails>> GetOrderDetails(Guid deliveryCompanyId)
            => await GetValidOrders(await _context.DeliveryCompanyServices.Where(d => d.DeliveryCompanyId.Equals(deliveryCompanyId) && d.IsDeliveryCompanyAccept == null).ToListAsync());

        //Delivery  Accept Or Refuse The Order 
        public async Task SetOrderStatus(Guid deliveryCompanyId, Guid orderId, bool isDeliveryAccept, decimal deliveryCost = 0)
        {
            if (!(IsCompetedDelivery(orderId)))
            {
                var deliveryOrder = _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId) && d.DeliveryCompanyId.Equals(deliveryCompanyId)).FirstOrDefault();
                deliveryOrder.IsDeliveryCompanyAccept = isDeliveryAccept;
                deliveryOrder.DeliveryCost = deliveryCost;
                await _context.SaveChangesAsync();
                if (isDeliveryAccept)
                {
                    var order = _context.Orders.FirstOrDefault(o => o.Id.Equals(orderId));
                    await _baseServices.SendMessage(order.PersonId is null ?
                                (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                                (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "Delivery Company Accepted You Order You Can Accept it Now");
                }
            }
            else
            {
                await _baseServices.SendMessage((await _baseServices.GetUserAccount((await _baseServices.GetEntity<DeliveryCompany>(deliveryCompanyId)).ApplicationUserId)).Email, $"This Is Competed Order");
            }

        }

        //Get Delivery Details For The Customer 
        public async Task<List<DeliveryDetails>> GetDeliveryDetails(Guid customerId, bool? iscustomerAccept)
        {
            var orders = await _context.Orders.Where(o => (o.PersonId != null ? o.PersonId.Equals(customerId) : o.BloodBankId.Equals(customerId)) && (!o.IsFree) && (o.StatusId == 3) && (o.WithDelivery)).ToListAsync();

            var deliveryDetails = new List<DeliveryDetails>();
            foreach (var order in orders)
            {
                var deliveryOrders = await _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(order.Id) && d.IsDeliveryCompanyAccept == true && d.IsCustomerAccept == iscustomerAccept).ToListAsync();
                if (deliveryOrders is null)
                    continue;
                foreach (var deliveryOrder in deliveryOrders)
                {
                    if(iscustomerAccept == null && IsCompetedDelivery(order.Id))
                    {
                        break;
                    }
                    var deliveryCompany = await _baseServices.GetEntity<DeliveryCompany>(deliveryOrder.DeliveryCompanyId);
                    deliveryDetails.Add(new DeliveryDetails
                    {
                        DeliveryCompanyId = deliveryOrder.DeliveryCompanyId,
                        OrderId = deliveryOrder.OrderId,
                        Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                        OrderDate = order.OrderDate,
                        OrderPrice = order.OrderPrice,
                        Quantity = order.Quantity,
                        CompanyName = deliveryCompany.DeliveryCompanyName,
                        Location = (await _baseServices.GetEntity<Governorate>(deliveryCompany.GovernorateId)).Name + "-" +
                        (await _baseServices.GetEntity<City>(deliveryCompany.CityId)).Name + "-" + deliveryCompany.Location,
                        DeliveryCost = deliveryOrder.DeliveryCost
                    });
                }
            }
            return deliveryDetails;
        }


        //Customer Accept Or Refuse The Comapany 
        public async Task SetDeliverySatuts(Guid deliveryId, Guid orderId, bool isCustomerAccept)
        {
            if (!( IsCompetedDelivery(orderId)))
            {
                var deliveryOrder =  _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId) && d.DeliveryCompanyId.Equals(deliveryId)).FirstOrDefault();
                deliveryOrder.IsCustomerAccept = isCustomerAccept;
                await _context.SaveChangesAsync();
                await _baseServices.SendMessage((await _baseServices.GetUserAccount((await _baseServices.GetEntity<DeliveryCompany>(deliveryOrder.DeliveryCompanyId)).ApplicationUserId)).Email, $"Customer {(isCustomerAccept ? "Accept" : "Refuse")}Your Deliver");
            }
            else
            {
                var order = await _baseServices.GetEntity<Order>(orderId);
                await _baseServices.SendMessage(order.PersonId is null ?
                            (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                            (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "This Order Has an Accepted Delivery Service");
            }
        }

        //Get Accepted Order Details 
        public async Task<List<OrderDetails>> GetAcceptedOrders(Guid deliveryComapnyId)
            => await GetValidOrders(await _context.DeliveryCompanyServices.Where(d => d.DeliveryCompanyId.Equals(deliveryComapnyId) && d.IsDeliveryCompanyAccept == true && d.IsCustomerAccept == true).ToListAsync());


        private async Task<List<OrderDetails>> GetValidOrders(List<DeliveryCompanyServices> services)
        {
            var ordersDetails = new List<OrderDetails>();
            foreach (var deliveryService in services)
            {
                var order = await _context.Orders.Where(o => o.Id.Equals(deliveryService.OrderId) && (o.StatusId == 3)).FirstOrDefaultAsync();

                if (order is null)
                    continue;

                var customer = "";
                if (order.PersonId is not null)
                {
                    var person = (await _baseServices.GetEntity<Person>((Guid)order.PersonId));
                    customer = person.FirstName + " " + person.LastName;
                }
                else
                {
                    var bloodbank = (await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId));
                    customer = bloodbank.BloodBankName;
                }

                ordersDetails.Add(new OrderDetails
                {
                    OrderId = deliveryService.OrderId,
                    Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                    CustomerName = customer,
                    CustomerPhoneNumber = order.PersonId is null ?
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).PhoneNumber :
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).PhoneNumber,
                    Location = (await _baseServices.GetEntity<Governorate>(order.GovernorateId)).Name
                    + "-" + (await _baseServices.GetEntity<City>(order.CityId)).Name + "-" + order.Location,
                    OrderDate = order.OrderDate,
                    RequiredDuration = order.RequiredDuration,
                    Quantity = order.Quantity,
                    OrderPrice = order.OrderPrice,
                    AcceptedQuantity = order.Quantity
                });
            }
            return ordersDetails;
        }

        public bool IsCompetedDelivery(Guid orderId)
            => (_context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId)).ToList()).Any(d => d.IsCustomerAccept == true && d.IsDeliveryCompanyAccept == true);
    
        public async Task<List<BloodBankOrderDetaills>> GetBloodBankOrdersDetails(Guid orderId)
        {
            var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsBloodBankAccept.Equals(true) && o.IsCustomerAccept.Equals(true)).ToListAsync();

            var bloodBanksOrderDetails = new List<BloodBankOrderDetaills>();
            foreach (var bloodBankOrder in bloodBankOrders)
            {
                var bloodBank = await _baseServices.GetEntity<BloodBank>(bloodBankOrder.BloodBankId);
                bloodBanksOrderDetails.Add(new BloodBankOrderDetaills
                {
                    BloodBankId = bloodBankOrder.BloodBankId,
                    BloodBankName = bloodBank.BloodBankName,
                    BloodBankLocation = (await _baseServices.GetEntity<Governorate>(bloodBank.GovernorateId)).Name + "-" +
                    (await _baseServices.GetEntity<City>(bloodBank.CityId)).Name + "-" + bloodBank.Location,
                    BloodPrice = (bloodBankOrder.Quanitty) * (bloodBankOrder.price),
                    Quantity = bloodBankOrder.Quanitty,
                    PhoneNumber = (await _baseServices.GetUserAccount(bloodBank.ApplicationUserId)).PhoneNumber
                });
            }

            return bloodBanksOrderDetails;
        }

       
    }
}
