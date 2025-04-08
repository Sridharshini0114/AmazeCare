using AmazeCare.Models;

namespace AmazeCare.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> ScheduleAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<bool> DeleteAppointmentAsync(int appointmentId);

        Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);

        // ✅ NEW: Get appointment by its ID
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);

        // ✅ NEW: Update an existing appointment (if you want to avoid delete & re-add)
        Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
    }
}
