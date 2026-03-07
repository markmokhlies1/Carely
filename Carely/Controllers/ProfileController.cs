using Azure.Core;
using Carely.Dtos.Requests;
using Carely.Dtos.Requests.Doctor;
using Carely.Dtos.Requests.Jwt;
using Carely.Dtos.Requests.Mother;
using Carely.Dtos.Responses.Admin;
using Carely.Dtos.Responses.Doctor;
using Carely.Dtos.Responses.Mother;
using Carely.Models;
using Carely.Models.Base;
using Carely.Models.Enums.User;
using Carely.Services;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
 
namespace Carely.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        #region Fields And Ctor
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public ProfileController(IUserRepository userRepo, IJwtTokenProvider jwtTokenProvider)
        {
            _userRepo = userRepo;
            _jwtTokenProvider = jwtTokenProvider;
        }
        #endregion


        #region Login As Doctor Or Admin Or Mother
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var mother = await _userRepo.GetMotherByEmailAsync(request.Email!);
            if (mother != null && BCrypt.Net.BCrypt.Verify(request.Password, mother.PasswordHash))
                return Ok(GenerateResponse(mother));

            var doctor = await _userRepo.GetDoctorByEmailAsync(request.Email!);
            if (doctor != null && BCrypt.Net.BCrypt.Verify(request.Password, doctor.PasswordHash))
                return Ok(GenerateResponse(doctor));

            var admin = await _userRepo.GetAdminByEmailAsync(request.Email!);
            if (admin != null && BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
                return Ok(GenerateResponse(admin));

            return Unauthorized(new { message = "Invalid email or password." });
        }

        #endregion

        #region Register As Mother
        [HttpPost("register")]
        public async Task<IActionResult> RegisterMother([FromBody] CreateMotherRequest request)
        {

            if (await EmailExists(request.Email!))
                return BadRequest(new { message = "Email already exists." });

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var mother = new Mother
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Hight = request.Hight,
                Weight = request.Weight,
                Role = UserRole.Mother
            };

            await _userRepo.AddMotherAsync(mother);
            return Ok(new { message = "Mother registered successfully" });
        }
        #endregion

        #region  Register As Doctor 

        [HttpPost("register-doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] CreateDoctorRequest request)
        {
            if (await EmailExists(request.Email!))
                return BadRequest(new { message = "Email already exists." });

            var doctor = new Doctor
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                Age = request.Age,
                Link = request.Link,
                Specification = request.Specification,
                Role = UserRole.Doctor
            };

            await _userRepo.AddDoctorAsync(doctor);
            return Ok(new { message = "Doctor registered successfully" });
        }

        #endregion

        #region Update Mother Profile

        [HttpPut("update")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateMotherRequest request)
        {
            var motherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var mother = await _userRepo.GetMotherByIdAsync(motherId);
            if (mother == null)
                return NotFound(new { message = "Mother not found." });

            mother.FirstName = request.FirstName ?? mother.FirstName;
            mother.LastName = request.LastName ?? mother.LastName;
            mother.PhoneNumber = request.PhoneNumber ?? mother.PhoneNumber;
            mother.BirthDate = request.BirthDate != default ? request.BirthDate : mother.BirthDate;
            mother.Hight = request.Hight != 0 ? request.Hight : mother.Hight;
            mother.Weight = request.Weight != 0 ? request.Weight : mother.Weight;

            if (!string.IsNullOrWhiteSpace(request.Password))
                mother.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _userRepo.UpdateMotherAsync(mother);

            return Ok(new
            {
                message = "Profile updated successfully",
                updatedProfile = new
                {
                    mother.FirstName,
                    mother.LastName,
                    mother.Email,
                    mother.PhoneNumber,
                    mother.BirthDate,
                    mother.Hight,
                    mother.Weight
                }
            });
        }
        #endregion

        #region Update Doctor Profile 

        [HttpPut("update-doctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctorProfile([FromBody] UpdateDoctorRequest request)
        {
            var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var doctor = await _userRepo.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found." });

            doctor.FirstName = request.FirstName;
            doctor.LastName = request.LastName;
            doctor.PhoneNumber = request.PhoneNumber;
            doctor.Gender = request.Gender;
            doctor.Age =  request.Age ;
            doctor.Link = request.Link;
            doctor.Specification = request.Specification;

            if (!string.IsNullOrWhiteSpace(request.Password))
                doctor.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _userRepo.UpdateDoctorAsync(doctor);

            return Ok(new
            {
                message = "Doctor profile updated successfully",
                updatedProfile = new
                {
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.Email,
                    doctor.PhoneNumber,
                    doctor.Gender,
                    doctor.Age,
                    doctor.Link,
                    doctor.Specification
                }
            });
        }
        #endregion

        #region Get Loged in User 

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userIdClaim == null || role == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest(new { message = "Invalid user ID in token." });

            object? profile = null;

            switch (role)
            {
                case "Mother":
                    var mother = await _userRepo.GetMotherByIdAsync(userId);
                    if (mother != null)
                        profile = MotherResponse.FromEntity(mother);
                    break;

                case "Doctor":
                    var doctor = await _userRepo.GetDoctorByIdAsync(userId);
                    if (doctor != null)
                        profile = DoctorResponse.FromEntity(doctor);
                    break;

                case "Admin":
                    var admin = await _userRepo.GetAdminByIdAsync(userId);
                    if (admin != null)
                        profile = AdminResponse.FromEnitity(admin);
                    break;

                default:
                    return Unauthorized(new { message = "Unknown role." });
            }

            if (profile == null)
                return NotFound(new { message = $"{role} profile not found." });

            return Ok(new
            {
                message = "Profile fetched successfully",
                role,
                profile
            });
        }

        #endregion

        #region LogOut
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully" });
        }
        #endregion

        #region Helper
        private async Task<bool> EmailExists(string email)
        {
            return await _userRepo.GetMotherByEmailAsync(email) != null
                || await _userRepo.GetDoctorByEmailAsync(email) != null
                || await _userRepo.GetAdminByEmailAsync(email) != null;
        }
        private object GenerateResponse(User user)
        {
            var tokenRequest = new GenerateTokenRequest
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = new List<string> { user.Role.ToString() },
                Permissions = []
            };

            var tokenResponse = _jwtTokenProvider.GenerateJwtToken(tokenRequest);

            return new
            {
                message = "Login successful",
                role = user.Role.ToString(),
                token = tokenResponse.AccessToken,
                expiresAt = tokenResponse.Expires
            };
        }

        #endregion

    }
}
