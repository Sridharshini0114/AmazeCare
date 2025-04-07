using AmazeCare.Models;

namespace AmazeCare.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment> ScheduleAppointmentAsync(Appointment appointment);
        Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
    }
}
