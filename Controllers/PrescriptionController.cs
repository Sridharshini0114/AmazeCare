using AmazeCare.Interfaces;
using AmazeCare.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AmazeCare.Controllers
{
    [Route("api/prescription")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        // POST api/prescription
        [HttpPost]
        public async Task<ActionResult<PrescriptionResponseDTO>> CreatePrescriptionAsync([FromBody] PrescriptionDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Prescription data is required.");
            }

            try
            {
                var response = await _prescriptionService.CreatePrescriptionAsync(dto);

                // Return 201 Created with location header pointing to where the resource can be accessed
                return Created($"api/prescription/appointment/{response.AppointmentId}", response);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while creating the prescription.");
            }
        }

        [HttpPost("{prescriptionId}/details")]
        public async Task<ActionResult<PrescriptionDetailResponseDTO>> AddDetail(int prescriptionId, [FromBody] PrescriptionDetailDTO dto)
        {
            var createdDetail = await _prescriptionService.AddPrescriptionDetailAsync(prescriptionId, dto);

            return Created($"api/prescription/{prescriptionId}/details/{createdDetail.DetailId}", createdDetail);
        }



        // GET api/prescription/appointment/{appointmentId}
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<List<PrescriptionDetailResponseDTO>>> GetPrescriptionDetailsByAppointmentIdAsync(int appointmentId)
        {
            try
            {
                var details = await _prescriptionService.GetPrescriptionDetailsByAppointmentIdAsync(appointmentId);

                if (details == null || !details.Any())
                {
                    return NotFound($"No prescription details found for appointmentId {appointmentId}.");
                }

                return Ok(details);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred while retrieving the prescription details.");
            }
        }
    }
}
