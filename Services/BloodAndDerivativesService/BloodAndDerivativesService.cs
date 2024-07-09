using Ikagai.Core;
using Ikagai.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Services.BloodAndDerivativesService
{
    public class BloodAndDerivativesService : IBloodAndDerivativesService
    {
        private readonly ApplicationDbContext _context;

        public BloodAndDerivativesService(ApplicationDbContext context)
            => (_context) = (context);


        public async Task<List<BaseDto>> Derivatives()
            => await _context.BloodAndDerivatives.Skip(8).Select(b => new BaseDto { Id = b.Id, Name = b.Name }).ToListAsync();

        public async Task<List<BaseDto>> GetBlood()
           => await _context.BloodAndDerivatives.Take(8).Select(b => new BaseDto { Id = b.Id, Name = b.Name }).ToListAsync();


    }
}
