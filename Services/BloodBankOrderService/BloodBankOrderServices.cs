using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Services.BaseService;
using Ikagai.Services.DeliveryComapnyService;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ikagai.Services.BloodBankOrderService
{
    public class BloodBankOrderServices : IBloodBankOrderServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices _baseServices;
        public BloodBankOrderServices(ApplicationDbContext context, IBaseServices baseServices)
            => (_context, _baseServices) = (context, baseServices);

        //Get New Orders For BloodBank
        public async Task<List<OrderDetails>> GetBloodBankNewOrders(Guid bloodBankId)
            => await GetValidOrders(await _context.BloodBankOrders.Where(d => d.BloodBankId == bloodBankId && d.IsBloodBankAccept == null).ToListAsync(), 3);

        //Accept Order Or Change Order Quantity The Accept  Or Refuse The Order
        public async Task SetOrderStatus(Guid bloodBankId, Guid orderId, bool isBloodBankAccept, decimal price = 0, int quantity = 0)
        {
            if (await IsOrderComplete(orderId) && isBloodBankAccept)
            {
                _baseServices.SendMessage((await _baseServices.GetUserAccount((await _baseServices.GetEntity<BloodBank>(bloodBankId)).ApplicationUserId)).Email, "This Order is Completed Order Thanks For Your Intersting");
                return;
            }

            var order = await _baseServices.GetEntity<Order>(orderId);
            var bloodBankOrder = await _context.BloodBankOrders.Where(o => o.BloodBankId.Equals(bloodBankId) && o.OrderId.Equals(orderId)).FirstOrDefaultAsync();
            bloodBankOrder.price = price;
            bloodBankOrder.IsBloodBankAccept = isBloodBankAccept;
            bloodBankOrder.Quanitty = !isBloodBankAccept ? 0 : (quantity == 0 ? order.Quantity : quantity);

            await _context.SaveChangesAsync();

            if (isBloodBankAccept)
                await _baseServices.SendMessage(order.PersonId is null ?
                            (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                            (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "SomeOne Accepted You Order You Can Accept Him Now");
        }

        // Get BloodBanks Whose Accepted The Customer Order (iscustomerAccept = null) || Get Accepted Orders For The Customer (iscustomerAccept = true)
        public async Task<List<BloodBankDetails>> GetBloodBanksDetails(Guid customerId, bool? iscustomerAccept)
        {
            var orders = await _context.Orders.Where(o => (o.PersonId != null ? o.PersonId.Equals(customerId) : o.BloodBankId.Equals(customerId)) && (!o.IsFree)).ToListAsync();
            var bloodBanksDetails = new List<BloodBankDetails>();

            foreach (var order in orders)
            {
                var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(order.Id) && (o.IsBloodBankAccept == true) && ((o.IsCustomerAccept == iscustomerAccept))).ToListAsync();
                foreach (var bloodBankOrder in bloodBankOrders)
                {
                    if(iscustomerAccept == null && await IsOrderComplete(order.Id))
                    {
                        break;
                    }
                    var bloodBank = await _baseServices.GetEntity<BloodBank>(bloodBankOrder.BloodBankId);
                    bloodBanksDetails.Add(new BloodBankDetails
                    {
                        BloodBankId = bloodBankOrder.BloodBankId,
                        BloodBankName = bloodBank.BloodBankName,
                        Location = (await _baseServices.GetEntity<Governorate>(bloodBank.GovernorateId)).Name + "-" + (await _baseServices.GetEntity<City>(bloodBank.CityId)).Name + "-" + bloodBank.Location,
                        Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                        OrderId = order.Id,
                        Price = bloodBankOrder.price,
                        Quantity = bloodBankOrder.Quanitty,
                        OrderDate = order.OrderDate
                    });
                }
            }
            return bloodBanksDetails;
        }


        //Customer Accept Or Refuse bloodBank Or Change BloodBank Quantity
        public async Task SetCustomerStatus(Guid bloodBankId, Guid orderId, bool isAccept, int quantity = 0)
        {
            if (await IsOrderComplete(orderId) && isAccept)
            {
                var order = await _baseServices.GetEntity<Order>(orderId);
                await _baseServices.SendMessage(order.PersonId is null ?
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "This Order is Completed Order Thanks For Your Intersting");

                return;
            }

            var bloodBankOrder = await _context.BloodBankOrders.Where(o => o.BloodBankId.Equals(bloodBankId) && o.OrderId.Equals(orderId)).FirstOrDefaultAsync();

            if (isAccept && quantity!=0)
            {
                var validquantity = await GetValidChangeAsync(orderId, bloodBankOrder.Quanitty , quantity);
                bloodBankOrder.Quanitty = validquantity;
            }
            else if(isAccept)
            {
                bloodBankOrder.Quanitty =await GetValidAccept(orderId, bloodBankOrder.Quanitty);
            }

            bloodBankOrder.IsCustomerAccept = isAccept;


            await _context.SaveChangesAsync();
            await IsOrderComplete(orderId);
            GetOrderPrice(orderId);
            if (isAccept)
                await _baseServices.SendMessage((await _baseServices.GetUserAccount((await _baseServices.GetEntity<BloodBank>(bloodBankId)).ApplicationUserId)).Email, $"{(quantity == 0 ? "Customer Accepted Your Help You Can See Him Now!" : $"Customer Change Required Quantity To {quantity}")}");
        }

        //Accepted Order Details For BloodBank 
        public async Task<List<OrderDetails>> GetAcceptedOrderDetails(Guid bloodBankId)
              => await GetValidOrders(await _context.BloodBankOrders.Where(d => (d.BloodBankId == bloodBankId) && (d.IsBloodBankAccept == true) && (d.IsCustomerAccept == true)).ToListAsync());

        public async Task<bool> IsValidQuantity(Guid orderId, int quantity)
            => (quantity <= 0 || quantity >= (await _baseServices.GetEntity<Order>(orderId)).Quantity);

        public async Task<DeliveryDetails>? GetDeliveryDetails(Guid orderId)
        {
            var order = await _baseServices.GetEntity<Order>(orderId);
            var deliveryOrder = await _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId) && d.IsDeliveryCompanyAccept.Equals(true) && d.IsCustomerAccept.Equals(true)).FirstOrDefaultAsync();
            if (deliveryOrder is not null)
            {
                var deliveryCompany = await _baseServices.GetEntity<DeliveryCompany>(deliveryOrder.DeliveryCompanyId);
                return new DeliveryDetails()
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
                };
            }
            return null;
        }
        private async Task<List<OrderDetails>> GetValidOrders(List<BloodBankOrder> bloodBankOrders, byte status = 0)
        {
            var ordersDetails = new List<OrderDetails>();
            foreach (var bloodBankOrder in bloodBankOrders)
            {
                var order = await _context.Orders.Where(o => o.Id.Equals(bloodBankOrder.OrderId) && o.StatusId != status).FirstOrDefaultAsync();


                if (order is null)
                    continue;

                var iscompletedOrder = await IsOrderComplete(order.Id);

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
                    OrderId = bloodBankOrder.OrderId,
                    Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                    CustomerName = customer,
                    CustomerPhoneNumber = order.PersonId is null ?
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).PhoneNumber :
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).PhoneNumber,
                    Location = (await _baseServices.GetEntity<Governorate>(order.GovernorateId)).Name
                    + "-" + (await _baseServices.GetEntity<City>(order.CityId)).Name + "-" + order.Location,
                    OrderDate = order.OrderDate,
                    RequiredDuration = order.RequiredDuration,
                    Quantity = status == 0 ? bloodBankOrder.Quanitty : order.Quantity,
                    AcceptedQuantity = bloodBankOrder.Quanitty,
                    OrderPrice = (bloodBankOrder.Quanitty)*(bloodBankOrder.price)
                });
            }
            return ordersDetails;
        }
        private async Task<bool> IsOrderComplete(Guid ordeId)
        {
            int quantity = 0;
            (await _context.BloodBankOrders.Where(o => o.OrderId.Equals(ordeId) && o.IsBloodBankAccept == true && o.IsCustomerAccept == true).ToListAsync()).ForEach(bo => quantity += bo.Quanitty);

            var order = await _baseServices.GetEntity<Order>(ordeId);
            var isComplete = quantity >= order.Quantity;

            if (isComplete)
            {
                order.StatusId = 3;
                await _context.SaveChangesAsync();
            }
            return isComplete;
        }

        //public async Task<int> Getvalidchange(Guid orderId, int newQuantity)
        //{
        //    int quantity = 0;
        //    var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToListAsync();
        //    bloodBankOrders.ForEach(bo => quantity += bo.Quanitty);

        //    var order = await _baseServices.GetEntity<Order>(orderId);
        //    var restQuantity = order.Quantity - quantity;

        //    return restQuantity > 0 ? newQuantity : restQuantity < 0 ? newQuantity + restQuantity : order.Quantity;
        //}

        public async Task<int> GetValidChangeAsync(Guid orderId , int bloodBankQuantity ,  int newquantity)
        {
            int previousQuantity = 0;
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id.Equals(orderId));
            var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToListAsync();
            bloodBankOrders.ForEach(bo => previousQuantity += bo.Quanitty);

            if (previousQuantity == order.Quantity)
                return 0;


            int neededQuantity = order.Quantity - previousQuantity;
            return neededQuantity > 0 ? newquantity : neededQuantity < 0 ? newquantity + neededQuantity : order.Quantity;
        }
        public async Task<int> GetValidAccept(Guid orderId, int bloodBankQuantity)
        {
            int previousQuantity = 0;
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id.Equals(orderId));
            var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToListAsync();
            bloodBankOrders.ForEach(bo => previousQuantity += bo.Quanitty);

            if (previousQuantity == order.Quantity)
                return 0;


            int neededQuantity = order.Quantity - previousQuantity;

            if (neededQuantity >= bloodBankQuantity)
            {
                return bloodBankQuantity;
            }
            else if (neededQuantity < bloodBankQuantity)
            {
                return neededQuantity;
            }
            else
            {
                return 0;
            }
        }
        private void GetOrderPrice(Guid orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id.Equals(orderId));
            var bloodBankOrders = _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToList();

            decimal totalPrice = 0;

            foreach (var item in bloodBankOrders)
            {
                totalPrice += (item.price*item.Quanitty);
            }

            order.OrderPrice = totalPrice;
            _context.SaveChanges();
        }
    }
}
