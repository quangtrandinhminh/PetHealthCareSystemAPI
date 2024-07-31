using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Repository.Extensions;
using Utility.Enum;

namespace Service.IServices;

public interface IHospitalizationService
{
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo);
    Task<List<CageResponseDto>> GetAvailableCageByDate();
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize);
    Task<HospitalizationResponseDto> GetHospitalizationById(int hospitalizationId);
    List<EnumResponseDto> GetHospitalizationStatus();
    Task CreateHospitalization(HospitalizationRequestDto dto ,int staffId);
    Task UpdateHospitalization(HospitalizationRequestDto dto, int vetId);
    Task DeleteHospitalization(int id, int deleteBy);
    Task HospitalDischarge(int medicalRecordId, int VetId);
    HospitalizaionDropdownDto GetHospitalizaionDropdownData();
    Task<List<MedicalRecordResponseDto>> GetAllPetInHospitalization();
    Task<List<HospitalizationResponseDto>> CheckHospitalizaionByVetId(int vetId);
    Task<List<HospitalizationResponseDto>> CheckCreateHospitalization(int medicalRecordId);
}