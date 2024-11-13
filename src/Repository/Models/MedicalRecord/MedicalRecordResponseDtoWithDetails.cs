namespace Repository.Models.MedicalRecord;

public class MedicalRecordResponseDtoWithDetails : MedicalRecordResponseDto
{
    public string? Note { get; set; }
    public List<MedicalItemMRResponseDto> MedicalItems { get; set; }
}