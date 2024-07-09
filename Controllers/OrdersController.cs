using Ikagai.Dtos;
using Ikagai.Services.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices) => (_orderServices) = (orderServices);

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { status = false, Message = "Error Happen", Data = ModelState });
                }

                await _orderServices.CreateOrderAsync(dto);
                return Ok(new { stattus = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happen Check InPut Data Or Internet"});
            }
        }

        [HttpGet]
        [Route("GetAllFreeOrders")]
        public async Task<IActionResult> GetAllFreeOrders(Guid customerId)
        {
            return Ok(new { status = true, Message = "Success", Data =await _orderServices.GetAllOrders(customerId , true) });
        }

        [HttpGet]
        [Route("GetAllPaiedOrders")]
        public async Task<IActionResult> GetAllPaiedOrders(Guid customerId)
        {
            return Ok(new { status = true, Message = "Success", Data = await _orderServices.GetAllOrders(customerId, false) });
        }

        [HttpGet]
        [Route("GetDonorData")]
        public async Task<IActionResult> GetDonorData(Guid orderId)
        {
            return Ok(new { status = true, Message = "Success", Data = await _orderServices.GetDonorDetails(orderId) });
        }

        [HttpGet]
        [Route("GetBloodBankData")]
        public async Task<IActionResult> GetBloodBankData(Guid orderId)
        {
            return Ok(new { status = true, Message = "Success", Data = await _orderServices.GetBloodBankData(orderId) });
        }

        [HttpGet]
        [Route("GetDeliveryData")]
        public async Task<IActionResult> GetDeliveryData(Guid orderId)
        {
            return Ok(new { status = true, Message = "Success", Data = await _orderServices.GeteliveryData(orderId) });
        }
    }
}
