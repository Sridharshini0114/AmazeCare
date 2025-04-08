using AmazeCare.Interfaces;
using AmazeCare.Models.DTOs;
using AmazeCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicalRecordController : Controller
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public MedicalRecordController(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        // POST api/medicalrecord
        [HttpPost]
        public async Task<ActionResult<MedicalRecordViewDto>> CreateMedicalRecordAsync([FromBody] MedicalRecordCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Medical record data is required.");

            try
            {
                var response = await _medicalRecordService.CreateMedicalRecordAsync(dto);
                return Created($"api/medicalrecord/appointment/{response.AppointmentId}", response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred while creating the medical record. {ex.Message}");
            }
        }

        // PUT api/medicalrecord
        [HttpPut]
        public async Task<ActionResult<MedicalRecordViewDto>> UpdateMedicalRecordAsync([FromBody] UpdateMedicalRecordDto dto)
        {
            if (dto == null)
                return BadRequest("Medical record update data is required.");

            try
            {
                var updated = await _medicalRecordService.UpdateMedicalRecordAsync(dto);
                if (updated == null)
                    return NotFound($"Medical record with ID {dto.RecordId} not found.");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the medical record. {ex.Message}");
            }
        }

        // GET api/medicalrecord/appointment/{appointmentId}
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<MedicalRecordViewDto>> GetMedicalRecordByAppointmentIdAsync(int appointmentId)
        {
            try
            {
                var record = await _medicalRecordService.GetMedicalRecordByAppointmentIdAsync(appointmentId);
                if (record == null)
                    return NotFound($"No medical record found for appointment ID {appointmentId}.");

                return Ok(record);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the medical record. {ex.Message}");
            }
        }
    }
}
