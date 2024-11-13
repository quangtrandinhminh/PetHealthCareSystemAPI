using Repository.Models.TimeTable;
using Repository.Models.User;

namespace Repository.Models.Hospitalization;

public class HospitalizationResponseDto
{
    public int Id { get; set; }
    public int MedicalRecordId { get; set; }
    public int CageId { get; set; }
    public TimeTableResponseDto TimeTable { get; set; }
    public DateOnly Date { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public int VetId { get; set; }
    public string HospitalizationDateStatus { get; set; }
    public UserResponseDto Vet { get; set; }

}