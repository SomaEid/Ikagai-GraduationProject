using Ikagai.Services.AddressService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressServices _addressServices;

        public AddressController(IAddressServices addressServices)
            => (_addressServices) = (addressServices);


        [HttpGet]
        [Route("GetGoveronrates")]
        public async Task<IActionResult> GetGoveronrates()
            => Ok(new { status = true, Message = "Success", Data = await _addressServices.GetGovernoratesAsync()});


        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities(byte governorateId)
           => Ok(new { status = true, Message = "Success", Data = await _addressServices.GetCitiesAsync(governorateId) });

    }
}
