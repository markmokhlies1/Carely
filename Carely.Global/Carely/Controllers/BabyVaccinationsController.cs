using Carely.Dtos.Responses.BabyVaccination;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[Route("api/BabyVaccinations")]
[ApiController]
[Authorize(Roles = "Mother")]
public class BabyVaccinationsController : ControllerBase
{
    private readonly IBabyVaccinationRepository _babyVaccinationRepo;
    private readonly IBabyRepository _babyRepo;
    private readonly IVaccinationRepository _vaccinationRepo;

    public BabyVaccinationsController(
        IBabyVaccinationRepository babyVaccinationRepo,
        IBabyRepository babyRepo,
        IVaccinationRepository vaccinationRepo)
    {
        _babyVaccinationRepo = babyVaccinationRepo;
        _babyRepo = babyRepo;
        _vaccinationRepo = vaccinationRepo;
    }

    private int? GetMotherIdFromToken()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.TryParse(claim, out int id) ? id : null;
    }

    private async Task<(Baby? baby, IActionResult? error)> ValidateBabyOwnership(int babyId, int motherId)
    {
        var baby = await _babyRepo.GetByIdAsync(babyId);
        if (baby == null) return (null, NotFound(new { message = "Baby not found" }));
        if (baby.MotherId != motherId) return (null, Forbid());
        return (baby, null);
    }

    #region Get All Vaccinations
    [HttpGet("{babyId}")]
    public async Task<IActionResult> GetAll(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token" });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var response = allVaccinations
            .Select(v => BabyVaccinationResponse.FromEntity(
                v,
                checkedIds.Contains(v.Id),
                baby!.DateOfBirth.AddMonths((int)v.Age)));

        return Ok(response);

    }
    #endregion

    #region Get Checked vaccination
    [HttpGet("{babyId}/checked")]
    public async Task<IActionResult> GetChecked(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var response = allVaccinations
            .Where(v => checkedIds.Contains(v.Id)).Select(v => BabyVaccinationResponse.FromEntity(v, true,
                baby!.DateOfBirth.AddMonths((int)v.Age)));
        return Ok(response);

    }
    #endregion

    #region Get unchecked vaccination
    [HttpGet("{babyId}/unchecked")]
    public async Task<IActionResult> GetUnchecked(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var response = allVaccinations
            .Where(v => !checkedIds.Contains(v.Id))
            .Select(v => BabyVaccinationResponse.FromEntity(
                v, false,
                baby!.DateOfBirth.AddMonths((int)v.Age)));

        return Ok(response);
    }
    #endregion

    #region Get Upcoming Vaccinations
    [HttpGet("{babyId}/upcoming")]
    public async Task<IActionResult> GetUpcoming(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var today = DateTime.Today;

        var futurePending = allVaccinations
            .Where(v => !checkedIds.Contains(v.Id))
            .Select(v => new { Vaccination = v, DueDate = baby!.DateOfBirth.AddMonths((int)v.Age) })
            .Where(x => x.DueDate >= today)
            .OrderBy(x => x.DueDate)
            .ToList();

        if (!futurePending.Any())
            return Ok(new List<BabyVaccinationResponse>());

        // Get only the vaccinations that share the nearest due date
        var nearestDueDate = futurePending.First().DueDate;

        var response = futurePending
            .Where(x => x.DueDate == nearestDueDate)
            .Select(x => BabyVaccinationResponse.FromEntity(x.Vaccination, false, x.DueDate));

        return Ok(response);
    }
    #endregion

    #region Get Late Vaccinations
    [HttpGet("{babyId}/late")]
    public async Task<IActionResult> GetLate(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var today = DateTime.Today;

        var response = allVaccinations
            .Where(v => !checkedIds.Contains(v.Id))
            .Select(v => new { Vaccination = v, DueDate = baby!.DateOfBirth.AddMonths((int)v.Age) })
            .Where(x => x.DueDate < today)   
            .OrderBy(x => x.DueDate)
            .Select(x => BabyVaccinationResponse.FromEntity(x.Vaccination, false, x.DueDate));

        return Ok(response);
    }
    #endregion

    #region Get Vaccination Summary
    [HttpGet("{babyId}/summary")]
    public async Task<IActionResult> GetSummary(int babyId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var allVaccinations = await _vaccinationRepo.GetAllAsync();
        var checkedRecords = await _babyVaccinationRepo.GetAllForBabyAsync(babyId);
        var checkedIds = checkedRecords.Select(bv => bv.VaccinationId).ToHashSet();

        var today = DateTime.Today;

        var pending = allVaccinations
            .Where(v => !checkedIds.Contains(v.Id))
            .Select(v => new { Vaccination = v, DueDate = baby!.DateOfBirth.AddMonths((int)v.Age) })
            .ToList();

        var summary = new BabyVaccinationSummaryResponse
        {
            Total = allVaccinations.Count(),
            Checked = checkedIds.Count,
            Unchecked = allVaccinations.Count() - checkedIds.Count,
            Late = pending.Count(x => x.DueDate < today),
            Upcoming = pending.Count(x => x.DueDate >= today)
        };

        return Ok(summary);
    }
    #endregion

    #region mother checks vaccination
    [HttpPost("{babyId}/{vaccinationId}")]

    public async Task<IActionResult> Check(int babyId, int vaccinationId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (baby, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var vaccination = await _vaccinationRepo.GetByIdAsync(vaccinationId);
        if (vaccination == null)
            return NotFound(new { message = "Vaccination not found." });


        var requiredDate = baby!.DateOfBirth.AddMonths((int)vaccination.Age);
        if (DateTime.Today < requiredDate)
            return BadRequest(new
            {
                message = $"Baby has not reached the required age for this vaccination. Due on {requiredDate:yyyy-MM-dd}."
            });

        var existing = await _babyVaccinationRepo.GetByBabyAndVaccinationAsync(babyId, vaccinationId);
        if (existing != null)
            return Conflict(new { message = "Already checked." });

        var record = new BabyVaccination
        {
            BabyId = babyId,
            VaccinationId = vaccinationId,
            Checkbox = true
        };

        await _babyVaccinationRepo.AddAsync(record);
        return Ok(new { message = "Vaccination checked." });
    }



    #endregion

    #region Mother uncheckes Vaccination
    [HttpDelete("{babyId}/{vaccinationId}")]
    public async Task<IActionResult> Uncheck(int babyId, int vaccinationId)
    {
        var motherId = GetMotherIdFromToken();
        if (motherId == null)
            return Unauthorized(new { message = "Invalid token." });

        var (_, error) = await ValidateBabyOwnership(babyId, motherId.Value);
        if (error != null) return error;

        var existing = await _babyVaccinationRepo.GetByBabyAndVaccinationAsync(babyId, vaccinationId);
        if (existing == null)
            return NotFound(new { message = "Vaccination not checked." });

        await _babyVaccinationRepo.DeleteAsync(existing);
        return Ok(new { message = "Vaccination unchecked." });
    }
}

    #endregion

