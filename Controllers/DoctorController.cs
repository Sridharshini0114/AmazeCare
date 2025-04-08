using AmazeCare.Interfaces;
using AmazeCare.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("{doctorId}/appointments")]
        public async Task<IActionResult> GetAppointments(int doctorId)
        {
            var appointments = await _doctorService.GetAppointmentsByDoctorId(doctorId);
            return Ok(appointments);
        }

        [HttpPut("appointment/{appointmentId}/status")]
        public async Task<IActionResult> UpdateStatus(int appointmentId, [FromBody] string newStatus)
        {
            var result = await _doctorService.UpdateAppointmentStatus(appointmentId, newStatus);
            if (result == null) return NotFound("Appointment not found");
            return Ok(result);
        }

        [HttpPost("{doctorId}/appointments/filter")]
        public async Task<IActionResult> FilterAppointments(int doctorId, [FromBody] AppointmentFilterDto filter)
        {
            var filtered = await _doctorService.FilterAppointments(doctorId, filter);
            return Ok(filtered);
        }

        [HttpGet("appointment/{appointmentId}/patient")]
        public async Task<IActionResult> GetPatientDetails(int appointmentId)
        {
            var patientDetails = await _doctorService.GetPatientDetailsForAppointment(appointmentId);
            return Ok(patientDetails);
        }
    }
}
