using Ikagai.Core;
using Ikagai.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Services.AddressService
{
    public class AddressServices : IAddressServices
    {
        private readonly ApplicationDbContext _context;
        public AddressServices(ApplicationDbContext context) 
            => (_context) = (context);

        public async Task<List<BaseDto>> GetGovernoratesAsync() 
            => await _context.Governorates.Select(g => new BaseDto { Id = g.Id, Name = g.Name }).ToListAsync();

        public async Task<List<BaseDto>> GetCitiesAsync(byte governorateId)
          => await _context.Cities.Where(c => c.GovernorateId == governorateId).Select(c => new BaseDto { Id = c.Id, Name = c.Name }).ToListAsync();

     
    }
}
