using Carely.Data;
using Carely.Models;
using Carely.Models.Enums.DetectionSession;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IDetectionSessionRepository
    {
        Task<DetectionSession?> StartSessionAsync(int babyId);

        Task<DetectionSession?> StopSessionAsync(int babyId);

        Task<Baby?> GetBabyByIdAsync(int babyId);

        Task<DetectionSession?> GetActiveSessionAsync(int babyId);

    }

    public class DetectionSessionRepository : IDetectionSessionRepository
    {
        private readonly AppDbContext _context;
        private readonly MqttService mqttService;

        public DetectionSessionRepository(AppDbContext context)
        {
            _context = context;
            
        }


        public async Task<Baby?> GetBabyByIdAsync(int babyId)
        {
            return await _context.Babies.FindAsync(babyId);
        }
        public async Task<DetectionSession?> GetActiveSessionAsync(int babyId)
        {
            return await _context.DetectionSessions.FirstOrDefaultAsync(s => s.BabyId == babyId && s.Status == Status.Active);   
        }

        public async Task<DetectionSession?> StartSessionAsync(int babyId)
        {
            var baby =await GetBabyByIdAsync(babyId);
            if (baby == null) return null;


            var existing = await GetActiveSessionAsync(babyId);
            if (existing != null) return null;

            var session = new DetectionSession
            {
                BabyId = babyId,
                StartTime = DateTime.UtcNow,
                Status = Status.Active,
            };

            _context.DetectionSessions.Add(session);
            await _context.SaveChangesAsync();


            return session;
        }

        public async Task<DetectionSession?> StopSessionAsync(int babyId)
        {
            var session =await GetActiveSessionAsync(babyId);
            if (session == null) return null;

            session.EndTime = DateTime.UtcNow;
            session.Status = Status.Ended;

            await _context.SaveChangesAsync();
            return session;
        }
    }
}
