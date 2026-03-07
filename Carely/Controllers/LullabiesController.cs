using Carely.Dtos.Requests.Lullaby;
using Carely.Dtos.Requests.Meeting;
using Carely.Dtos.Responses.Lullaby;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Carely.Controllers
{
    [Route("api/Lullabies")]
    [ApiController]
    public class LullabyController : ControllerBase
    {
        private readonly ILullabyRepository _repository;

        public LullabyController(ILullabyRepository repository)
        {
            _repository = repository;
        }



        #region Add lullabies
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddLullaby([FromForm] CreateLullabyRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (adminIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(adminIdClaim, out int adminId))
                return BadRequest(new { message = "Invalid admin ID in token." });


            var uploadsFolder = Path.Combine("wwwroot", "audio", "lullabies");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(dto.AudioFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.AudioFile.CopyToAsync(stream);
            }


            var lullaby = new Lullaby
            {
                Name = dto.Name,
                Duration = dto.Duration,
                FilePath = $"/audio/lullabies/{fileName}"
            };

            var added = await _repository.AddAsync(lullaby);

            return CreatedAtAction(nameof(GetById), new { id = added.Id },
                LullabiesResponse.FromEntity(added));
        }

        #endregion


        #region Update lullabies
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLullaby(int id, [FromForm] UpdateLullabyRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lullaby = await _repository.GetByIdAsync(id);
            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found" });

            // Update fields if provided
            if (!string.IsNullOrEmpty(dto.Name))
                lullaby.Name = dto.Name;

            if (dto.Duration.HasValue)
                lullaby.Duration = dto.Duration.Value;

            // Handle audio file replacement
            if (dto.AudioFile != null)
            {
                // Step 1: Delete old file if it exists
                if (!string.IsNullOrEmpty(lullaby.FilePath))
                {
                    var oldPath = Path.Combine("wwwroot", lullaby.FilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                // Step 2: Save new file
                var uploadsFolder = Path.Combine("wwwroot", "audio", "lullabies");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Path.GetFileName(dto.AudioFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.AudioFile.CopyToAsync(stream);
                }

                lullaby.FilePath = $"/audio/lullabies/{fileName}";
            }

            await _repository.UpdateAsync(lullaby);

            return Ok(LullabiesResponse.FromEntity(lullaby));
        }
        #endregion




        #region Get Lullaby by id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var lullaby = await _repository.GetByIdAsync(id);

            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found" });

            return Ok(LullabiesResponse.FromEntity(lullaby));
        }
        #endregion

        #region Get All Lullabies
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var lullabies = await _repository.GetAllAsync();


            var dtoList = lullabies.Select(LullabiesResponse.FromEntity);

            return Ok(dtoList);
        }
        #endregion

        #region Get Lullabies Count
        [HttpGet("count")]
        [Authorize]
        public async Task<IActionResult> GetCount()
        {
            var count = await _repository.GetCountAsync();
            return Ok(new { count });
        }
        #endregion

        #region Delete lullaby
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLullaby(int id)
        {
            var lullaby = await _repository.GetByIdAsync(id);
            if (lullaby == null)
                return NotFound(new { message = "Lullaby not found" });

            // Step 1: Delete the audio file from wwwroot if it exists
            if (!string.IsNullOrEmpty(lullaby.FilePath))
            {
                var filePath =Path.Combine("wwwroot" , lullaby.FilePath.TrimStart('/'));

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            // Step 2: Remove the record from the database
            await _repository.DeleteAsync(lullaby);

            return NoContent(); // 204 response, standard for successful delete
            #endregion
        }

    }
}
