using Ikagai.Email;

namespace Ikagai.Services.EmailService
{
    public interface IEmailServices
    {
        Task SendEmailAsync(Message message);
    }
}
