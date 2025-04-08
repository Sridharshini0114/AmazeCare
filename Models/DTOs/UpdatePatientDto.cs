public class UpdatePatientDto
{
    public int UserId { get; set; }

    public FullNameUpdateDto? FullNameUpdate { get; set; }
    public ContactNoUpdateDto? ContactNoUpdate { get; set; }
    public GenderUpdateDto? GenderUpdate { get; set; }
    public DateOfBirthUpdateDto? DateOfBirthUpdate { get; set; }
    public MedicalHistoryUpdateDto? MedicalHistoryUpdate { get; set; }
}
    public class FullNameUpdateDto { public string NewFullName { get; set; } }
    public class ContactNoUpdateDto { public string NewContactNo { get; set; } }
    public class GenderUpdateDto { public string NewGender { get; set; } }
    public class DateOfBirthUpdateDto { public DateTime NewDateOfBirth { get; set; } }
    public class MedicalHistoryUpdateDto { public string NewMedicalHistory { get; set; } }
