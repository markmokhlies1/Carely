using Carely.Data;
using Carely.Dtos.Responses.CryDetection;
using Carely.Models;
using Carely.Models.Enums.DetectionSession;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface ICryDetectionResultRepository
    {
        Task<CryDetectionResult> SaveResultAsync(int sessionId , MlResponse mlResult);
        Task<DetectionSession?> GetDetectionSessionAsync(int sessionId);
        Task<List<CryDetectionResult>> GetSessionResultsAsync(int babyId);
        Task<List<CryDetectionResult>> GetLastTwoResultsAsync(int sessionId);
    }
    public class CryDetectionRepository : ICryDetectionResultRepository
    {
        private readonly AppDbContext _context;

        public CryDetectionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CryDetectionResult>> GetLastTwoResultsAsync(int sessionId)
        {
            return await _context.CryDetectionResults
                .Where(r => r.DetectionSessionId == sessionId)
                .OrderByDescending(r => r.DetectedAt)
                .Take(2)
                .ToListAsync();
        }
        public async Task<DetectionSession?> GetDetectionSessionAsync(int babyId)
        {
            return await _context.DetectionSessions
                .Include(s => s.Baby)
                .ThenInclude(m => m.Mother)
                .FirstOrDefaultAsync(s => s.BabyId == babyId && s.Status == Status.Active);

        }

        public async Task<CryDetectionResult> SaveResultAsync(int sessionId , MlResponse mlResult)
        {
            var result = new CryDetectionResult
            {
                DetectionSessionId=sessionId,
                IsCrying = mlResult.IsCrying,
                DetectedAt= DateTime.UtcNow

            };

            _context.CryDetectionResults.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }
        public async Task<List<CryDetectionResult>> GetSessionResultsAsync(int babyId)
        {
            var session = await _context.DetectionSessions
                .FirstOrDefaultAsync(s => s.BabyId == babyId && s.Status == Status.Active);
            if (session == null) return new List<CryDetectionResult>();

            return await _context.CryDetectionResults
                .Where(r => r.DetectionSessionId == session.Id)
                .OrderByDescending(r => r.DetectedAt)
                .ToListAsync();
        }
    }
}
