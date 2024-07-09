using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Email;
using Ikagai.Services.BaseService;
using Ikagai.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Utilities.IO.Pem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ikagai.Services.OrderService
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices _baseServices;
        public OrderServices(ApplicationDbContext context, IBaseServices baseServices)
            => (_context, _baseServices) = (context, baseServices);

        public async Task CreateOrderAsync(OrderDto dto)
        {
            var order = await MapOrderDtoToOrder(dto);

            await _context.AddAsync(order);
            await _context.SaveChangesAsync();

            if (order.IsFree)
                await SaveFreeOrder(order);

            if (order.WithDelivery)
                await SendOrderToDeliveryCompanies(order);

            if (order.WithDelivery || !order.IsFree)
                await SendEmailToBloodBanks(await SendOrderToBloodBanks(order));
        }


        // Free And Paied Orders
        public async Task<List<AllOrders>> GetAllOrders(Guid customerId, bool isFree)
        {
            var orders = await _context.Orders.Where(o => (o.PersonId == null) ? o.BloodBankId.Equals(customerId) : o.PersonId.Equals(customerId) && o.IsFree.Equals(isFree)).ToListAsync();
            return await MapAllOrders(orders);
        }

        public async Task<List<DonorData>> GetDonorDetails(Guid orderId)
        {
            var donorOrders = await _context.DonorOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToListAsync();
            return await MapDonorData(GetPersonData(donorOrders));
        }

        public async Task<List<BloodBankData>> GetBloodBankData(Guid orderId)
        {
            var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId.Equals(orderId) && o.IsCustomerAccept == true).ToListAsync();
            return await MapBloodBankData(bloodBankOrders);
        }

        public async Task<List<DeliveryCompanyData>> GeteliveryData(Guid orderId)
        {
            var deliverservices = await _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId) && d.IsCustomerAccept == true).ToListAsync();
            return await MapToDeliveryData(deliverservices);
        }
        private async Task<List<DeliveryCompanyData>> MapToDeliveryData(List<DeliveryCompanyServices> services)
        {
            var deliveryData = new List<DeliveryCompanyData>();
            foreach (var service in services)
            {
                var company = await _context.DeliveryCompanies.FirstOrDefaultAsync(d => d.Id.Equals(service.DeliveryCompanyId));
                deliveryData.Add(new DeliveryCompanyData
                {
                    DeliveryCompanyId = service.DeliveryCompanyId,
                    City = (await _baseServices.GetEntity<City>(company.CityId)).Name,
                    Goveronate = (await _baseServices.GetEntity<Governorate>(company.GovernorateId)).Name,
                    Location = company.Location,
                    ClosedHour = company.ClosedHour,
                    OpenHour = company.OpenHour,
                    PhoneNumber = (await _baseServices.GetUserAccount(company.ApplicationUserId)).PhoneNumber,
                    DeliveryCompany = company.DeliveryCompanyName,
                    deliveryCost = service.DeliveryCost
                });
            }
            return deliveryData;
        }

        private async Task<List<BloodBankData>> MapBloodBankData( List<BloodBankOrder> bankOrders)
        {
            var banks = new List<BloodBankData>();
            foreach (var bankOrder in bankOrders)
            {
                var bank = await _context.BloodBanks.FirstOrDefaultAsync(b => b.Id.Equals(bankOrder.BloodBankId));

                banks.Add(new BloodBankData
                {
                    AcceptedQuantity = bankOrder.Quanitty,
                    BloodBank = bank.BloodBankName,
                    BloodBankId = bank.Id,
                    City = (await _baseServices.GetEntity<City>(bank.CityId)).Name,
                    Goveronate = (await _baseServices.GetEntity<Governorate>(bank.GovernorateId)).Name,
                    ClosedHour = bank.ClosedHour,
                    OpenHour = bank.OpenHour,
                    Location = bank.Location,
                    PhoneNumber = (await _baseServices.GetUserAccount(bank.ApplicationUserId)).PhoneNumber,
                    Price = (bankOrder.price) * bankOrder.Quanitty,
                });

            }
            return banks;
        }
        private List<Person> GetPersonData(List<DonorOrder> donorOrders)
        {
            var donors = new List<Person>();
            foreach (var donorOrder in donorOrders)
            {
                var person = _context.People.FirstOrDefault(p => p.Id.Equals(donorOrder.PersonId));
                donors.Add(person);
            }
            return donors;
        }
        private async Task<List<DonorData>> MapDonorData(List<Person> people)
        {
            var donors = new List<DonorData>();
            foreach (var person in people)
            {
                donors.Add(new DonorData
                {
                    BloodType = (await _baseServices.GetEntity<BloodAndDerivatives>(person.BloodAndDerivativesId)).Name,
                    City = (await _baseServices.GetEntity<City>(person.CityId)).Name,
                    Governorate = (await _baseServices.GetEntity<Governorate>(person.GovernorateId)).Name,
                    Location = person.Location,
                    Donor = person.FirstName + " " + person.LastName,
                    DonorId = person.Id,
                    PhoneNumber = (await _baseServices.GetUserAccount(person.ApplicationUserId)).PhoneNumber
                });
            }
            return donors;
        }
        private async Task<List<AllOrders>> MapAllOrders(List<Order> orders)
        {
            var allOrders = new List<AllOrders>();
            foreach (var order in orders)
            {
                allOrders.Add(new AllOrders
                {
                    Order = (await _baseServices.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId)).Name,
                    City = (await _baseServices.GetEntity<City>(order.CityId)).Name,
                    Governorate = (await _baseServices.GetEntity<Governorate>(order.GovernorateId)).Name,
                    Status = (await _baseServices.GetEntity<Status>(order.StatusId)).Name,
                    IsFree = order.IsFree ? "Free Order" : order.WithDelivery ? "Paied Order with Delivery" : "Paied Order",
                    Date = order.OrderDate,
                    Location = order.Location,
                    OrderId = order.Id,
                    Quantity = order.Quantity,
                    RequiredDuration = order.RequiredDuration,
                    TotalCost = order.OrderPrice,
                });
            }
            return allOrders;
        }

        private async Task<Order> MapOrderDtoToOrder(OrderDto dto)
            => new Order
            {
                PersonId = dto.PersonId,
                BloodBankId = dto.BloodBankId,
                BloodAndDerivativesId = dto.BloodAndDerivativesId,
                CityId = dto.CityId,
                GovernorateId = dto.GovernorateId,
                Location = dto.Location,
                IsFree = dto.IsFree,
                WithDelivery = dto.IsFree ? false : dto.WithDelivery,
                OrderPrice = 0,
                Quantity = dto.Quantity,
                RequiredDuration = dto.RequiredDuration,
                StatusId = 5,
            };


        #region Free Order
        private async Task<List<Person>> SendOrderToDonors(Order order)
        {
            var people = order.PersonId is null ?
                await _context.People.Where(p => ValidBlood(order.BloodAndDerivativesId).Contains(p.BloodAndDerivativesId)).ToListAsync() :
                await _context.People.Where(p => p.Id != order.PersonId && ValidBlood(order.BloodAndDerivativesId).Contains(p.BloodAndDerivativesId)).ToListAsync();

            people.ForEach(async p => await _context.DonorOrders.AddAsync(new DonorOrder { OrderId = order.Id, PersonId = p.Id }));

            await _context.SaveChangesAsync();

            return people;
        }
        private async Task SendEmailToDonors(List<Person> people)
            => people.ForEach(async p => await _baseServices.SendMessage
            ((await _baseServices.GetUserAccount(p.ApplicationUserId)).Email,
                "SomOne Needs Your heplp , Please Accept The Order If You Can !"));
        private async Task<ApplicationUser> GetCustomerAccount(Guid? personId, Guid? bloodBankId)
            => await _baseServices.GetUserAccount(personId is not null ?
                (await _baseServices.GetEntity<Person>((Guid)personId)).ApplicationUserId :
                (await _baseServices.GetEntity<BloodBank>((Guid)bloodBankId)).ApplicationUserId);
        private async Task SaveFreeOrder(Order order)
        {
            var people = await SendOrderToDonors(order);

            if (people is not null)
                await SendEmailToDonors(people);
            else
                await _baseServices.SendMessage((await GetCustomerAccount(order.PersonId, order.BloodBankId)).Email
                    , "Sorry , There is No Donor Can Help! but You still can ask bloodbanks by creating a paied Order which can help you more.");
        }
        private List<byte> ValidBlood(byte bloodId)
            => bloodId switch
            {
                1 => new List<byte> { 1, 2 },
                2 => new List<byte> { 2 },
                3 => new List<byte> { 1, 2, 3, 4 },
                4 => new List<byte> { 2, 4 },
                5 => new List<byte> { 1, 2, 5, 6 },
                6 => new List<byte> { 2, 6 },
                7 => new List<byte> { 1, 2, 3, 4, 5, 6, 7, 8 },
                8 => new List<byte> { 2, 4, 6, 8 },
            };

        #endregion

        #region PaiedOrder
        private async Task<List<BloodBank>> SendOrderToBloodBanks(Order order)
        {
            var bloodBanks = order.BloodBankId is not null ? await _context.BloodBanks.Where(b => b.Id != order.BloodBankId).ToListAsync()
                                                           : await _context.BloodBanks.ToListAsync();
            bloodBanks.ForEach(async b =>
            await _context.BloodBankOrders.AddAsync(new BloodBankOrder { OrderId = order.Id, BloodBankId = b.Id, price = 0, Quanitty = 0 }));

            await _context.SaveChangesAsync();

            return bloodBanks;
        }

        private async Task SendEmailToBloodBanks(List<BloodBank> bloodBanks)
          => bloodBanks.ForEach(async b => await _baseServices.SendMessage
          ((await _baseServices.GetUserAccount(b.ApplicationUserId)).Email,
              "SomOne Needs Your heplp , Please Accept The Order If You Can !"));

        private async Task SendOrderToDeliveryCompanies(Order order)
        {
            var deliveryCompanies = await _context.DeliveryCompanies.ToListAsync();
            deliveryCompanies.ForEach(async d => await _context.AddAsync(new DeliveryCompanyServices { DeliveryCompanyId = d.Id, OrderId = order.Id }));
            await _context.SaveChangesAsync();
        }
        #endregion




    }
}
