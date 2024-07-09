using Ikagai.Core;
using Ikagai.Dtos;

namespace Ikagai.Services.AddressService
{
    public interface IAddressServices
    {
        Task<List<BaseDto>> GetGovernoratesAsync();
        Task<List<BaseDto>> GetCitiesAsync(byte governorateId);

    }
}
