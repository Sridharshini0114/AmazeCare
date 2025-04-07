using AmazeCare.Interfaces;
using AmazeCare.Models;
using AmazeCare.Models.DTOs;

using AmazeCare.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AmazeCare.Business
{
    public class PatientUpdateService : IPatientUpdateService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        public PatientUpdateService(IUserRepository userRepository, IPatientRepository patientRepository)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
        }



        public async Task<bool> UpdatePatientAsync(UpdatePatientDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Update request cannot be null.");

            var user = await _userRepository.GetUserById(dto.UserId);
            if (user == null)
                throw new Exception("User not found.");

            var patient = await _patientRepository.GetByUserId(dto.UserId);
            if (patient == null)
                throw new Exception("Patient not found.");

            // Validate existing fields before updating
            if (user.ContactNo == null)
                throw new Exception("ContactNo is null in the database.");

            if (user.FullName == null)
                throw new Exception("FullName is null in the database.");

            if (user.Gender == null)
                throw new Exception("Gender is null in the database.");

            if (user.DateOfBirth == default)
                throw new Exception("DateOfBirth is not set in the database.");

            if (patient.MedicalHistory == null)
                throw new Exception("MedicalHistory is null in the database.");

            // Only update if new values are provided
            if (dto.FullNameUpdate != null && !string.IsNullOrEmpty(dto.FullNameUpdate.NewFullName))
                user.FullName = dto.FullNameUpdate.NewFullName;

            if (dto.ContactNoUpdate != null && !string.IsNullOrEmpty(dto.ContactNoUpdate.NewContactNo))
                user.ContactNo = dto.ContactNoUpdate.NewContactNo;

            if (dto.GenderUpdate != null && !string.IsNullOrEmpty(dto.GenderUpdate.NewGender))
                user.Gender = dto.GenderUpdate.NewGender;

            if (dto.DateOfBirthUpdate != null)
                user.DateOfBirth = dto.DateOfBirthUpdate.NewDateOfBirth;

            if (dto.MedicalHistoryUpdate != null && !string.IsNullOrEmpty(dto.MedicalHistoryUpdate.NewMedicalHistory))
                patient.MedicalHistory = dto.MedicalHistoryUpdate.NewMedicalHistory;

            // Save changes
            await _userRepository.UpdateUser(user);
            await _patientRepository.Update(patient);

            return true;
        }



    }
}
