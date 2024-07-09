using Ikagai.Services.DonorOrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorOrdersController : ControllerBase
    {
        private readonly IDonorOrderServices _services;

        public DonorOrdersController(IDonorOrderServices services) => _services = services;

        [HttpGet]
        [Route("GetDonorOrders")]
        public async Task<IActionResult> GetDonorOrders(Guid personId)
            => Ok(new { status = true, Message = "Success", Data = await _services.GetDonorOrders(personId) });

        [HttpPost]
        [Route("AcceptOrder")]
        public async Task<IActionResult> AcceptOrder(Guid personId, Guid orderId)
        {
            try
            {
                await _services.SetOrderStatus(orderId, personId, true);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened", Data = "Check The Internet Connection Or Input Data" });
            }
        }

        [HttpPost]
        [Route("RefuseOrder")]
        public async Task<IActionResult> RefuseOrder(Guid personId, Guid orderId)
        {
            try
            {
                await _services.SetOrderStatus(orderId, personId, false);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened", Data = "Check The Internet Connection Or Input Data" });
            }
        }

        [HttpGet]
        [Route("GetDonors")]
        public async Task<IActionResult> GetDonors(Guid customerId)
           => Ok(new { status = true, Message = "Success", Data = await _services.GetAcceptedDonors(customerId , null)});

        [HttpGet]
        [Route("GetAcceptedDonorsForCustomer")]
        public async Task<IActionResult> GetAcceptedDonorsForCustomer(Guid customerId)
          => Ok(new { status = true, Message = "Success", Data = await _services.GetAcceptedDonors(customerId, true) });

        [HttpPost]
        [Route("AcceptDonor")]
        public async Task<IActionResult> AcceptDonor(Guid donorId, Guid orderId)
        {
            try
            {
                await _services.SetCustomerOrderStatus(orderId, donorId, true);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened", Data = "Check The Internet Connection Or Input Data" });
            }
        }

        [HttpPost]
        [Route("RefuseDonor")]
        public async Task<IActionResult> RefuseDonor(Guid donorId, Guid orderId)
        {
            try
            {
                await _services.SetCustomerOrderStatus(orderId, donorId, false);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened", Data = "Check The Internet Connection Or Input Data" });
            }
        }

        [HttpGet]
        [Route("GetAcceptedOrdersForDonor")]
        public async Task<IActionResult> GetAcceptedOrdersForDonor(Guid donorId)
           => Ok(new { status = true, Message = "Success", Data = await _services.GetAcceptedOrders(donorId)});

        
    }
}
