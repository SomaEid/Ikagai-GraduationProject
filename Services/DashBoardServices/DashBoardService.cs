using Ikagai.Core;
using Ikagai.DashBoardDto;
using Ikagai.Services.BaseService;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ikagai.Services.DashBoardServices
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaseServices _baseService;
        public DashBoardService(ApplicationDbContext context, IBaseServices baseService) => (_context, _baseService) = (context, baseService);

        public async Task<long> GetUserCountAysnc() => await _context.Users.CountAsync();
        public async Task<List<UserDto>> GetUsersDetails()
        {
            var users = await _context.Users.ToListAsync();
            var usersDto = new List<UserDto>();
            foreach (var item in users)
            {
                var roleId = await _context.UserRoles.Where(u => u.UserId.Equals(item.Id)).FirstOrDefaultAsync();
                var role = await _context.Roles.Where(r => r.Id.Equals(roleId.RoleId)).FirstOrDefaultAsync();
                usersDto.Add(new UserDto { Id = item.Id, Email = item.Email, PhoneNumber = item.PhoneNumber, Role = role.Name, UserName = item.UserName });
            }
            return usersDto;
        }

        public async Task<long> GetCustomersCountAysnc() => await _context.People.CountAsync(p => p.RoleName == "Customer");
        public async Task<long> GetDonorsCountAysnc() => await _context.People.CountAsync(p => p.RoleName == "Donor");
        public async Task<long> GetPeopleCountAysnc() => await _context.People.CountAsync();
        public async Task<List<PersonDto>> GetPersonData(string role = null)
        {
            var people = await _context.People.Where(p => role == null ? p.RoleName != role : p.RoleName == role).ToListAsync();
            return await MapPerson(people);
        }
        public async Task<long> GetBloodBanksCountAysnc() => await _context.BloodBanks.CountAsync();
        public async Task<long> GetDeliveryCompaniesCountAysnc() => await _context.DeliveryCompanies.CountAsync();
        public async Task<long> GetOrdersCountAysnc() => await _context.Orders.CountAsync();
        public async Task<long> GetFreeOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree);
        public async Task<long> GetFreeAndUnCompletedOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree && o.StatusId != 3);
        public async Task<long> GetFreeAndCompletedOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree && o.StatusId == 3);
        public async Task<long> GetPaiedOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree == false);
        public async Task<long> GetPaiedAndCompletedOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree == false && o.StatusId == 3);
        public async Task<long> GetPaiedAndUnCompletedOrdersCountAysnc() => await _context.Orders.CountAsync(o => o.IsFree == false && o.StatusId != 3);

        public async Task<List<BloodBankDto>> GetBloodBankData()
        {
            var bloodBanks = await _context.BloodBanks.ToListAsync();
            return await MapBloodBank(bloodBanks);
        }
        public async Task<List<DeliveryCompanyDto>> GetDeliveryData()
        {
            var deliveryCompanies = await _context.DeliveryCompanies.ToListAsync();
            return await MapDelivery(deliveryCompanies);
        }

        public async Task<List<OrderDto>> GetOrderDetails()
        {
            var orders = await _context.Orders.ToListAsync();
            return await MapOrder(orders);
        }
        public async Task<List<OrderDto>> GetFreeOrderDetails()
            => await MapOrder(await _context.Orders.Where(o => o.IsFree).ToListAsync());

        public async Task<List<OrderDto>> GetFreeAndCompletedOrderDetails()
           => await MapOrder(await _context.Orders.Where(o => o.IsFree && o.StatusId == 3).ToListAsync());

        public async Task<List<OrderDto>> GetFreeAndUnCompletedOrderDetails()
           => await MapOrder(await _context.Orders.Where(o => o.IsFree && o.StatusId != 3).ToListAsync());

        public async Task<List<OrderDto>> GetPaiedOrderDetails()
            => await MapOrder(await _context.Orders.Where(o => o.IsFree == false).ToListAsync());

        public async Task<List<OrderDto>> GetPaiedAndCompletedOrderDetails()
          => await MapOrder(await _context.Orders.Where(o => o.IsFree == false && o.StatusId == 3).ToListAsync());
        public async Task<List<OrderDto>> GetPaiedAndUnCompletedOrderDetails()
         => await MapOrder(await _context.Orders.Where(o => o.IsFree == false && o.StatusId != 3).ToListAsync());


        public async Task<List<FreeOrder>> GetDonorsForFreeOrder(Guid orderId)
        {
            var DonorOrders = await _context.DonorOrders.Where(o => o.OrderId == orderId).ToListAsync();

            var freeOrders = new List<FreeOrder>();

            foreach (var donororder in DonorOrders)
            {
                var donor = await _context.People.Where(d => d.Id == donororder.PersonId).FirstOrDefaultAsync();
                freeOrders.Add(new FreeOrder
                {
                    DonorId = donororder.PersonId,
                    CustomerStatus = donororder.IsCustomerAccept == true ? "Accept" : donororder.IsCustomerAccept == false ? "Refused" : "Null",
                    Donor = donor.FirstName + " " + donor.LastName,
                    DonorStatus = donororder.IsDonorAccept == true ? "Accept" : donororder.IsDonorAccept == false ? "Refused" : "Null"
                });
            }

            return freeOrders;
        }

        public async Task<List<PaiedOrder>> GetBloodBanksForPaiedOrder(Guid orderId)
        {
            var BloodBankOrders = await _context.BloodBankOrders.Where(o => o.OrderId == orderId).ToListAsync();

            var paiedOrders = new List<PaiedOrder>();

            foreach (var bloodBankOrder in BloodBankOrders)
            {
                var bloodBank = await _context.BloodBanks.Where(d => d.Id == bloodBankOrder.BloodBankId).FirstOrDefaultAsync();
                paiedOrders.Add(new PaiedOrder
                {
                    BloodBankId = bloodBankOrder.BloodBankId,
                    CustomerStatus = bloodBankOrder.IsCustomerAccept == true ? "Accept" : bloodBankOrder.IsCustomerAccept == false ? "Refused" : "Null",
                    BloodBank = bloodBank.BloodBankName,
                    BloodBankStatus = bloodBankOrder.IsBloodBankAccept == true ? "Accept" : bloodBankOrder.IsBloodBankAccept == false ? "Refused" : "Null",
                    Price = bloodBankOrder.price,
                    Quantity = bloodBankOrder.Quanitty
                });
            }

            return paiedOrders;
        }

        public async Task<List<DeliveryCompanyData>> GetDeliveryDataForPaiedOrder(Guid orderId)
        {
            var deliverySerevices = await _context.DeliveryCompanyServices.Where(d => d.OrderId.Equals(orderId)).ToListAsync();
            var deliveryData = new List<DeliveryCompanyData>();
            if (deliverySerevices is not null)
            {
                foreach (var item in deliverySerevices)
                {
                    var delivery = await _context.DeliveryCompanies.Where(d => d.Id == item.DeliveryCompanyId).FirstOrDefaultAsync();
                    deliveryData.Add(new DeliveryCompanyData
                    {
                        DeliveryCompanyId = item.DeliveryCompanyId,
                        CompanyName = delivery.DeliveryCompanyName,
                        DeliveryCompanyStatus = item.IsDeliveryCompanyAccept == true ? "Accept" : item.IsDeliveryCompanyAccept == false ? "Refused" : "Null",
                        CustomerStatus = item.IsCustomerAccept == true ? "Accept" : item.IsCustomerAccept == false ? "Refused" : "Null",
                        DeliveryCost = item.DeliveryCost
                    });
                }
            }
            return deliveryData;
        }

        public async Task<long> GetDonationRequstsCountAysnc() => await _context.DonationRequests.CountAsync();

        public async Task<List<DonationRequestData>> GetDonationRequestsDetails()
        {
            var requests = await _context.DonationRequests.ToListAsync();
            var requestsData = new List<DonationRequestData>();
            foreach (var item in requests)
            {
                var donor = await _context.People.Where(p => p.Id == item.PersonId).FirstOrDefaultAsync();
                requestsData.Add(new DonationRequestData
                {
                    BloodBank = item.BloodBankId is null ? "Null" : (await _context.BloodBanks.Where(b => b.Id == item.BloodBankId).FirstOrDefaultAsync()).BloodBankName,
                    BloodType = (await _context.BloodAndDerivatives.Where(b => b.Id == donor.BloodAndDerivativesId).FirstOrDefaultAsync()).Name,
                    City = (await _context.Cities.Where(c => c.Id == item.CityId).FirstOrDefaultAsync()).Name,
                    Governorate = (await _context.Governorates.Where(c => c.Id == item.GovernorateId).FirstOrDefaultAsync()).Name,
                    Status = (await _context.Statuses.Where(c => c.Id == item.StatusId).FirstOrDefaultAsync()).Name,
                    RequestId = item.Id,
                    Donor = donor.FirstName + " " + donor.LastName,
                    LastDonationDate = item.LastDonationDate,
                    Location = item.Location,
                    RequestDate = item.RequestDate
                });
            }
            return requestsData;
        }
        public async Task<TestResultData> GetTestResultData(Guid DonationRequestId)
        {
            var result = await _context.TestResults.Where(r => r.DonationRequestId.Equals(DonationRequestId)).FirstOrDefaultAsync();
            if (result == null)
                return null;
            return new TestResultData
            {
                Comments = result.Comments,
                DonateAgain = result.DonateAgain,
                Id = result.Id,
                ResultDate = result.ResultDate,
                ResultImg = result.ResultImg
            };
        }

        // Get All Orders For a Person Or BlooBank 
        public async Task<List<OrderDto>> GetAllOrderForACustomer(Guid customerId)
            => await MapOrder(await _context.Orders.Where(o => o.PersonId != null ? o.PersonId.Equals(customerId) : o.BloodBankId.Equals(customerId)).ToListAsync());

        // Get All Donation Requests For A Person
        public async Task<List<DonationRequestData>> GetAllDonationRequestsForAPerson(Guid personId)
        {
            var requests = await _context.DonationRequests.Where(r => r.PersonId.Equals(personId)).ToListAsync();
            var requestsData = new List<DonationRequestData>();
            foreach (var item in requests)
            {
                var donor = await _context.People.Where(p => p.Id == item.PersonId).FirstOrDefaultAsync();
                requestsData.Add(new DonationRequestData
                {
                    BloodBank = item.BloodBankId is null ? "Null" : (await _context.BloodBanks.Where(b => b.Id == item.BloodBankId).FirstOrDefaultAsync()).BloodBankName,
                    BloodType = (await _context.BloodAndDerivatives.Where(b => b.Id == donor.BloodAndDerivativesId).FirstOrDefaultAsync()).Name,
                    City = (await _context.Cities.Where(c => c.Id == item.CityId).FirstOrDefaultAsync()).Name,
                    Governorate = (await _context.Governorates.Where(c => c.Id == item.GovernorateId).FirstOrDefaultAsync()).Name,
                    Status = (await _context.Statuses.Where(c => c.Id == item.StatusId).FirstOrDefaultAsync()).Name,
                    RequestId = item.Id,
                    Donor = donor.FirstName + " " + donor.LastName,
                    LastDonationDate = item.LastDonationDate,
                    Location = item.Location,
                    RequestDate = item.RequestDate
                });
            }
            return requestsData;

        }



        private async Task<List<OrderDto>> MapOrder(List<Order> orders)
        {
            var ordersdto = new List<OrderDto>();
            foreach (var order in orders)
            {
                string customerName = "";
                if (order.PersonId is not null)
                {
                    var person = await _context.People.Where(p => p.Id == order.PersonId).FirstOrDefaultAsync();
                    customerName = person.FirstName + " " + person.LastName;
                }
                else
                {
                    customerName = (await _context.BloodBanks.Where(b => b.Id == order.BloodBankId).FirstOrDefaultAsync()).BloodBankName;
                }


                var city = await _baseService.GetEntity<City>(order.CityId);
                var governorate = await _baseService.GetEntity<Governorate>(order.GovernorateId);
                var bloodType = await _baseService.GetEntity<BloodAndDerivatives>(order.BloodAndDerivativesId);
                var status = await _baseService.GetEntity<Status>(order.StatusId);

                ordersdto.Add(new OrderDto
                {
                    BloodAndDerivatives = bloodType.Name,
                    City = city.Name,
                    CustomerName = customerName,
                    Governorate = governorate.Name,
                    Id = order.Id,
                    IsFree = order.IsFree,
                    Location = order.Location,
                    OrderDate = order.OrderDate,
                    OrderPrice = order.OrderPrice,
                    Quantity = order.Quantity,
                    RequiredDuration = order.RequiredDuration,
                    Status = status.Name,
                    WithDelivery = order.WithDelivery
                });
            }
            return ordersdto;
        }
        private async Task<List<PersonDto>> MapPerson(List<Person> people)
        {
            List<PersonDto> personData = new List<PersonDto>();
            foreach (var person in people)
            {
                var city = await _baseService.GetEntity<City>(person.CityId);
                var governorate = await _baseService.GetEntity<Governorate>(person.GovernorateId);
                var bloodType = await _baseService.GetEntity<BloodAndDerivatives>(person.BloodAndDerivativesId);
                personData.Add(new PersonDto
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    BirthDate = person.BirthDate,
                    BloodType = bloodType.Name,
                    City = city.Name,
                    Governorate = governorate.Name,
                    Location = person.Location,
                    Gender = person.Gender == true ? "Male" : "Femal",
                    RoleName = person.RoleName,
                    NumberOfOrders = GetNumberOfOrders(person.Id),
                    NumberOfDonationRequests = GetNumberOfDonationRequest(person.Id)
                });
            }
            return personData;
        }

        private int GetNumberOfOrders(Guid customerId)
            => _context.Orders.Count(o => o.PersonId != null ? o.PersonId.Equals(customerId) : o.BloodBankId.Equals(customerId));

        private int GetNumberOfDonationRequest(Guid personId)
            => _context.DonationRequests.Count(d => d.PersonId.Equals(personId));

        private int GetNumberOfDonationRequestForBanks(Guid bloodBankId)
           => _context.DonationRequests.Count(d => d.BloodBankId.Equals(bloodBankId));

        private int GetNumberOfResivedOrdersForBanks(Guid bloodBankId)
          => _context.BloodBankOrders.Count(d => d.BloodBankId.Equals(bloodBankId));

        // Donation Requests Foreach BloodBank
        public async Task<List<DonationRequestData>> GetAllDonationRequestsForABloodBank(Guid bloodBankId)
        {
            var requests = await _context.DonationRequests.Where(r => r.BloodBankId.Equals(bloodBankId)).ToListAsync();
            var requestsData = new List<DonationRequestData>();
            foreach (var item in requests)
            {
                var donor = await _context.People.Where(p => p.Id == item.PersonId).FirstOrDefaultAsync();
                requestsData.Add(new DonationRequestData
                {
                    BloodBank = item.BloodBankId is null ? "Null" : (await _context.BloodBanks.Where(b => b.Id == item.BloodBankId).FirstOrDefaultAsync()).BloodBankName,
                    BloodType = (await _context.BloodAndDerivatives.Where(b => b.Id == donor.BloodAndDerivativesId).FirstOrDefaultAsync()).Name,
                    City = (await _context.Cities.Where(c => c.Id == item.CityId).FirstOrDefaultAsync()).Name,
                    Governorate = (await _context.Governorates.Where(c => c.Id == item.GovernorateId).FirstOrDefaultAsync()).Name,
                    Status = (await _context.Statuses.Where(c => c.Id == item.StatusId).FirstOrDefaultAsync()).Name,
                    RequestId = item.Id,
                    Donor = donor.FirstName + " " + donor.LastName,
                    LastDonationDate = item.LastDonationDate,
                    Location = item.Location,
                    RequestDate = item.RequestDate
                });
            }
            return requestsData;

        }

        // All Resived Orders Foreach BloodBank
        public async Task<List<OrderDto>> GetAllResivedOrdersForBloodBank(Guid BloodBankId)
        {
            var bloodBankOrders = await _context.BloodBankOrders.Where(o => o.BloodBankId.Equals(BloodBankId)).ToListAsync();
            var orders = new List<Order>();
            foreach (var bloodbanks in bloodBankOrders)
            {
                orders.Add(await _context.Orders.FirstOrDefaultAsync(o => o.Id.Equals(bloodbanks.OrderId)));
            }
           return await MapOrder(orders);
        }

        private async Task<List<BloodBankDto>> MapBloodBank(List<BloodBank> bloodBanks)
        {
            List<BloodBankDto> bloodBanksDto = new List<BloodBankDto>();
            foreach (var bloodBank in bloodBanks)
            {
                var city = await _baseService.GetEntity<City>(bloodBank.CityId);
                var governorate = await _baseService.GetEntity<Governorate>(bloodBank.GovernorateId);
                var status = await _baseService.GetEntity<Status>(bloodBank.StatusId);
                bloodBanksDto.Add(new BloodBankDto
                {
                    BloodBankName = bloodBank.BloodBankName,
                    City = city.Name,
                    Governorate = governorate.Name,
                    ClosedHour = bloodBank.ClosedHour,
                    FirstName = bloodBank.FirstName,
                    LastName = bloodBank.LastName,
                    Id = bloodBank.Id,
                    Location = bloodBank.Location,
                    OpenHour = bloodBank.OpenHour,
                    Status = status.Name,
                    NumberOfOrders = GetNumberOfOrders(bloodBank.Id),
                    NumberOfResivedDonationRequests = GetNumberOfDonationRequestForBanks(bloodBank.Id),
                    NumberOfResivedOrders = GetNumberOfResivedOrdersForBanks(bloodBank.Id)
                });
            }
            return bloodBanksDto;
        }

        private async Task<List<DeliveryCompanyDto>> MapDelivery(List<DeliveryCompany> deliveryCompanies)
        {
            List<DeliveryCompanyDto> deliverydto = new List<DeliveryCompanyDto>();
            foreach (var delivery in deliveryCompanies)
            {
                var city = await _baseService.GetEntity<City>(delivery.CityId);
                var governorate = await _baseService.GetEntity<Governorate>(delivery.GovernorateId);
                deliverydto.Add(new DeliveryCompanyDto
                {
                    DeliveryCompanyName = delivery.DeliveryCompanyName,
                    City = city.Name,
                    Governorate = governorate.Name,
                    ClosedHour = delivery.ClosedHour,
                    FirstName = delivery.FirstName,
                    LastName = delivery.LastName,
                    Id = delivery.Id,
                    Location = delivery.Location,
                    OpenHour = delivery.OpenHour,
                    CommercialRegister = delivery.CommercialRegister,
                    NumberOfRecievedOrders = GetDeliveryOrders(delivery.Id)
                });
            }
            return deliverydto;
        }

        private int GetDeliveryOrders(Guid deliveryId)
        {
            var deliveryServices = _context.DeliveryCompanyServices.Where(d => d.DeliveryCompanyId.Equals(deliveryId)).ToList();
            var orders = new List<Order>();
            foreach (var item in deliveryServices)
            {
                var order = _context.Orders.Where(o => o.Id.Equals(item.OrderId) && o.StatusId == 3).FirstOrDefault();
                if(order is not null)
                {
                    orders.Add(order);
                }
                
            }
            return orders.Count;
        }
    }
}
