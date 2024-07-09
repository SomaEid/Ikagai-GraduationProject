using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Email;
using Ikagai.Services.EmailService;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Services.BaseService
{
    public class BaseServices : IBaseServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailServices _emailServices;
        public BaseServices(ApplicationDbContext context, IEmailServices emailServices)
            => (_context , _emailServices) = (context , emailServices);

        public async Task<T> GetEntity<T>(byte id) where T : class
            => await _context.FindAsync<T>(id);

        public async Task<T> GetEntity<T>(Guid id) where T : class
            => await _context.FindAsync<T>(id);

        public async Task SendMessage(string email, string content) 
            => await _emailServices.SendEmailAsync(new Message(new string[] { email }, "Ikigai", content));

        public async Task<ApplicationUser> GetUserAccount(string id)
            => await _context.Users.FindAsync(id);

    }
}
