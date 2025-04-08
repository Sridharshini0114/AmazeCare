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

        public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _repository.GetAppointmentsByDoctorIdAsync(doctorId);
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _repository.GetAppointmentByIdAsync(appointmentId);
        }

        public async Task<Appointment?> UpdateAppointmentAsync(AppointmentDto dto)
        {
            var appointment = new Appointment
            {
                AppointmentId = dto.AppointmentId, // ✅ Ensure AppointmentDto includes this
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                Symptoms = dto.Symptoms,
                Status = dto.Status
            };

            return await _repository.UpdateAppointmentAsync(appointment);
        }

    }
}
