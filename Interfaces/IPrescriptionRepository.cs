using AmazeCare.Models;

namespace AmazeCare.Interfaces
{
    public interface IPrescriptionRepository :  IRepository<int, Prescription>
    {
        Task<List<PrescriptionDetail>> GetDetailsByAppointmentId(int appointmentId);
        Task AddDetail(PrescriptionDetail detail);
    }
}
