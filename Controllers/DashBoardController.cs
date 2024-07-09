using Ikagai.Services.DashBoardServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _service;

        public DashBoardController(IDashBoardService service) => _service = service;

        #region GeneralData
        [HttpGet]
        [Route("GetUserCount")]
        public async Task<IActionResult> GetUserCount()
           => Ok(new { status = true, Message = "Success", data = await _service.GetUserCountAysnc() });

        [HttpGet]
        [Route("GetUsersDetails")]
        public async Task<IActionResult> GetUsersDetails()
           => Ok(new { status = true, Message = "Success", data = await _service.GetUsersDetails() });

        [HttpGet]
        [Route("GetDonorsCount")]
        public async Task<IActionResult> GetDonorsCount()
           => Ok(new { status = true, Message = "Success", data = await _service.GetDonorsCountAysnc() });

        [HttpGet]
        [Route("GetCustomersCount")]
        public async Task<IActionResult> GetCustomersCount()
           => Ok(new { status = true, Message = "Success", data = await _service.GetCustomersCountAysnc() });

        [HttpGet]
        [Route("GetPeopleCount")]
        public async Task<IActionResult> GetPeopleCount()
          => Ok(new { status = true, Message = "Success", data = await _service.GetPeopleCountAysnc() });

        [HttpGet]
        [Route("GetPeopleDetails")]
        public async Task<IActionResult> GetPeopleDetails()
         => Ok(new { status = true, Message = "Success", data = await _service.GetPersonData() });

        [HttpGet]
        [Route("GetDonorDetails")]
        public async Task<IActionResult> GetDonorDetails()
        => Ok(new { status = true, Message = "Success", data = await _service.GetPersonData("Donor") });
        [HttpGet]
        [Route("GetCustomerDetails")]
        public async Task<IActionResult> GetCustomerDetails()
        => Ok(new { status = true, Message = "Success", data = await _service.GetPersonData("Customer") });

        [HttpGet]
        [Route("GetBloodBankCount")]
        public async Task<IActionResult> GetBloodBankCount()
         => Ok(new { status = true, Message = "Success", data = await _service.GetBloodBanksCountAysnc() });

        [HttpGet]
        [Route("GetBloodBankDetails")]
        public async Task<IActionResult> GetBloodBankDetails()
      => Ok(new { status = true, Message = "Success", data = await _service.GetBloodBankData() });


        [HttpGet]
        [Route("GetDonationRequestForABloodBank")]
        public async Task<IActionResult> GetDonationRequestForABloodBank(Guid bloodBankId)
  => Ok(new { status = true, Message = "Success", data = await _service.GetAllDonationRequestsForABloodBank(bloodBankId) });

        [HttpGet]
        [Route("GetAllReceivedOrdersForBloodBank")]
        public async Task<IActionResult> GetAllReceivedOrdersForBloodBank(Guid bloodBankId)
 => Ok(new { status = true, Message = "Success", data = await _service.GetAllResivedOrdersForBloodBank(bloodBankId) });

        [HttpGet]
        [Route("GetAllOrdersForBloodBank")]
        public async Task<IActionResult> GetAllOrdersForBloodBank(Guid bloodBankId)
