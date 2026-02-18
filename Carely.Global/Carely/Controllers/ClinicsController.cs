using Carely.Dtos.Requests.Clinic;
using Carely.Dtos.Responses.Clinic;
using Carely.Dtos.Responses.ClinicWorkTime;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Carely.Controllers
{
    [Route("api/Clinics")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicRepository _clinicRepo;
        private readonly IUserRepository _userRepo;

        public ClinicsController(IClinicRepository clinicRepo, IUserRepository userRepo)
        {
            _clinicRepo = clinicRepo;
            _userRepo = userRepo;
        }

        #region Add Clinic

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddClinic([FromBody] CreateClinicRequest request)
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var doctor = await _userRepo.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
                return Unauthorized(new { message = "Doctor not found" });

            var clinic = new Clinic
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                PhoneNumber = request.PhoneNumber,
                DoctorId = doctorId
            };

            foreach (var wt in request.WorkTimes)
            {
                clinic.WorkTimes.Add(new ClinicWorkTime
                {
                    Day = wt.Day,
                    From = wt.From,
                    To = wt.To
                });
            }

            await _clinicRepo.AddClinicAsync(clinic);

            return Ok(new { message = "Clinic created successfully", id = clinic.Id });
        }

        #endregion

        #region Update Clinic

        [HttpPut("{clinicId}")] 
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateClinic(int clinicId, [FromBody] UpdateClinicRequest request)
        {

            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest();

            await _clinicRepo.UpdateClinicAsync(clinicId, doctorId, request);

            return Ok(new { message = "Clinic updated successfully" });
        }

        #endregion

        #region Delte Clinic

        [HttpDelete("{clinicId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> DeleteClinic(int clinicId)
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var clinic = await _clinicRepo.GetClinicByIdAsync(clinicId);
            if (clinic == null)
                return NotFound(new { message = "Clinic not found" });

            if (clinic.DoctorId != doctorId)
                return Unauthorized(new { message = "You can delete only your own clinics" });

            await _clinicRepo.DeleteClinicAsync(clinic);

            return Ok(new { message = "Clinic deleted successfully" });
        }

        #endregion

        #region Get Clinic By Id 

        [HttpGet("{clinicId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int clinicId)
        {
            var clinic = await _clinicRepo.GetClinicByIdAsync(clinicId);
            if (clinic == null)
                return NotFound(new { message = "Clinic not found" });

            return Ok(ClinicResponse.FromEntity(clinic));
        }

        #endregion

        #region GetClinics For LoggedIn Doctor

        [HttpGet("my-clinics")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetClinicsForLoggedInDoctor()
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var clinics = await _clinicRepo.GetDoctorClinicsAsync(doctorId);

            var response = clinics.Select(c => new ClinicResponse
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                City = c.City,
                PhoneNumber = c.PhoneNumber,
                WorkTimes = c.WorkTimes.Select(w => new ClinicWorkTimeResponse
                {
                    Day = w.Day,
                    From = w.From,
                    To = w.To
                }).ToList()
            });

            return Ok(response);
        }


        #endregion

        #region Get Doctor Clincs 

        [HttpGet("doctor/{doctorId}")]
        [Authorize]
        public async Task<IActionResult> GetDoctorClinics(int doctorId)
        {
            var clinics = await _clinicRepo.GetDoctorClinicsAsync(doctorId);
            return Ok(clinics.Select(ClinicResponse.FromEntity));
        }

        #endregion

    }
}
