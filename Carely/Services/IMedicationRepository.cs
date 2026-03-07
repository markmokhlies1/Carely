using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IMedicationRepository
    {
        Task<Medication> AddAsync(Medication medication);
        Task<Medication?> GetByIdAsync(int id);
        Task<IEnumerable<Medication>> GetAllForMotherAsync(int motherId);
        Task UpdateAsync(Medication medication);
        Task DeleteAsync(int id);
        Task<int> GetCountForMotherAsync(int motherId);
    }
    public class MedicationRepository : IMedicationRepository
    { 
        private readonly AppDbContext _context;

        public MedicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Medication> AddAsync(Medication medication)
        {
            await _context.Medications.AddAsync(medication);
            await _context.SaveChangesAsync();
            return medication;
        }

        public async Task<Medication?> GetByIdAsync(int id)
        {
            return await _context.Medications
                .Include(m => m.Mother)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Medication>> GetAllForMotherAsync(int motherId)
        {
            return await _context.Medications
                .Where(m => m.MotherId == motherId)
                .Include(m => m.Mother)
                .ToListAsync();
        }


        public async Task UpdateAsync(Medication medication)
        {
            _context.Medications.Update(medication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCountForMotherAsync(int motherId)
        {
            return await _context.Medications.CountAsync(m => m.MotherId == motherId);
        }
    }

}
