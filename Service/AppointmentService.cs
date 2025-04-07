using AmazeCare.Interfaces;
using AmazeCare.Models;
using AmazeCare.Models.DTOs;

namespace AmazeCare.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Appointment> ScheduleAppointmentAsync(AppointmentDto dto)
        {
            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Symptoms = dto.Symptoms,
                Status = dto.Status
            };

            return await _repository.ScheduleAppointmentAsync(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _repository.GetAppointmentsByPatientIdAsync(patientId);
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            return await _repository.DeleteAppointmentAsync(appointmentId);
        }
    }
}
