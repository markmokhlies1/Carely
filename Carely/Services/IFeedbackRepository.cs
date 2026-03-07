using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IFeedbackRepository
    {
        Task<Feedback?> GetByIdAsync(int id);
        Task<Feedback?> GetFeedbackForMotherAsync(int meetingId, int motherId);
        Task<List<Feedback>> GetFeedbacksForMeetingAsync(int meetingId);
        Task AddAsync(Feedback feedback);
        Task UpdateAsync(Feedback feedback);
        Task DeleteAsync(Feedback feedback);
    }
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;

        public FeedbackRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.FindAsync(id);
        }

        public async Task<Feedback?> GetFeedbackForMotherAsync(int meetingId, int motherId)
        {
            return await _context.Feedbacks
                .FirstOrDefaultAsync(f => f.MeetingId == meetingId && f.MotherId == motherId);
        }

        public async Task<List<Feedback>> GetFeedbacksForMeetingAsync(int meetingId)
        {
            return await _context.Feedbacks
                .Include(f => f.Mother)
                .Where(f => f.MeetingId == meetingId)
                .ToListAsync();
        }

        public async Task AddAsync(Feedback feedback)
        {
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Feedback feedback)
        {
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
        }
    }

}
