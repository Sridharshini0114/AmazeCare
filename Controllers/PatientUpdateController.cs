using AmazeCare.Interfaces;
using AmazeCare.Models.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientUpdateController : ControllerBase
    {
        private readonly IPatientUpdateService _updateService;

        public PatientUpdateController(IPatientUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPut]
        [Authorize(Roles = "patient")]
        public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientDto dto)
        {
            try
            {
                await _updateService.UpdatePatientAsync(dto);
                return Ok("Patient details updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
