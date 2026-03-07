using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carely.Controllers
{
    [Route("api/MotherLullabyUsage")]
    [ApiController]
    public class MotherLullabyUsageController : ControllerBase
    {
        private readonly IMotherLullabyUsageRepository _repository;

        public MotherLullabyUsageController(IMotherLullabyUsageRepository repository)
        {
            _repository = repository;
        }


        #region Get Lullaby Usage Summary
        [HttpGet("lullaby/{lullabyId}/summary")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLullabyUsageSummary(int lullabyId)
        {
            var summary = await _repository.GetLullabyUsageSummaryAsync(lullabyId);

            if (summary == null)
                return NotFound(new { message = "Lullaby not found." });

            return Ok(summary);
        }
        #endregion


    }
}
