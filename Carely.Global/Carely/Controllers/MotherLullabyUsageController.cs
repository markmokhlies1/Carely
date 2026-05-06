using Carely.Dtos.Requests.Lullaby;
using Carely.Dtos.Responses.Lullaby;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Carely.Controllers
{
    [Route("api/MotherLullabyUsage")]
    [ApiController]
    public class MotherLullabyUsageController : ControllerBase
    {
        private readonly IMotherLullabyUsageRepository _repository;
        private readonly MqttService _mqttService;

        public MotherLullabyUsageController(IMotherLullabyUsageRepository repository, MqttService mqttService)
        {
            _repository = repository;
            _mqttService = mqttService;
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

        #region play lullaby
        [HttpPost("play")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> Play([FromBody] PlayCommandRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

           
            var lullaby = await _repository.GetLullabyByIdAsync(request.LullabyId);
            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found." });

       
            var usage = await _repository.RecordPlayAsync(motherId, request.LullabyId);
            if (usage == null)
                return BadRequest(new { message = "Could not record play usage." });

            var resumePosition = usage.LastPosition ?? TimeSpan.Zero;
          
            var fullUrl = $"{Request.Scheme}://{Request.Host}{(lullaby.FilePath.StartsWith("/") ? lullaby.FilePath : "/" + lullaby.FilePath)}";




            var payload = JsonSerializer.Serialize(new { command = "PLAY", url = fullUrl, position = resumePosition, level = usage.VolumeLevel });
            try
            {
                await _mqttService.PublishAsync("carely/device/audio/command", payload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to publish MQTT command: {ex.Message}" });
            }

            return Ok(new PlayCommandResponse
            {
                Command = "PLAY",
                Url = fullUrl,
               
            });
        }

        #endregion

        #region stop lullaby
        [HttpPost("stop")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> Stop([FromBody] StopCommandRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var lullaby = await _repository.GetLullabyByIdAsync(request.LullabyId);
            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found." });

            //lastpostion from request
            //TimeSpan stopPosition = request.StopPosition;


            //var usage = await _repository.RecordStopAsync(motherId, request.LullabyId, stopPosition);


            //taking value from iot to respond 
            var usage = await _repository.RecordStopAsync(motherId, request.LullabyId, TimeSpan.Zero);
            if (usage == null)
                return BadRequest(new { message = "No active playback found to stop." });

            var payload = JsonSerializer.Serialize(new { command = "STOP" });
            try
            {
                await _mqttService.PublishAsync("carely/device/audio/command", payload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to publish MQTT command: {ex.Message}" });
            }

            return Ok(new StopCommandResponse
            {
                //Respond
                Command = "STOP",
                StopPosition = usage.LastPosition ?? TimeSpan.Zero
            });
        }
        #endregion

        #region volume lullaby
        [HttpPost("volume")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> SetVolume([FromBody] VolumeCommandRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var lullaby = await _repository.GetLullabyByIdAsync(request.LullabyId);
            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found." });

            var usage = await _repository.RecordVolumeAsync(motherId, request.LullabyId, request.Level);
            if (usage == null)
                return BadRequest(new { message = "Cannot change volume unless lullaby is playing." });

            var payload = JsonSerializer.Serialize(new { command = "VOLUME", level = usage.VolumeLevel });
            await _mqttService.PublishAsync("carely/device/audio/command", payload);

            return Ok(new VolumeCommandResponse
            {
                Command = "VOLUME",
                Level = usage.VolumeLevel
            });
        }


        #endregion


    }
}
