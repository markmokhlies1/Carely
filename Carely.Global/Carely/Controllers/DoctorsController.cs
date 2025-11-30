using Carely.Dtos.Responses.Doctor;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carely.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public DoctorsController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        #region Get All Doctors
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _userRepo.GetDoctorListAsync();

            if (doctors == null)
                return NotFound(new { message = "Doctors not found" });

            return Ok(doctors.Select(DoctorResponse.FromEntity));
        }
        #endregion

        #region Get Doctor By Id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _userRepo.GetDoctorByIdAsync(id);

            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            return Ok(DoctorResponse.FromEntity(doctor));
        }
        #endregion

        #region Delete Doctor
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _userRepo.GetDoctorByIdAsync(id);

            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            await _userRepo.DeleteDoctorAsync(doctor);

            return Ok(new { message = "Doctor deleted successfully" });
        }
        #endregion

        #region Get Doctor Count
        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDoctorsCount()
        {
            var count = await _userRepo.GetDoctorsCountAsync();
            return Ok(count);
        }
        #endregion

    }
}
