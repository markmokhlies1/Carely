using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IUserRepository
    {
        Task<Mother?> GetMotherByEmailAsync(string email);
        Task<Admin?> GetAdminByEmailAsync(string email);
        Task AddMotherAsync(Mother mother);
        Task<Mother?> GetMotherByIdAsync(int id);
        Task UpdateMotherAsync(Mother mother);
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Mother?> GetMotherByEmailAsync(string email)
            => await _context.Mothers.FirstOrDefaultAsync(m => m.Email == email);

        public async Task<Admin?> GetAdminByEmailAsync(string email)
            => await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);

        public async Task AddMotherAsync(Mother mother)
        {
            await _context.Mothers.AddAsync(mother);
            await _context.SaveChangesAsync();
        }
        public async Task<Mother?> GetMotherByIdAsync(int id)
        => await _context.Mothers.FirstOrDefaultAsync(m => m.Id == id);

        public async Task UpdateMotherAsync(Mother mother)
        {
            _context.Mothers.Update(mother);
            await _context.SaveChangesAsync();
        }
    }
}
