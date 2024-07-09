using Ikagai.Core;
using Ikagai.Dtos;

namespace Ikagai.Services.BaseService
{
    public interface IBaseServices
    {
        Task<T> GetEntity<T>(Guid id) where T : class;
        Task<T> GetEntity<T>(byte id) where T : class;
        Task SendMessage(string email, string content);
        Task<ApplicationUser> GetUserAccount(string id);
    }
}
