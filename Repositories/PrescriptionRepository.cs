using AmazeCare.Contexts;
using AmazeCare.Models;
using Microsoft.EntityFrameworkCore;
using AmazeCare.Interfaces;

namespace AmazeCare.Repositories
{
    public class PrescriptionRepository : Repository<int, Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(AmazecareContext context) : base(context) { }

        public override async Task<IEnumerable<Prescription>> GetAll()
        {
            return await _context.Prescriptions.ToListAsync();
        }

        public override async Task<Prescription> GetById(int id)
        {
            return await _context.Prescriptions.FirstOrDefaultAsync(p => p.PrescriptionId == id);
        }

        public async Task<List<PrescriptionDetail>> GetDetailsByAppointmentId(int appointmentId)
        {
            return await _context.PrescriptionDetails
                .Include(p => p.Medicine)
                .Include(p => p.FoodTiming)
                .Include(p => p.Prescription) // 👈 needed to access AppointmentId
                .Where(p => p.Prescription != null && p.Prescription.AppointmentId == appointmentId)
                .ToListAsync();
        }

        public async Task AddDetail(PrescriptionDetail detail)
        {
            _context.PrescriptionDetails.Add(detail);
            await _context.SaveChangesAsync();
        }
    }
}
