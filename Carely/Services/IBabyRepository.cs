using Carely.Models;
using Carely.Data;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IBabyRepository
    {
        Task<Baby> AddAsync(Baby baby);
        Task UpdateAsync(Baby baby);

        Task DeleteAsync(int id);

        Task<IEnumerable<Baby>> GetAllForMotherAsync(int motherId);
        Task<Baby?> GetByIdAsync(int id);
    }


    public class BabyRepository : IBabyRepository
    {
        private readonly AppDbContext _context;

        public BabyRepository(AppDbContext context) {
            _context = context;
        }



        public async Task<Baby> AddAsync(Baby baby)
        {
          
            await _context.Babies.AddAsync(baby);
            await _context.SaveChangesAsync();
            return baby;
        }


        public async Task UpdateAsync(Baby baby)
        {
            _context.Babies.Update(baby);
            await _context.SaveChangesAsync();

        }
        public async Task DeleteAsync(int id)
        {
            var baby = await _context.Babies.FindAsync(id);
            if (baby != null) {
                _context.Babies.Remove(baby);
                await _context.SaveChangesAsync();

            }

        }
        public async Task<Baby?> GetByIdAsync(int id)
        {
            return await _context.Babies
                .Include(m => m.Mother)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IEnumerable<Baby>> GetAllForMotherAsync(int motherId)
        {
            return await _context.Babies
                .Where(m => m.MotherId == motherId)
                .Include(m => m.Mother)
                .ToListAsync();
        }




    }
}
