
using Carely.Dtos.Requests.Baby;
using Carely.Dtos.Responses.Baby;
using Carely.Dtos.Responses.Vaccination;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Carely.Controllers
{
    [Route("api/Babies")]
    [ApiController]
    public class BabiesController : ControllerBase
    {
        private readonly IBabyRepository _babyRepo;
        private readonly IUserRepository _userRepo;
        


        public BabiesController(IBabyRepository babyRepo, IUserRepository userRepo, IBabyVaccinationRepository babyVaccinationRepo)
        {
            _babyRepo = babyRepo;
            _userRepo = userRepo;
           

        }

        #region Add Baby 
        [HttpPost]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> Create([FromBody] CreateBabyRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token" });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var mother = await _userRepo.GetMotherByIdAsync(motherId);
            if (mother == null)
                return NotFound(new { message = "Mother not found" });
            var baby = new Models.Baby
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Weight = request.Weight,
                DateOfBirth = request.DateOfBirth,
                Developmental = request.Developmental,
                MotherId = mother.Id

            };
            var created = await _babyRepo.AddAsync(baby);

           
            return Ok(BabyResponse.FromEntity(created));
        }
        #endregion

        #region Update Baby
        [HttpPut("{id}")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> UpdateBaby(int id, [FromBody] UpdateBabyRequest request)
        {

            var baby = await _babyRepo.GetByIdAsync(id);

            if (baby == null)
                return NotFound(new { message = "Baby Not Found" });

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            if (baby.MotherId != motherId)
                return Forbid();


            baby.FirstName = request.FirstName;
            baby.LastName = request.LastName;
            baby.Gender = request.Gender;
            baby.Weight = request.Weight;
            baby.DateOfBirth = request.DateOfBirth;
            baby.Developmental = request.Developmental;

            await _babyRepo.UpdateAsync(baby);
            return Ok(new { message = "Baby updated successfully" });
        }
        #endregion

        #region Delete Baby
        [HttpDelete("{id}")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> Delete(int id)
        {
            var baby = await _babyRepo.GetByIdAsync(id);
            if (baby == null)
                return NotFound();

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            if (baby.MotherId != motherId)
                return Forbid();

            await _babyRepo.DeleteAsync(id);
            return Ok(new { message = "Baby deleted successfully." });
        }

        #endregion

        #region Get Baby for login mother
        [HttpGet]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetAllForMother()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token" });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Ivalid mother Id in token." });

            var Babies = await _babyRepo.GetAllForMotherAsync(motherId);

            var response = Babies.Select(BabyResponse.FromEntity);
            return Ok(response);
            #endregion


        }
    }
}