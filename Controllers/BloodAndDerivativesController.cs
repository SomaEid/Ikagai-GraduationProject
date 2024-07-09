using Ikagai.Services.BloodAndDerivativesService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodAndDerivativesController : ControllerBase
    {
        private readonly IBloodAndDerivativesService _bloodAndDerivativesService;

        public BloodAndDerivativesController(IBloodAndDerivativesService bloodAndDerivativesService)
            => (_bloodAndDerivativesService) = (bloodAndDerivativesService);

        [HttpGet]
        [Route("GetDerivatives")]
        public async Task<IActionResult> GetDerivatives()
            => Ok(new {status = true , Message = "Success" , Data = await _bloodAndDerivativesService.Derivatives()});

        [HttpGet]
        [Route("GetBlood")]
        public async Task<IActionResult> GetBlood()
            => Ok(new { status = true, Message = "Success", Data = await _bloodAndDerivativesService.GetBlood()});
    }
}
