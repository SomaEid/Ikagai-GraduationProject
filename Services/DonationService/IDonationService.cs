using Ikagai.Dtos;

namespace Ikagai.Services.DonationService
{
    public interface IDonationService
    {
        Task<DonationResponce> CreateDonationRequestAsync(DonationRequestDto dto);
        Task<List<BloodBanksDonation>> GetBloodBanks(Guid requestId);
        Task ChooseBloodBank(Guid requestId, Guid bloodBankId);
        Task<List<BloodBankDonor>> GetBloodBankDonors(Guid bloodBankId);
    }
}
