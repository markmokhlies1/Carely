using Carely.Dtos.Requests.Medication;
using Carely.Dtos.Responses.Medication;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Carely.Controllers
{
    [Route("api/medications")]
    [Authorize(Roles = "Mother")]

    [ApiController] 
    public class MedicationsController : ControllerBase 
    {
        private readonly IMedicationRepository _medicationRepo; 
        private readonly IUserRepository _userRepo;

        public MedicationsController(IMedicationRepository medicationRepo, IUserRepository userRepo)
        {
            _medicationRepo = medicationRepo;
            _userRepo = userRepo;
        }


        #region Get Mediction for login mother
        [HttpGet]
        public async Task<IActionResult> GetAllForMother()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var medications = await _medicationRepo.GetAllForMotherAsync(motherId);
            var response = medications.Select(MedicationResponse.FromEntity);
            return Ok(response);
        }
        #endregion

        #region Get Medication by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medication = await _medicationRepo.GetByIdAsync(id);
            if (medication == null)
                return NotFound();

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            if (medication.MotherId != motherId)
                return Forbid();

            return Ok(MedicationResponse.FromEntity(medication));
        }
        #endregion

        #region Add Medication
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicationRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var mother = await _userRepo.GetMotherByIdAsync(motherId);
            if (mother == null)
                return NotFound(new { message = "Mother not found" });

            var medication = new Medication
            {
                Name = request.Name,
                Description = request.Description,
                Spot = request.Spot,
                StartDate = request.StartDate,
                Duration = request.Duration,
                MedicationType = request.MedicationType,
                MotherId = mother.Id
            };

            var created = await _medicationRepo.AddAsync(medication);
            return Ok(MedicationResponse.FromEntity(created));
        }
        #endregion

        #region Update Medication
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicationRequest request)
        {
            var medication = await _medicationRepo.GetByIdAsync(id);
            if (medication == null)
                return NotFound();

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            if (medication.MotherId != motherId)
                return Forbid();

            if (request.StartDate != medication.StartDate)
            {
                if (request.StartDate < DateTime.Today)
                {
                    return BadRequest(new { message = "Start date cannot be more than 10 days in the past." });
                }
            }

            medication.Name = request.Name;
            medication.Description = request.Description;
            medication.Spot = request.Spot;
            medication.StartDate = request.StartDate;
            medication.Duration = request.Duration;
            medication.MedicationType = request.MedicationType;

            await _medicationRepo.UpdateAsync(medication);
            return Ok(MedicationResponse.FromEntity(medication));
        }
        #endregion

        #region Delelete Medication
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var medication = await _medicationRepo.GetByIdAsync(id);
            if (medication == null)
                return NotFound();

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            if (medication.MotherId != motherId)
                return Forbid();

            await _medicationRepo.DeleteAsync(id);
            return Ok(new { message = "Medication deleted successfully." });
        }
        #endregion

        #region get medicion count for login mother 
        [HttpGet("count")]
        public async Task<IActionResult> GetCountForMother()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            int motherId = int.Parse(motherIdClaim);

            int count = await _medicationRepo.GetCountForMotherAsync(motherId);

            return Ok(new { count });
        }
        #endregion

    }
}
