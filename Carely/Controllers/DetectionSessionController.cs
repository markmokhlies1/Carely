using Carely.Dtos.Requests.DetectionSession;
using Carely.Dtos.Responses.DetectionSession;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Carely.Controllers
{
    [Route("api/DetectionSession")]
    [ApiController]
    public class DetectionSessionController : ControllerBase
    {
        private readonly IDetectionSessionRepository _repository;
        private readonly MqttService _mqttService;

        public DetectionSessionController(IDetectionSessionRepository repository, MqttService mqttService)
        {
            _repository = repository;
            _mqttService = mqttService;
        }

        #region Start Mic
        [HttpPost("start")]
        [Authorize(Roles ="Mother")]
        public async Task<IActionResult> StartMic([FromBody] PlayMicRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid request data", errors = ModelState });
            }

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null) 
                return Unauthorized(new {message ="Invaild token"});

            if(!int.TryParse(motherIdClaim , out int motherId))
                return BadRequest(new {message ="Invalid mother Id in token"});

            var baby = await _repository.GetBabyByIdAsync(request.BabyId);
            if (baby == null)
                return NotFound(new { message = "Baby not found" });

            if (baby.MotherId != motherId)
               return Unauthorized(new { message = "This baby does not belong to you." });

            var session = await _repository.StartSessionAsync(request.BabyId);
            if(session == null)
                return BadRequest(new { message = "A session is already active for this baby." });

            var payload = JsonSerializer.Serialize(new { command = "START_RECORDING" });
            try
            {
                await _mqttService.PublishAsync("carely/device/mic/command", payload);
            }
            catch (Exception ex) {
                return StatusCode(500, new { message = $"Failed to publish MQTT command: {ex.Message}" });

            }

            return Ok(new PlayMicResponse { 
                Command = "START_RECORDING",
                StartTime = session.StartTime,
                Status = session.Status.ToString()
            });
        }
        #endregion

        #region Stop Mic
        [HttpPost("stop")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> StopMic([FromBody] StopMicRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request data.", errors = ModelState });

         
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            
            var baby = await _repository.GetBabyByIdAsync(request.BabyId);
            if (baby == null)
                return NotFound(new { message = "Baby not found." });

            if (baby.MotherId != motherId)
                return Unauthorized(new { message = "This baby does not belong to you." });

           
            var session = await _repository.StopSessionAsync(request.BabyId);
            if (session == null)
                return BadRequest(new { message = "No active session found for this baby." });

           
            var payload = JsonSerializer.Serialize(new { command = "STOP_RECORDING" });
            try
            {
                await _mqttService.PublishAsync("carely/device/mic/command", payload);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Failed to publish MQTT command: {ex.Message}" });
            }

            return Ok(new StopMicResponse
            {
                Command = "STOP_RECORDING",
                EndTime = session.EndTime ?? DateTime.UtcNow,
                Status = session.Status.ToString()
            });
        }
            #endregion
        }
}
