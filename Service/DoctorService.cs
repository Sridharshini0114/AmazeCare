using AmazeCare.Interfaces;
using AmazeCare.Models;
using AmazeCare.Models.DTOs;

namespace AmazeCare.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPatientRepository _patientRepo;

        public DoctorService(
            IAppointmentRepository appointmentRepo,
            IUserRepository userRepo,
            IPatientRepository patientRepo)
        {
            _appointmentRepo = appointmentRepo;
            _userRepo = userRepo;
            _patientRepo = patientRepo;
        }

        // 1. Get appointments by DoctorId
        public async Task<IEnumerable<AppointmentViewDto>> GetAppointmentsByDoctorId(int doctorId)
        {
            var appointments = await _appointmentRepo.GetAppointmentsByDoctorIdAsync(doctorId);

            return appointments.Select(a => new AppointmentViewDto
            {
                AppointmentId = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Symptoms = a.Symptoms,
                PatientId = a.PatientId,
                PatientName = a.Patient?.User?.FullName ?? "Unknown",
                ContactNo = a.Patient?.User?.ContactNo ?? "N/A"
            });
        }


        public async Task<AppointmentViewDto?> UpdateAppointmentStatus(int appointmentId, string newStatus)
        {
            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
                return null;

            appointment.Status = newStatus;

            var updatedAppointment = await _appointmentRepo.UpdateAppointmentAsync(appointment);
            if (updatedAppointment == null) return null;

            return new AppointmentViewDto
            {
                AppointmentId = updatedAppointment.AppointmentId,
                AppointmentDate = updatedAppointment.AppointmentDate,
                Status = updatedAppointment.Status,
                Symptoms = updatedAppointment.Symptoms,
                PatientId = updatedAppointment.PatientId,
                PatientName = updatedAppointment.Patient?.User?.FullName ?? "Unknown",
                ContactNo = updatedAppointment.Patient?.User?.ContactNo ?? "N/A"
            };
        }

        public async Task<IEnumerable<AppointmentViewDto>> FilterAppointments(int doctorId, AppointmentFilterDto filter)
        {
            var appointments = await _appointmentRepo.GetAppointmentsByDoctorIdAsync(doctorId);

            var filtered = ApplyFilters(appointments.ToList(), filter);

            return filtered.Select(a => new AppointmentViewDto
            {
                AppointmentId = a.AppointmentId,
                AppointmentDate = a.AppointmentDate,
                Status = a.Status,
                Symptoms = a.Symptoms,
                PatientId = a.PatientId,
                PatientName = a.Patient?.User?.FullName ?? "Unknown",
                ContactNo = a.Patient?.User?.ContactNo ?? "N/A"
            });
        }

        public async Task<PatientDetailsDto> GetPatientDetailsForAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepo.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
                throw new Exception("Appointment not found");

            var patient = await _patientRepo.GetById(appointment.PatientId);
            if (patient == null)
                throw new Exception("Patient not found");

            return new PatientDetailsDto
            {
                PatientId = patient.PatientId,
                FullName = patient.User?.FullName ?? "N/A",
                Gender = patient.User?.Gender ?? "N/A",
                ContactNo = patient.User?.ContactNo ?? "N/A",
                DateOfBirth = patient.User?.DateOfBirth ?? DateTime.MinValue,
                MedicalHistory = patient.MedicalHistory ?? "N/A"
            };
        }

        private List<Appointment> ApplyFilters(List<Appointment> appointments, AppointmentFilterDto filter)
        {
            var query = appointments.AsQueryable();

            // Filter by Status (case-insensitive)
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(a => a.Status.Equals(filter.Status, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by From Date only
            if (filter.DateRange?.From != null)
            {
                query = query.Where(a => a.AppointmentDate >= filter.DateRange.From.Value.Date);
            }

            // Filter by To Date only
            if (filter.DateRange?.To != null)
            {
                query = query.Where(a => a.AppointmentDate <= filter.DateRange.To.Value.Date);
            }

            return query.ToList();
        }



    }
}
