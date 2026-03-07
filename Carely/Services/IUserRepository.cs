using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IUserRepository
    {
        #region Mother
        Task<Mother?> GetMotherByEmailAsync(string email);
        Task<Mother?> GetMotherByIdAsync(int id);
        Task AddMotherAsync(Mother mother);
        Task UpdateMotherAsync(Mother mother);
        Task<List<Mother>?> GetMotherListAsync();
        public Task DeleteMotherAsync(Mother mother);
        Task<int> GetMothersCountAsync();

        #endregion

        #region Admin
        Task<Admin?> GetAdminByEmailAsync(string email);
        Task<Admin?> GetAdminByIdAsync(int userId);
        #endregion

        #region Doctor
        Task<Doctor?> GetDoctorByEmailAsync(string email);
        Task<Doctor?> GetDoctorByIdAsync(int id);
        Task AddDoctorAsync(Doctor doctor);
        Task UpdateDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetDoctorListAsync();
        Task DeleteDoctorAsync(Doctor doctor);
        Task<int> GetDoctorsCountAsync();

        #endregion
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Mother
        public async Task<Mother?> GetMotherByEmailAsync(string email)
            => await _context.Mothers.FirstOrDefaultAsync(m => m.Email == email);

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
        public async Task<List<Mother>?> GetMotherListAsync()
        {
            return await _context.Mothers.ToListAsync();
        }
        public async Task DeleteMotherAsync(Mother mother)
        {
            _context.Mothers.Remove(mother);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetMothersCountAsync()
        {
            return await _context.Mothers.CountAsync();
        }


        #endregion

        #region Admin
        public async Task<Admin?> GetAdminByEmailAsync(string email)
            => await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        public async Task<Admin?> GetAdminByIdAsync(int userId)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Id == userId);
        }
        #endregion

        #region Doctor
        public async Task<Doctor?> GetDoctorByEmailAsync(string email)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Doctor>> GetDoctorListAsync()
        {
            return await _context.Doctors.AsNoTracking().ToListAsync();
        }

        public async Task<int> GetDoctorsCountAsync()
        {
            return await _context.Doctors.CountAsync();
        }

        public async Task DeleteDoctorAsync(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        #endregion

    }

}
