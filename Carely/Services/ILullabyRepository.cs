using Carely.Models;
using Carely.Data;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface ILullabyRepository
    {
        Task<Lullaby> AddAsync(Lullaby lullaby);

        Task UpdateAsync(Lullaby lullaby);
        Task<Lullaby?> GetByIdAsync(int id);
        Task<IEnumerable<Lullaby>> GetAllAsync();
        Task<int> GetCountAsync();

        Task DeleteAsync(Lullaby lullaby);
    }

    public class LullabyRepository : ILullabyRepository
    {
        private readonly AppDbContext _context;

        public LullabyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Lullaby> AddAsync(Lullaby lullaby)
        {
            await _context.Lullabies.AddAsync(lullaby);
            await _context.SaveChangesAsync();
            return lullaby;
        }

        public async Task UpdateAsync(Lullaby lullaby)
        {
            _context.Lullabies.Update(lullaby);
            await _context.SaveChangesAsync();
        }

        public async Task<Lullaby?> GetByIdAsync(int id)
        {
            return await _context.Lullabies
                .Include(l => l.MotherUsages)
                .ThenInclude(mu => mu.Mother)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<IEnumerable<Lullaby>> GetAllAsync()
        {
            return await _context.Lullabies
                .Include(l => l.MotherUsages)
                .ThenInclude(mu => mu.Mother)
                .ToListAsync();


        }


        public async Task<int> GetCountAsync()
        {
            return await _context.Lullabies.CountAsync();
        }

        public async Task DeleteAsync(Lullaby lullaby)
        {
            _context.Lullabies.Remove(lullaby);
            await _context.SaveChangesAsync();
        }

    }
}