=> Ok(new { status = true, Message = "Success", data = await _service.GetAllOrderForACustomer(bloodBankId) });





        [HttpGet]
        [Route("GetDeliveryComapniesCount")]
        public async Task<IActionResult> GetDeliveryComapniesCount()
         => Ok(new { status = true, Message = "Success", data = await _service.GetDeliveryCompaniesCountAysnc() });
        [HttpGet]
        [Route("GetDeliveryComapniesDetails")]
        public async Task<IActionResult> GetDeliveryComapniesDetails()
         => Ok(new { status = true, Message = "Success", data = await _service.GetDeliveryData() });

        [HttpGet]
        [Route("GetAllOrdersForACustomer")]
        public async Task<IActionResult> GetAllOrdersForACustomer(Guid customerId)
       => Ok(new { status = true, Message = "Success", data = await _service.GetAllOrderForACustomer(customerId) });

        [HttpGet]
        [Route("GetAllDonationRequestsForAPerson")]
        public async Task<IActionResult> GetAllDonationRequestsForAPerson(Guid personId)
     => Ok(new { status = true, Message = "Success", data = await _service.GetAllDonationRequestsForAPerson(personId) });
        #endregion

        #region Order
        [HttpGet]
        [Route("GetOrdersCount")]
        public async Task<IActionResult> GetOrdersCount()
          => Ok(new { status = true, Message = "Success", data = await _service.GetOrdersCountAysnc() });

        [HttpGet]
        [Route("GetFreeOrdersCount")]
        public async Task<IActionResult> GetFreeOrdersCount()
        => Ok(new { status = true, Message = "Success", data = await _service.GetFreeOrdersCountAysnc() });

        [HttpGet]
        [Route("GetFreeAndCompletedOrdersCount")]
        public async Task<IActionResult> GetFreeAndCompletedOrdersCount()
       => Ok(new { status = true, Message = "Success", data = await _service.GetFreeAndCompletedOrdersCountAysnc() });

        [HttpGet]
        [Route("GetFreeAndUnCompletedOrdersCount")]
        public async Task<IActionResult> GetFreeAndUnCompletedOrdersCount()
      => Ok(new { status = true, Message = "Success", data = await _service.GetFreeAndUnCompletedOrdersCountAysnc() });


        [HttpGet]
        [Route("GetPaiedOrdersCount")]
        public async Task<IActionResult> GetPaiedOrdersCount()
        => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedOrdersCountAysnc() });

        [HttpGet]
        [Route("GetPaiedAndCompletedOrdersCount")]
        public async Task<IActionResult> GetPaiedAndCompletedOrdersCount()
      => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedAndCompletedOrdersCountAysnc() });

        [HttpGet]
        [Route("GetPaiedAndUnCompletedOrdersCount")]
        public async Task<IActionResult> GetPaiedAndUnCompletedOrdersCount()
     => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedAndUnCompletedOrdersCountAysnc() });

        [HttpGet]
        [Route("GetPaiedOrdersDetails")]
        public async Task<IActionResult> GetPaiedOrdersDetails()
         => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedOrderDetails() });

        [HttpGet]
        [Route("GetPaiedAndCompletedOrdersDetails")]
        public async Task<IActionResult> GetPaiedAndCompletedOrdersDetails()
       => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedAndCompletedOrderDetails() });

        [HttpGet]
        [Route("GetPaiedAndUnCompletedOrdersDetails")]
        public async Task<IActionResult> GetPaiedAndUnCompletedOrdersDetails()
      => Ok(new { status = true, Message = "Success", data = await _service.GetPaiedAndUnCompletedOrderDetails() });

        [HttpGet]
        [Route("GetFreeOrdersDetails")]
        public async Task<IActionResult> GetFreeOrdersDetails()
         => Ok(new { status = true, Message = "Success", data = await _service.GetFreeOrderDetails() });

        [HttpGet]
        [Route("GetFreeAndCompletedOrdersDetails")]
        public async Task<IActionResult> GetFreeAndCompletedOrdersDetails()
        => Ok(new { status = true, Message = "Success", data = await _service.GetFreeAndCompletedOrderDetails() });

        [HttpGet]
        [Route("GetFreeAndUnCompletedOrdersDetails")]
        public async Task<IActionResult> GetFreeAndUnCompletedOrdersDetails()
       => Ok(new { status = true, Message = "Success", data = await _service.GetFreeAndUnCompletedOrderDetails() });

        [HttpGet]
        [Route("GetAllOrdersDetails")]
        public async Task<IActionResult> GetAllOrdersDetails()
         => Ok(new { status = true, Message = "Success", data = await _service.GetOrderDetails() });


        [HttpGet]
        [Route("GetDonorsForFreeOrder")]
        public async Task<IActionResult> GetDonorsForFreeOrder(Guid orderId)
         => Ok(new { status = true, Message = "Success", data = await _service.GetDonorsForFreeOrder(orderId) });

        [HttpGet]
        [Route("GetBloodBanksForPaiedOrder")]
        public async Task<IActionResult> GetBloodBanksForPaiedOrder(Guid orderId)
        => Ok(new { status = true, Message = "Success", data = await _service.GetBloodBanksForPaiedOrder(orderId) });

        [HttpGet]
        [Route("GetDeliveryetailsForPaiedOrder")]
        public async Task<IActionResult> GetDeliveryetailsForPaiedOrder(Guid orderId)
      => Ok(new { status = true, Message = "Success", data = await _service.GetDeliveryDataForPaiedOrder(orderId) });

        #endregion

        #region DonationRequest

        [HttpGet]
        [Route("GetDonationRequestsCount")]
        public async Task<IActionResult> GetDonationRequestsCount()
           => Ok(new { status = true, Message = "Success", data = await _service.GetDonationRequstsCountAysnc() });

        [HttpGet]
        [Route("GetDonationRequestsData")]
        public async Task<IActionResult> GetDonationRequestsData()
         => Ok(new { status = true, Message = "Success", data = await _service.GetDonationRequestsDetails() });

        [HttpGet]
        [Route("GetTestResultForDonationRequest")]
        public async Task<IActionResult> GetTestResultForDonationRequest(Guid donationRequestId)
       => Ok(new { status = true, Message = "Success", data = await _service.GetTestResultData(donationRequestId) });


        #endregion
    }
}
