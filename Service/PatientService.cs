using AmazeCare.Interfaces;
using AmazeCare.Misc;
using AmazeCare.Models;
using AmazeCare.Models.DTOs;
using AmazeCare.Repositories;
using AmazeCare.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AmazeCare.Business
{
    public class PatientService : IPatientService
    {
        private readonly IUserRepository _userRepository; // Interface instead of class
        private readonly IPatientRepository _patientRepository;
        private readonly TokenService _tokenService;
   

        public PatientService(
            IUserRepository userRepository, // Inject interface here
            IPatientRepository patientRepository,
            TokenService tokenService,
            IDGeneratorService idGenerator)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _tokenService = tokenService;
           
        }




        public async Task<LoginResponseDto> RegisterPatient(PatientUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmail(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            var role = await _userRepository.GetRoleByName("patient");
            if (role == null)
                throw new Exception("Role 'patient' not found");

          

            var user = new User
            {
                
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                ContactNo = dto.ContactNo,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };

            user = await _userRepository.AddUser(user);

            if (dto.RoleName.ToLower() == "patient")
            {
          
                var patient = new Patient
                {
                    UserId = user.UserId,
                    MedicalHistory = dto.MedicalHistory
                };
                await _patientRepository.Add(patient);
            }

            var token = _tokenService.GenerateToken(user, role.RoleName);

            return new LoginResponseDto
            {
                Token = token,
                Role = role.RoleName,
                UserId = user.UserId
            };
        }


        public async Task<IEnumerable<PatientUserViewDto>> GetAllPatients()
        {
            var patients = await _patientRepository.GetAll();

            return patients.Select(p => new PatientUserViewDto
            {
                PatientId = p.PatientId,
                UserId = p.UserId,
                FullName = p.User?.FullName ?? "",
                Email = p.User?.Email ?? "",
                ContactNo = p.User?.ContactNo ?? "",
                Gender = p.User?.Gender ?? "",
                DateOfBirth = p.User?.DateOfBirth ?? DateTime.MinValue
            }).ToList();
        }


        public async Task<LoginResponseDto> Login(LoginDto dto)
        {
            var user = await _userRepository.GetUserByEmailAndPassword(dto.Email, dto.Password);
            if (user == null)
                throw new Exception("Invalid email or password");

            var token = _tokenService.GenerateToken(user, user.Role!.RoleName);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role.RoleName,
                UserId = user.UserId
            };
        }

        public async Task<bool> DeleteUserWithPatientAsync(int userId)
        {
            // Delete Patient first (if exists)
            await _patientRepository.DeletePatient(userId);

            // Then delete User
            return await _userRepository.DeleteUser(userId);
        }

        public async Task<IEnumerable<Patient>> FilterPatientsAsync(PatientFilterDto filters, int? sortBy = null)
        {
            var patients = await _patientRepository.GetAllWithUserDetails();

            var filtered = ApplyFilters(patients.ToList(), filters, sortBy);

            if (!filtered.Any())
                throw new Exception("No patients found");

            return filtered;
        }


        private List<Patient> SortPatients(int sortBy, List<Patient> patients)
        {
            switch (sortBy)
            {
                case -4:
                    patients = patients.OrderByDescending(p => p.User.FullName).ToList();
                    break;
                case -3:
                    patients = patients.OrderByDescending(p => p.User.DateOfBirth).ToList();
                    break;
                case -2:
                    patients = patients.OrderByDescending(p => p.User.Gender).ToList();
                    break;
                case -1:
                    patients = patients.OrderByDescending(p => p.UserId).ToList();
                    break;
                case 1:
                    patients = patients.OrderBy(p => p.UserId).ToList();
                    break;
                case 2:
                    patients = patients.OrderBy(p => p.User.FullName).ToList();
                    break;
                case 3:
                    patients = patients.OrderBy(p => p.User.DateOfBirth).ToList();
                    break;
                case 4:
                    patients = patients.OrderBy(p => p.User.Gender).ToList();
                    break;
            }

            return patients;
        }


        public List<Patient> ApplyFilters(List<Patient> patients, PatientFilterDto filters, int? sortBy = null)
        {
            var query = patients.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(filters.FullName))
            {
                query = query.Where(p => p.User != null &&
                    p.User.FullName.ToLower().Contains(filters.FullName.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(filters.ContactNo))
            {
                query = query.Where(p => p.User != null &&
                    p.User.ContactNo.Contains(filters.ContactNo));
            }

            if (!string.IsNullOrWhiteSpace(filters.Gender))
            {
                query = query.Where(p => p.User != null &&
                    p.User.Gender.Equals(filters.Gender, StringComparison.OrdinalIgnoreCase));
            }

            if (filters.DateOfBirthRange?.From != null && filters.DateOfBirthRange?.To != null)
            {
                query = query.Where(p => p.User != null &&
                    p.User.DateOfBirth >= filters.DateOfBirthRange.From &&
                    p.User.DateOfBirth <= filters.DateOfBirthRange.To);
            }

            var result = query.ToList();

            // Sorting
            if (sortBy.HasValue)
            {
                result = SortPatients(sortBy.Value, result);
            }

            if (!result.Any())
                throw new Exception("No patients found matching the criteria.");

            return result;
        }



    }
}
