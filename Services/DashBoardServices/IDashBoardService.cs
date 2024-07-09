using Ikagai.DashBoardDto;

namespace Ikagai.Services.DashBoardServices
{
    public interface IDashBoardService
    {
        Task<List<OrderDto>> GetAllOrderForACustomer(Guid customerId);
        Task<List<DonationRequestData>> GetAllDonationRequestsForABloodBank(Guid bloodBankId);
        Task<List<OrderDto>> GetAllResivedOrdersForBloodBank(Guid BloodBankId);
        Task<long> GetUserCountAysnc();
        Task<List<UserDto>> GetUsersDetails();
        Task<long> GetCustomersCountAysnc();
        Task<long> GetDonorsCountAysnc();
        Task<long> GetPeopleCountAysnc();
        Task<long> GetBloodBanksCountAysnc();
        Task<long> GetDeliveryCompaniesCountAysnc();
        Task<long> GetPaiedOrdersCountAysnc();
        Task<long> GetFreeOrdersCountAysnc();
        Task<long> GetOrdersCountAysnc();
        Task<long> GetDonationRequstsCountAysnc();
        Task<long> GetPaiedAndCompletedOrdersCountAysnc();
        Task<long> GetPaiedAndUnCompletedOrdersCountAysnc();
        Task<long> GetFreeAndUnCompletedOrdersCountAysnc();
        Task<long> GetFreeAndCompletedOrdersCountAysnc();
        Task<List<OrderDto>> GetFreeAndCompletedOrderDetails();
        Task<List<OrderDto>> GetFreeAndUnCompletedOrderDetails();
        Task<List<OrderDto>> GetPaiedAndCompletedOrderDetails();
        Task<List<OrderDto>> GetPaiedAndUnCompletedOrderDetails();
        Task<List<DeliveryCompanyDto>> GetDeliveryData();
        Task<List<BloodBankDto>> GetBloodBankData();
        Task<List<PersonDto>> GetPersonData(string role = null);
        Task<List<OrderDto>> GetOrderDetails();
        Task<List<OrderDto>> GetFreeOrderDetails();
        Task<List<OrderDto>> GetPaiedOrderDetails();
        Task<List<FreeOrder>> GetDonorsForFreeOrder(Guid orderId);
        Task<List<PaiedOrder>> GetBloodBanksForPaiedOrder(Guid orderId);
        Task<List<DeliveryCompanyData>> GetDeliveryDataForPaiedOrder(Guid orderId);
        Task<List<DonationRequestData>> GetDonationRequestsDetails();
        Task<TestResultData> GetTestResultData(Guid DonationRequestId);
        Task<List<DonationRequestData>> GetAllDonationRequestsForAPerson(Guid personId);
    }
}
