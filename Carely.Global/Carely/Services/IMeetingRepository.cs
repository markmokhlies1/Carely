using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IMeetingRepository
    {
        Task AddAsync(Meeting meeting);
        Task UpdateAsync(Meeting meeting);
        Task DeleteAsync(Meeting meeting);
        Task<Meeting?> GetByIdAsync(int id);
        Task<List<Meeting>> GetUpcomingAsync(); 
        Task<List<Meeting>> GetEndedAsync();
        Task<int> GetTotalMeetingsCountAsync();
        Task<int> GetUpcomingMeetingsCountAsync();
        Task<int> GetEndedMeetingsCountAsync();
        Task<bool> AddMotherToMeetingAsync(int meetingId, int motherId);
        Task<bool> RemoveMotherFromMeetingAsync(int meetingId, int motherId);
        Task<List<Mother>?> GetRegisteredMothersAsync(int meetingId);
        Task<int> GetRegisteredMotherCountAsync(int meetingId);
        Task<int> GetFeedbackCountAsync(int meetingId);

        Task<List<Meeting>> GetMotherUpcomingMeetingsAsync(int motherId);
        Task<List<Meeting>> GetMotherEndedMeetingsAsync(int motherId);
        Task<int> GetMotherUpcomingCountAsync(int motherId);
        Task<int> GetMotherEndedCountAsync(int motherId);

        Task<List<Meeting>> GetDoctorUpcomingMeetingsAsync(int doctorId);
        Task<List<Meeting>> GetDoctorEndedMeetingsAsync(int doctorId);
        Task<int> GetDoctorUpcomingCountAsync(int doctorId);
        Task<int> GetDoctorEndedCountAsync(int doctorId);


    }
    public class MeetingRepository : IMeetingRepository
    {
        private readonly AppDbContext _context;

        public MeetingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Meeting meeting)
        {
            _context.Meetings.Update(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Meeting meeting)
        {
            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
        }

        public async Task<Meeting?> GetByIdAsync(int id)
        {
            return await _context.Meetings
                .Include(m => m.Mothers)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Meeting>> GetUpcomingAsync()
        {
            return await _context.Meetings
                .Where(m => m.Date > DateTime.Now)
                .OrderBy(m => m.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Meeting>> GetEndedAsync()
        {
            return await _context.Meetings
                .Where(m => m.Date < DateTime.Now)
                .OrderByDescending(m => m.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> GetTotalMeetingsCountAsync()
        {
            return await _context.Meetings.CountAsync();
        }

        public async Task<int> GetUpcomingMeetingsCountAsync()
        {
            return await _context.Meetings
                        .CountAsync(m => m.Date > DateTime.Now);
        }

        public async Task<int> GetEndedMeetingsCountAsync()
        {
            return await _context.Meetings
                        .CountAsync(m => m.Date <= DateTime.Now);
        }

        public async Task<bool> AddMotherToMeetingAsync(int meetingId, int motherId)
        {
            var meeting = await _context.Meetings
            .Include(m => m.Mothers)
            .FirstOrDefaultAsync(m => m.Id == meetingId);

            if (meeting == null)
                return false;

            if (meeting.Date <= DateTime.Now) 
                return false;

            
            if (meeting.Mothers.Any(m => m.Id == motherId))
                return false;

            var mother = await _context.Mothers.FindAsync(motherId);
            if (mother == null)
                return false;

            meeting.Mothers.Add(mother);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMotherFromMeetingAsync(int meetingId, int motherId)
        {
            var meeting = await _context.Meetings
            .Include(m => m.Mothers)
            .FirstOrDefaultAsync(m => m.Id == meetingId);

            if (meeting == null)
                return false;

            if (meeting.Date <= DateTime.Now) 
                return false;

            var mother = meeting.Mothers.FirstOrDefault(m => m.Id == motherId);
            if (mother == null)
                return false; 

            meeting.Mothers.Remove(mother);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Mother>?> GetRegisteredMothersAsync(int meetingId)
        {
            var meeting = await _context.Meetings
            .Include(m => m.Mothers)
            .FirstOrDefaultAsync(m => m.Id == meetingId);

            if (meeting == null)
                return null;

            return meeting.Mothers.ToList();
        }

        public async Task<int> GetRegisteredMotherCountAsync(int meetingId)
        {
            var meeting = await _context.Meetings
            .Include(m => m.Mothers)
            .FirstOrDefaultAsync(m => m.Id == meetingId);

            if (meeting == null)
                return 0;

            return meeting.Mothers.Count;
        }

        public async Task<int> GetFeedbackCountAsync(int meetingId)
        {
            return await _context.Feedbacks
            .Where(f => f.MeetingId == meetingId)
            .CountAsync();
        }

        public async Task<List<Meeting>> GetMotherUpcomingMeetingsAsync(int motherId)
        {
            return await _context.Meetings
            .Where(m => m.Date > DateTime.Now &&
                    m.Mothers.Any(mm => mm.Id == motherId))
            .OrderBy(m => m.Date)
            .ToListAsync();
        }

        public async Task<List<Meeting>> GetMotherEndedMeetingsAsync(int motherId)
        {
            return await _context.Meetings
            .Where(m => m.Date <= DateTime.Now &&
                    m.Mothers.Any(mm => mm.Id == motherId))
            .OrderByDescending(m => m.Date)
            .ToListAsync();
        }

        public async Task<int> GetMotherUpcomingCountAsync(int motherId)
        {
            return await _context.Meetings
                .CountAsync(m => m.Date > DateTime.Now &&
                         m.Mothers.Any(mm => mm.Id == motherId));
        }

        public async Task<int> GetMotherEndedCountAsync(int motherId)
        {
            return await _context.Meetings
                .CountAsync(m => m.Date <= DateTime.Now &&
                         m.Mothers.Any(mm => mm.Id == motherId));
        }
        public async Task<List<Meeting>> GetDoctorUpcomingMeetingsAsync(int doctorId)
        {
            return await _context.Meetings
                .Where(m => m.Date > DateTime.Now &&
                            m.DoctorId == doctorId)
                .OrderBy(m => m.Date)
                .ToListAsync();
        }
        public async Task<List<Meeting>> GetDoctorEndedMeetingsAsync(int doctorId)
        {
            return await _context.Meetings
                .Where(m => m.Date <= DateTime.Now &&
                            m.DoctorId == doctorId)
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }
        public async Task<int> GetDoctorUpcomingCountAsync(int doctorId)
        {
            return await _context.Meetings
                .CountAsync(m => m.Date > DateTime.Now &&
                                 m.DoctorId == doctorId);
        }
        public async Task<int> GetDoctorEndedCountAsync(int doctorId)
        {
            return await _context.Meetings
                .CountAsync(m => m.Date <= DateTime.Now &&
                                 m.DoctorId == doctorId);
        }

    }
}
