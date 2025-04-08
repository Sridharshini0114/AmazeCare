using AmazeCare.Models;
using AmazeCare.Models.DTOs;
namespace AmazeCare.Interfaces

{
    public interface IDoctorService
    {
        Task<IEnumerable<AppointmentViewDto>> GetAppointmentsByDoctorId(int doctorId);
        Task<AppointmentViewDto?> UpdateAppointmentStatus(int appointmentId, string newStatus);
        Task<IEnumerable<AppointmentViewDto>> FilterAppointments(int doctorId, AppointmentFilterDto filter);
        Task<PatientDetailsDto> GetPatientDetailsForAppointment(int appointmentId);

    }
}
