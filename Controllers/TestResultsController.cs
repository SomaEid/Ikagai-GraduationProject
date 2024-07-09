using Ikagai.Dtos;
using Ikagai.Services.TestResultService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultServices _testResultServices;

        public TestResultsController(ITestResultServices testResultServices) => (_testResultServices) = (testResultServices);

        [HttpPost]
        [Route("AddTestResult")]
        public async Task<IActionResult> AddTestResult([FromForm] TestResultDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { status = false, Message = "Error Happen", Data = ModelState });

            return Ok(new { status = true, Message = "Success", Data = await _testResultServices.AddResult(dto) });
        }

        [HttpGet]
        [Route("DonorResults")]
        public async Task<IActionResult> DonotResults(Guid personId)
            => Ok(new { status = true, Message = "Success", Data = await _testResultServices.GetDonationResult(personId) });
    }
}
