using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Services.BloodBankOrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodBankOrdersController : ControllerBase
    {
        private readonly IBloodBankOrderServices _services;

        public BloodBankOrdersController(IBloodBankOrderServices services) => _services = services;

        [HttpGet]
        [Route("GetBloodBankOrders")]
        public async Task<IActionResult> GetBloodBankOrders(Guid bloodBankId)
            => Ok(new { status = true, Message = "Success", Data = await _services.GetBloodBankNewOrders(bloodBankId) });

        [HttpPost]
        [Route("AcceptOrder")]
        public async Task<IActionResult> AcceptOrder(Guid bloodBankId, Guid orderId, decimal price)
        {
            try
            {
                await _services.SetOrderStatus(bloodBankId, orderId, true, price);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened , Check Input Data Or Internet Connection" });
            }
        }
        [HttpPost]
        [Route("ChangeOrderQuantity")]
        public async Task<IActionResult> ChangeOrderQuantity(Guid bloodBankId, Guid orderId, decimal price, int quantity)
        {
            try
            {
                if (await _services.IsValidQuantity(orderId, quantity))
                    return BadRequest(new { status = false, Message = "Error Happen , 0 < Quantity < Order Quantity" });

                await _services.SetOrderStatus(bloodBankId, orderId, true, price, quantity);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened , Check Input Data Or Internet Connection" });
            }
        }

        [HttpPost]
        [Route("RefuseOrder")]
        public async Task<IActionResult> RefuseOrder(Guid bloodBankId, Guid orderId)
        {
            await _services.SetOrderStatus(bloodBankId, orderId, false);
            return Ok(new { status = true, Message = $"Success" });
        }

        [HttpGet]
        [Route("GetBloodBankDetailsForTheCustomer")]
        public async Task<IActionResult> GetBloodBankDetailsForTheCustomer(Guid customerId)
          => Ok(new { status = true, Message = "Success", Data = await _services.GetBloodBanksDetails(customerId, null) });

        [HttpPost]
        [Route("AccpetBloodBank")]
        public async Task<IActionResult> AccpetBloodBank(Guid bloodBankId, Guid orderId)
        {
            await _services.SetCustomerStatus(bloodBankId, orderId, true);
            return Ok(new { status = true, Message = $"Success" });
        }

        [HttpPost]
        [Route("RefuseBloodBank")]
        public async Task<IActionResult> RefuseBloodBank(Guid bloodBankId, Guid orderId)
        {
            await _services.SetCustomerStatus(bloodBankId, orderId, false);
            return Ok(new { status = true, Message = $"Success" });
        }

        [HttpPost]
        [Route("ChangeOrderQuantityByCustomer")]
        public async Task<IActionResult> ChangeOrderQuantityByCustomer(Guid bloodBankId, Guid orderId, int quantity)
        {
            try
            {
                if (await _services.IsValidQuantity(orderId, quantity))
                    return BadRequest(new { status = false, Message = "Error Happen , 0 < Quantity < Order Quantity" });

                await _services.SetCustomerStatus(bloodBankId, orderId, true, quantity);
                return Ok(new { status = true, Message = "Success" });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened , Check Input Data Or Internet Connection" });
            }
        }


        [HttpGet]
        [Route("GetAcceptedOrderDetailsForBloodBank")]
        public async Task<IActionResult> GetAcceptedOrderDetailsForBloodBank(Guid bloodBankId)
            => Ok(new { status = true, Message = "Success", Data = await _services.GetAcceptedOrderDetails(bloodBankId) });

        [HttpGet]
        [Route("GetAcceptedOrderDetailsForCustomer")]
        public async Task<IActionResult> GetAcceptedOrderDetailsForCustomer(Guid customerId)
           => Ok(new { status = true, Message = "Success", Data = await _services.GetBloodBanksDetails(customerId, true) });

        [HttpGet]
        [Route("GetDeliveryCompanyData")]
        public async Task<IActionResult> GetDeliveryCompanyData(Guid orderId)
        {
            var data = await _services.GetDeliveryDetails(orderId);
            if (data is not null)
                return Ok(new { status = true, Message = "Success", Date = data });
            else
                return Ok(new { status = true, Message = "Success", Date = "No Delivery" });
        }
           

    }
}
