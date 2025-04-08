using AmazeCare.Models;

namespace AmazeCare.Interfaces
{
    public interface IMedicalRecordRepository : IRepository<int, MedicalRecord>
    {
        Task<MedicalRecord?> GetByAppointmentId(int appointmentId);

    }
}
