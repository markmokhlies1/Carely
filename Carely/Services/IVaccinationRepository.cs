using Carely.Models;
using Carely.Data;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IVaccinationRepository
    {
        Task<IEnumerable<Vaccination>> GetAllAsync();
        Task<Vaccination?> GetByIdAsync(int id);
    }
    public class VaccinationRepository : IVaccinationRepository
    {
        private readonly AppDbContext _context;

        public VaccinationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Vaccination>> GetAllAsync()
        {

            return await _context.Vaccinations
                  .Include(v => v.VaccinationUsage)
                  .ToListAsync();
        }
        public async Task<Vaccination?> GetByIdAsync(int id)
        {
            return await _context.Vaccinations.FindAsync(id);
        }

    }

}
