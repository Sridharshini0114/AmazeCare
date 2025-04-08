using AmazeCare.Contexts;
using AmazeCare.Models;
using Microsoft.EntityFrameworkCore;
using AmazeCare.Interfaces;

namespace AmazeCare.Repositories
{
    public class MedicalRecordRepository : Repository<int, MedicalRecord>, IMedicalRecordRepository
    {
        public MedicalRecordRepository(AmazecareContext context) : base(context) { }

        public override async Task<IEnumerable<MedicalRecord>> GetAll()
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient).ThenInclude(p => p.User)
                .Include(m => m.Doctor).ThenInclude(d => d.User)
                .Include(m => m.RecommendedTests).ThenInclude(rt => rt.MedicalTest)
                .ToListAsync();
        }

        public override async Task<MedicalRecord> GetById(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient).ThenInclude(p => p.User)
                .Include(m => m.Doctor).ThenInclude(d => d.User)
                .Include(m => m.Appointment)
                .Include(m => m.RecommendedTests).ThenInclude(rt => rt.MedicalTest)
                .FirstOrDefaultAsync(m => m.RecordId == id);
        }

        public async Task<MedicalRecord?> GetByAppointmentId(int appointmentId)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient).ThenInclude(p => p.User)
                .Include(m => m.Doctor).ThenInclude(d => d.User)
                .Include(m => m.Appointment)
                .Include(m => m.RecommendedTests).ThenInclude(rt => rt.MedicalTest)
                .FirstOrDefaultAsync(m => m.AppointmentId == appointmentId);
        }
    }
}
