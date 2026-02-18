using Carely.Data;
using Carely.Dtos.Requests.Clinic;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IClinicRepository
    {
        Task AddClinicAsync(Clinic clinic);
        Task UpdateClinicAsync(int clinicId, int doctorId, UpdateClinicRequest request);
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

        public async Task UpdateClinicAsync(int clinicId, int doctorId, UpdateClinicRequest request)
        {
            var clinic = await _context.Clinics
        .Include(c => c.WorkTimes)
        .FirstOrDefaultAsync(c => c.Id == clinicId);

            if (clinic == null)
                throw new KeyNotFoundException("Clinic not found");

            if (clinic.DoctorId != doctorId)
                throw new UnauthorizedAccessException();

            clinic.Name = request.Name ?? clinic.Name;
            clinic.Address = request.Address ?? clinic.Address;
            clinic.City = request.City ?? clinic.City;
            clinic.PhoneNumber = request.PhoneNumber ?? clinic.PhoneNumber;

            if (request.WorkTimes != null)
            {
                foreach (var updateWt in request.WorkTimes)
                {
                    var existingWt = clinic.WorkTimes
                        .FirstOrDefault(w => w.Day == updateWt.Day);

                    if (existingWt != null)
                    {
                        existingWt.From = updateWt.From ?? existingWt.From;
                        existingWt.To = updateWt.To ?? existingWt.To;
                    }
                    else
                    {
                        clinic.WorkTimes.Add(new ClinicWorkTime
                        {
                            Day = (DayOfWeek)updateWt.Day,
                            From = updateWt.From!.Value,
                            To = updateWt.To!.Value,
                            ClinicId = clinic.Id
                        });
                    }
                }

                var requestDays = request.WorkTimes.Select(w => w.Day).ToList();

                var toDelete = clinic.WorkTimes
                    .Where(w => !requestDays.Contains(w.Day))
                    .ToList();

                foreach (var wt in toDelete)
                {
                    _context.ClinicWorkTimes.Remove(wt);
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}
