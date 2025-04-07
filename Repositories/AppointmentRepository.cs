using AmazeCare.Contexts;
using AmazeCare.Interfaces;
using AmazeCare.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazeCare.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AmazecareContext _context;

        public AppointmentRepository(AmazecareContext context)
        {
            _context = context;
        }

        public async Task<Appointment> ScheduleAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor).ThenInclude(d => d.User)
                .Include(a => a.Patient).ThenInclude(p => p.User)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
