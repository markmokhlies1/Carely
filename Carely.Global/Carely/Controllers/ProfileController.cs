using Carely.Dtos.Requests;
using Carely.Dtos.Requests.Jwt;
using Carely.Dtos.Requests.Mother;
using Carely.Models;
using Carely.Models.Base;
using Carely.Models.Enums;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Carely.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public ProfileController(IUserRepository userRepo, IJwtTokenProvider jwtTokenProvider)
        {
            _userRepo = userRepo;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var admin = await _userRepo.GetAdminByEmailAsync(request.Email!);
            var mother = await _userRepo.GetMotherByEmailAsync(request.Email!);

            var user = (User?)admin ?? mother;
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

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

            return Ok(new
            {
                message = "Login successful",
                role = user.Role.ToString(),
                token = tokenResponse.AccessToken,
                expiresAt = tokenResponse.Expires
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMother([FromBody] CreateMotherRequest request)
        {
            var existing = await _userRepo.GetMotherByEmailAsync(request.Email!);
            if (existing != null)
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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully" });
        }

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
            mother.Email = request.Email ?? mother.Email;
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
    }
}
