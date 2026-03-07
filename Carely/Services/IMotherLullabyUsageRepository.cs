using Carely.Data;
using Carely.Dtos.Responses.Lullaby;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public interface IMotherLullabyUsageRepository
    {
        Task<LullabyUsageSummaryResponse?> GetLullabyUsageSummaryAsync(int lullabyId);
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
    }
}
