using AmazeCare.Business;
using AmazeCare.Interfaces;
using AmazeCare.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientAuthController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientAuthController(IPatientService service)
        {
            _service = service;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(PatientUserDto dto)
        {
            try
            {
                var result = await _service.RegisterPatient(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var result = await _service.Login(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("ViewAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _service.GetAllPatients();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("filter")]
        public async Task<IActionResult> FilterPatients([FromBody] PatientFilterDto filters, [FromQuery] int? sortBy = null)
        {
            try
            {
                var result = await _service.FilterPatientsAsync(filters, sortBy);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("delete-user-with-patient/{userId}")]
        public async Task<IActionResult> DeleteUserWithPatient(int userId)
        {
            try
            {
                var result = await _service.DeleteUserWithPatientAsync(userId);
                if (!result)
                {
                    return NotFound(new { message = "User or patient not found" });
                }

                return Ok(new { message = "User and associated patient deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
