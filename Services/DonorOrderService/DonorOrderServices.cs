using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Services.BaseService;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Services.DonorOrderService
{
    public class DonorOrderServices : IDonorOrderServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices _baseServices;

        public DonorOrderServices(ApplicationDbContext context, IBaseServices baseServices)
            => (_context, _baseServices) = (context, baseServices);

        //Not Accepted And Not Refused Orders For A Donor
        public async Task<List<OrderDetails>> GetDonorOrders(Guid donorId)
            => await GetValidOrders(await _context.DonorOrders.Where(d => d.PersonId == donorId && d.IsDonorAccept == null).ToListAsync(), 3);

        //Donor Accept Or Refuse An Order
        public async Task SetOrderStatus(Guid orderId, Guid personId, bool isAccept)
        {
            if (!IsOrderCompleted(orderId))
            {
                (await _context.DonorOrders.Where(o => o.OrderId == orderId && o.PersonId == personId).FirstOrDefaultAsync()).IsDonorAccept = isAccept;
                await _context.SaveChangesAsync();
                if (isAccept)
                {
                    var order = await _baseServices.GetEntity<Order>(orderId);
                    await _baseServices.SendMessage(order.PersonId is null ?
                        (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                        (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "SomeOne Accepted You Order You Can Accept Him Now");
                }
            }
            else
            {
                await _baseServices.SendMessage(
                      (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>(personId)).ApplicationUserId))).Email, "This Order Was Completed Thanks For Your Intersting");
            }

        }

        //Donors Whose accepted Orders Or Donors Accepted From Customer
        public async Task<List<DonorDetails>> GetAcceptedDonors(Guid customerId, bool? isCustomerAccept)
        {
            var orders = await _context.Orders.Where(o => (o.PersonId != null ? o.PersonId.Equals(customerId) : o.BloodBankId.Equals(customerId)) && o.IsFree ).ToListAsync();
          
            var donors = new List<DonorDetails>();
            foreach (var order in orders)
            {
                var donorOrders = await _context.DonorOrders.Where(o => o.OrderId.Equals(order.Id) && (o.IsDonorAccept == true) && (o.IsCustomerAccept == isCustomerAccept)).ToListAsync();

                foreach (var donorOrder in donorOrders)
                {
                    if (isCustomerAccept == null && IsOrderCompleted(order.Id))
                    {
                        break;
                    }
                    var person = await _baseServices.GetEntity<Person>(donorOrder.PersonId);
                    donors.Add(new DonorDetails
                    {
                        DonorId = person.Id,
                        Name = person.FirstName + " " + person.LastName,
                        Location = (await _baseServices.GetEntity<Governorate>(person.GovernorateId)).Name + "-" + (await _baseServices.GetEntity<City>(person.CityId)).Name + "-" + person.Location,
                        BloodType = (await _baseServices.GetEntity<BloodAndDerivatives>(person.BloodAndDerivativesId)).Name,
                        Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                        OrderDate = order.OrderDate,
                        PhoneNumber = (await _baseServices.GetUserAccount(person.ApplicationUserId)).PhoneNumber,
                        OrderId = donorOrder.OrderId
                    });
                }
            }
            return donors;
        }

        //Customer Accept Or Refuse A Donor
        public async Task SetCustomerOrderStatus(Guid orderId, Guid personId, bool isAccept)
        {
            var order = await _baseServices.GetEntity<Order>(orderId);
            if (IsOrderCompleted(orderId))
            {  
                await _baseServices.SendMessage(order.PersonId is null ?
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<BloodBank>((Guid)order.BloodBankId)).ApplicationUserId))).Email :
                    (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>((Guid)order.PersonId)).ApplicationUserId))).Email, "You Get The Required Quantity");
            }
            else
            {
               ( await _context.DonorOrders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.PersonId == personId)).IsCustomerAccept = isAccept;

                order.StatusId = IsOrderCompleted(orderId) ?(byte) 3 : (byte) 5;
                await _context.SaveChangesAsync();

                if (isAccept)
                {
                    await _baseServices.SendMessage(
                   (await _baseServices.GetUserAccount(((await _baseServices.GetEntity<Person>(personId)).ApplicationUserId))).Email, "A Customer Accepted You Help You Can See details Now");
                }
            }
        }

        //Accepted Orders From Donor And Customer 
        public async Task<List<OrderDetails>> GetAcceptedOrders(Guid donorId)
            => await GetValidOrders(await _context.DonorOrders.Where(d => (d.PersonId == donorId) && (d.IsDonorAccept == true) && (d.IsCustomerAccept == true)).ToListAsync());

        private async Task<List<OrderDetails>> GetValidOrders(List<DonorOrder> donorOrders, byte status = 0)
        {
            var ordersDetails = new List<OrderDetails>();
            foreach (var donorOrder in donorOrders)
            {
                var order = await _context.Orders.Where(o => o.Id.Equals(donorOrder.OrderId) && o.StatusId != status).FirstOrDefaultAsync();

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
                    OrderId = donorOrder.OrderId,
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
                });
            }
            return ordersDetails;
        }

        private bool IsOrderCompleted(Guid orderId)
        {
            var donorOrders = _context.DonorOrders.Where(o => o.OrderId.Equals(orderId)).ToList();
            var order = _context.Orders.FirstOrDefault(o => o.Id.Equals(orderId));

            int count = donorOrders.Count(d => d.IsCustomerAccept is true);
            return count >= order.Quantity ? true : false;
        }
    }
}

