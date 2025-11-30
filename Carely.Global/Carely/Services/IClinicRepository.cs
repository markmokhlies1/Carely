using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IClinicRepository
    {
        Task AddClinicAsync(Clinic clinic);
        Task UpdateClinicAsync(Clinic clinic);
        Task DeleteClinicAsync(Clinic clinic);
        Task<Clinic?> GetClinicByIdAsync(int clinicId);
        Task<List<Clinic>> GetDoctorClinicsAsync(int doctorId);
    }
    public class ClinicRepository : IClinicRepository
    {
        private readonly AppDbContext _context;

        public ClinicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddClinicAsync(Clinic clinic)
        {
            await _context.Clinics.AddAsync(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateClinicAsync(Clinic clinic)
        {
            _context.Clinics.Update(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClinicAsync(Clinic clinic)
        {
            _context.Clinics.Remove(clinic);
            await _context.SaveChangesAsync();
        }

        public async Task<Clinic?> GetClinicByIdAsync(int clinicId)
        {
            return await _context.Clinics
                .Include(c => c.WorkTimes)
                .FirstOrDefaultAsync(c => c.Id == clinicId);
        }

        public async Task<List<Clinic>> GetDoctorClinicsAsync(int doctorId)
        {
            return await _context.Clinics
                .Where(c => c.DoctorId == doctorId)
                .Include(c => c.WorkTimes)
                .ToListAsync();
        }
    }

}
