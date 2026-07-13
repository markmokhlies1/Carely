using Carely.Data;
using Carely.Dtos.Responses.Lullaby;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IMotherLullabyUsageRepository
    {
        Task<LullabyUsageSummaryResponse?> GetLullabyUsageSummaryAsync(int lullabyId);
        Task<MotherLullabyUsage?> RecordPlayAsync(int motherId, int lullabyId);
       Task<MotherLullabyUsage?> RecordStopAsync(int motherId , int lullabyId, TimeSpan stopPosition);
        Task<MotherLullabyUsage?> RecordVolumeAsync(int motherId, int lullabyId, int level);
        Task<Lullaby?> GetLullabyByIdAsync(int lullabyId);
        Task<Mother?> GetMotherByIdAsync(int motherId);
    }

    public class MotherLullabyUsageRepository : IMotherLullabyUsageRepository
    {
        private readonly AppDbContext _context;

        public MotherLullabyUsageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LullabyUsageSummaryResponse?> GetLullabyUsageSummaryAsync(int lullabyId)
        {
            var lullaby = await _context.Lullabies
                .Include(l => l.MotherUsages)
                .ThenInclude(mu => mu.Mother)
                .FirstOrDefaultAsync(l => l.Id == lullabyId);

            if (lullaby == null) return null;

            return new LullabyUsageSummaryResponse
            {
                LullabyId = lullaby.Id,
                LullabyName = lullaby.Name,
                MotherCount = lullaby.MotherUsages.Count,
                Mothers = lullaby.MotherUsages.Select(mu =>
                {
                    if (mu.Mother == null)
                    {
                        throw new InvalidOperationException(
                            $"Mother record missing for usage Id {mu.Id} (MotherId={mu.MotherId})"
                        );
                    }

                    return new MotherUsageResponse
                    {
                        MotherId = mu.MotherId,
                        MotherName = $"{mu.Mother.FirstName} {mu.Mother.LastName}".Trim(),
                        PlayCount = mu.PlayCount
                    };
                }).ToList()

            };
        }

        public async Task<Mother?> GetMotherByIdAsync(int motherId)
        {
            return await _context.Mothers.FindAsync(motherId);
        }

        public async Task<Lullaby?> GetLullabyByIdAsync(int lullabyId)
        {
            return await _context.Lullabies.FindAsync(lullabyId);
        }

        public async Task<MotherLullabyUsage?> RecordPlayAsync(int motherId, int lullabyId)
        {
            var mother = await GetMotherByIdAsync(motherId);
            if (mother == null) return null;

            var lullaby = await GetLullabyByIdAsync(lullabyId);
            if (lullaby == null) return null;

            var usage = await _context.MotherLullabyUsages
                .FirstOrDefaultAsync(mu => mu.MotherId == motherId && mu.LullabyId == lullabyId);

            if (usage == null)
            {
                usage = new MotherLullabyUsage
                {
                    MotherId = motherId,
                    LullabyId = lullabyId,
                    PlayCount = 0,
                    LastPosition = null,
                    IsPlaying = true,
                    VolumeLevel = 50,
                };
                _context.MotherLullabyUsages.Add(usage);
                
            }
            else
            {
                usage.IsPlaying = true;
                if (usage.VolumeLevel == 0) usage.VolumeLevel = 50;
            }
            await _context.SaveChangesAsync();
            return usage;
        }

        public async Task<MotherLullabyUsage?> RecordStopAsync(int motherId, int lullabyId, TimeSpan stopPosition)
        {
            var usage = await _context.MotherLullabyUsages.FirstOrDefaultAsync(mu => mu.MotherId == motherId && mu.LullabyId== lullabyId);

          if(usage == null || !usage.IsPlaying) return null;

            var lullaby = await GetLullabyByIdAsync(lullabyId);
            if(lullaby == null) return null;

            if(stopPosition >= lullaby.Duration)
            {
                usage.PlayCount++;
                usage.LastPosition = null;
            }
            else
            {
                usage.LastPosition = stopPosition;
            }

            usage.IsPlaying = false;
            await _context.SaveChangesAsync(); 
            return usage;
        }

        public async Task<MotherLullabyUsage?> RecordVolumeAsync(int motherId, int lullabyId, int level)
        {
            var mother = await GetMotherByIdAsync(motherId);
            if (mother == null) return null;

            var lullaby = await GetLullabyByIdAsync(lullabyId);
            if (lullaby == null) return null;

            var usage = await _context.MotherLullabyUsages
                .FirstOrDefaultAsync(mu => mu.MotherId == motherId && mu.LullabyId == lullabyId);

            if (usage == null || !usage.IsPlaying) return null;
            usage.VolumeLevel = level;
            await _context.SaveChangesAsync();
            return usage;
        }

    }
}
