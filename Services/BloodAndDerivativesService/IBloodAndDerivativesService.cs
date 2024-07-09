using Ikagai.Dtos;

namespace Ikagai.Services.BloodAndDerivativesService
{
    public interface IBloodAndDerivativesService
    {
        Task<List<BaseDto>> Derivatives();
        Task<List<BaseDto>> GetBlood();
    }
}
