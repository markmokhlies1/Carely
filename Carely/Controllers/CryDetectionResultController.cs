using Carely.Dtos.Responses.CryDetection;
using Carely.Services;
using Microsoft.AspNetCore.Mvc;

namespace Carely.Controllers
{
    [Route("api/CryDetection")]
    [ApiController]
    public class CryDetectionResultController : ControllerBase
    {
        private readonly ICryDetectionResultRepository _repository;
        private readonly IFcmService _fcmService;
        private readonly HttpClient _httpClient;

        public CryDetectionResultController(
            ICryDetectionResultRepository repository,
            IFcmService fcmService,
            IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _fcmService = fcmService;
            _httpClient = httpClientFactory.CreateClient();
        }

        #region Receive WAV + Process
        [HttpPost("audio/chunk")]
        public async Task<IActionResult> ReceiveAudioChunk(
            IFormFile audio,
            [FromQuery] int babyId)
        {

            if (audio == null || audio.Length == 0)
                return BadRequest(new { message = "No audio file received." });


            var session = await _repository.GetDetectionSessionAsync(babyId);
            if (session == null)
                return BadRequest(new { message = "No active session found." });


            using var memoryStream = new MemoryStream();
            await audio.CopyToAsync(memoryStream);
            byte[] wavBytes = memoryStream.ToArray();


            using var formContent = new MultipartFormDataContent();
            formContent.Add(new ByteArrayContent(wavBytes), "file", "audio.wav");


            var mlHttpResponse = await _httpClient.PostAsync(
                "https://cry-model-api.redsky-7510cc1d.uaenorth.azurecontainerapps.io/predict",
                formContent
            );


            if (!mlHttpResponse.IsSuccessStatusCode)
                return StatusCode(500, new { message = "ML model call failed." });


            var mlResult = await mlHttpResponse.Content.ReadFromJsonAsync<MlResponse>();
            if (mlResult == null)
                return StatusCode(500, new { message = "Could not read ML response." });


            var savedResult = await _repository.SaveResultAsync(session.Id, mlResult);

            var lastTwo = await _repository.GetLastTwoResultsAsync(session.Id);
           
            
            if (lastTwo.Count ==2 && lastTwo.All(r => r.IsCrying))
            {
                var gap = Math.Abs((lastTwo[0].DetectedAt - lastTwo[1].DetectedAt).TotalSeconds);
                if(gap <= 17)
                {
                    var motherDeviceToken = session.Baby?.Mother?.DeviceToken;
                    if (!string.IsNullOrEmpty(motherDeviceToken))
                    {
                        try
                        {
                            await _fcmService.SendNotificationAsync(motherDeviceToken);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"FCM failed: {ex.Message}");
                        
                        }
                    }
                }
            }

            return Ok(new
            {
                isCrying = savedResult.IsCrying,
                detectedAt = savedResult.DetectedAt
            });
        }
        #endregion

        #region Get Session Results
        [HttpGet("results/{babyId}")]
        public async Task<IActionResult> GetSessionResults(int babyId)
        {
            var results = await _repository.GetSessionResultsAsync(babyId);

            if (!results.Any())
                return NotFound(new { message = "No results found for this baby." });

            return Ok(results.Select(r => new
            {
                isCrying = r.IsCrying,
                detectedAt = r.DetectedAt
            }));
        }
        #endregion
    }
}