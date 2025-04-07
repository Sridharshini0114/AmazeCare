using AmazeCare.Models.DTOs;
using AmazeCare.Models;

namespace AmazeCare.Interfaces
{
    public interface IPatientService
    {
        Task<LoginResponseDto> RegisterPatient(PatientUserDto dto);
        Task<LoginResponseDto> Login(LoginDto dto);
        Task<IEnumerable<PatientUserViewDto>> GetAllPatients();
        Task<bool> DeleteUserWithPatientAsync(int userId);
        Task<IEnumerable<Patient>> FilterPatientsAsync(PatientFilterDto filters, int? sortBy = null);
    }
}
