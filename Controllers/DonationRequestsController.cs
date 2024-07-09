using Ikagai.Dtos;
using Ikagai.Services.DonationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ikagai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationRequestsController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationRequestsController(IDonationService donationService) => (_donationService) = (donationService);

        [HttpPost]
        [Route("CreateDonationRequest")]
        public async Task<IActionResult> CreateDonationRequest(DonationRequestDto dto)
        {
            try
            {
                var Result = await _donationService.CreateDonationRequestAsync(dto);
                var test = Result.RequestStatus == 4 ? "Donation Refused" : Result.RequestStatus == 8 ? "Your Donation Is Delayed" : "Success";

                return Ok(new { status = true, Message = test, Data = Result });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happened", Date = ModelState });
            }
        }



        [HttpGet]
        [Route("GetBloodBanks")]
        public async Task<IActionResult> GetBloodBanks(Guid donationRequestId)
            => Ok(new { status = true, Message = "Success", Data = await _donationService.GetBloodBanks(donationRequestId) });

        [HttpGet]
        [Route("GetBloodBankDonors")]
        public async Task<IActionResult> GetBloodBankDonors(Guid bloodBankId)
           => Ok(new { status = true, Message = "Success", Data = await _donationService.GetBloodBankDonors(bloodBankId) });

        [HttpPost]
        [Route("ChooseBloodBank")]
        public async Task<IActionResult> ChooseBloodBank(Guid donationRequestId, Guid bloodBankId)
        {
            try
            {
                await _donationService.ChooseBloodBank(donationRequestId, bloodBankId);
                return Ok(new { status = true, Message = "Success", });
            }
            catch
            {
                return BadRequest(new { status = false, Message = "Error Happen Check Internet Or Input Data" });
            }


        }
    }
}
