using Ikagai.Services.DeliveryComapnyService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryCompanyServicesController : ControllerBase
    {
        private readonly IDeliveryOrdersService _deliveryOrdersService;

        public DeliveryCompanyServicesController(IDeliveryOrdersService deliveryOrdersService) => _deliveryOrdersService = deliveryOrdersService;

        [HttpGet]
        [Route("GetDeliveryOrders")]
        public async Task<IActionResult> GetDeliveryOrders(Guid deliveryCompanyId)
            => Ok(new { status = true, Message = "Success", Data = await _deliveryOrdersService.GetOrderDetails(deliveryCompanyId) });

        [HttpPost]
        [Route("AcceptOrder")]
        public async Task<IActionResult> AcceptOrder(Guid deliveryCompanyId, Guid orderId, decimal deliveryCost)
        {
            try
            {
                await _deliveryOrdersService.SetOrderStatus(deliveryCompanyId, orderId, true, deliveryCost);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened Check Internet Connection Or Input Data" });
            }
        }

        [HttpPost]
        [Route("RefuseOrder")]
        public async Task<IActionResult> RefuseOrder(Guid deliveryCompanyId, Guid orderId)
        {
            try
            {
                await _deliveryOrdersService.SetOrderStatus(deliveryCompanyId, orderId, false);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened Check Internet Connection Or Input Data" });
            }
        }

        [HttpGet]
        [Route("GetDeliveryServiceDetails")]
        public async Task<IActionResult> GetDeliveryServiceDetails(Guid customerId)
           => Ok(new { status = true, Message = "Success", Data = await _deliveryOrdersService.GetDeliveryDetails(customerId, null) });

        [HttpPost]
        [Route("AcceptDeliveryComapny")]
        public async Task<IActionResult> AcceptDeliveryComapny(Guid deliveryCompanyId, Guid orderId)
        {
            try
            {
                await _deliveryOrdersService.SetDeliverySatuts(deliveryCompanyId, orderId, true);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened Check Internet Connection Or Input Data" });
            }
        }

        [HttpPost]
        [Route("RefuseDeliveryComapny")]
        public async Task<IActionResult> RefuseDeliveryComapny(Guid deliveryCompanyId, Guid orderId)
        {
            try
            {
                await _deliveryOrdersService.SetDeliverySatuts(deliveryCompanyId, orderId, false);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened Check Internet Connection Or Input Data" });
            }
        }

        [HttpGet]
        [Route("GetAcceptedOrderDetails")]
        public async Task<IActionResult> GetOrderDetails(Guid deliveryCompanyId)
          => Ok(new { status = true, Message = "Success", Data = await _deliveryOrdersService.GetAcceptedOrders(deliveryCompanyId) });

        [HttpGet]
        [Route("GetAcceptedDeliveryServiceDetails")]
        public async Task<IActionResult> GetAcceptedDeliveryServiceDetails(Guid customerId)
           => Ok(new { status = true, Message = "Success", Data = await _deliveryOrdersService.GetDeliveryDetails(customerId, true) });

        [HttpGet]
        [Route("GetBloodBanksData")]
        public async Task<IActionResult> GetBloodBanksData(Guid orderId)
            => Ok(new { status = true, Message = "Success", Data = await _deliveryOrdersService.GetBloodBankOrdersDetails(orderId) });
    }
}
