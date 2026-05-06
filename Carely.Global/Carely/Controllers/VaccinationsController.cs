using Carely.Dtos.Responses.Vaccination;
using Carely.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Carely.Controllers
{
    [Route("api/Vaccinations")]
    [ApiController]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationRepository _repository;

        public VaccinationsController(IVaccinationRepository repository)
        {
            _repository = repository;
        }

        #region Get all vaccinations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vaccinations = await _repository.GetAllAsync();

            var dtoList = vaccinations.Select(v => VaccinationResponse.FromEntity(v));


            return Ok(dtoList);
        }
        #endregion

        #region Get Vaccination by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vaccination = await _repository.GetByIdAsync(id);
            if (vaccination == null)
                return NotFound(new { message = "Vaccination not found." });

            return Ok(VaccinationResponse.FromEntity(vaccination));
        }
        #endregion
    }
}
