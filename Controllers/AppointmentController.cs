using AmazeCare.Models.DTOs;
using AmazeCare.Services;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> Schedule([FromBody] AppointmentDto dto)
        {
            var result = await _service.ScheduleAppointmentAsync(dto);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var appointments = await _service.GetAppointmentsByPatientIdAsync(patientId);
            return Ok(appointments);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAppointmentAsync(id);
            if (!success) return NotFound("Appointment not found");
            return Ok("Deleted successfully");
        }
    }
}
