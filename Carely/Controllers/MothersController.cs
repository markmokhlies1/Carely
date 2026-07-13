using Carely.Dtos.Responses.Mother;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using Carely.Dtos.Requests.Mother;

namespace Carely.Controllers
{
    [Route("api/mothers")]
    [ApiController]
    public class MothersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public MothersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        #region Get All Mother
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var mothers = await _userRepo.GetMotherListAsync();

            if (mothers == null)
            {
                return NotFound(new { message = "Mothers not found" });
            }
            return Ok(mothers.Select(MotherResponse.FromEntity));
        }
        #endregion

        #region Get By Id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var mother = await _userRepo.GetMotherByIdAsync(id);
            if (mother == null)
                return NotFound(new { message = "Mother not found" });

            return Ok(MotherResponse.FromEntity(mother));
        }
        #endregion

        #region Delete Mother
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var mother = await _userRepo.GetMotherByIdAsync(id);
            if (mother == null)
                return NotFound(new { message = "Mother not found" });

            await _userRepo.DeleteMotherAsync(mother);
            return Ok(new { message = "Mother deleted successfully" });
        }
        #endregion

        #region Get Mother count
        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMothersCount()
        {
            var count = await _userRepo.GetMothersCountAsync();

            return Ok(count);
        }
        #endregion

        #region Device Token
        [HttpPost("device-token")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> UpdateDeviceToken(
           [FromBody] UpdateDeviceTokenRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var mother = await _userRepo.GetMotherByIdAsync(motherId);
            if (mother == null)
                return NotFound(new { message = "Mother not found." });

            mother.DeviceToken = request.DeviceToken;
            await _userRepo.UpdateMotherAsync(mother);

            return Ok(new { message = "Device token updated successfully." });
        }
        #endregion

    }
}
