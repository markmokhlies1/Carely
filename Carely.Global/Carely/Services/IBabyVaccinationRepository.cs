using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IBabyVaccinationRepository
    {
        Task<BabyVaccination?> GetByBabyAndVaccinationAsync(int babyId, int vaccinationId);
        Task AddAsync(BabyVaccination record);
        Task DeleteAsync(BabyVaccination record);

        Task <IEnumerable<BabyVaccination>> GetAllForBabyAsync(int babyId);
    }

    public class BabyVaccinationRepository : IBabyVaccinationRepository
    {
        private readonly AppDbContext _context;

        public BabyVaccinationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BabyVaccination?> GetByBabyAndVaccinationAsync(int babyId , int vaccinationId)
        {
            return await _context.BabyVaccinations.FirstOrDefaultAsync(bv => bv.BabyId == babyId && bv.VaccinationId == vaccinationId);
        }

        public async Task AddAsync(BabyVaccination record)
        {
            await _context.BabyVaccinations.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BabyVaccination record)
        {
            _context.BabyVaccinations.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<BabyVaccination>> GetAllForBabyAsync(int babyId)
        {
            return await _context.BabyVaccinations
                .Where(bv => bv.BabyId == babyId)
                .ToListAsync();
        }

    }
}