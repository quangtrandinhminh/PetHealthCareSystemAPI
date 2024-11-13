using Repository.Extensions;
using Repository.Models.Appointment;
using Repository.Models.Cage;
using Repository.Models.Hospitalization;
using Repository.Models.MedicalRecord;
using Repository.Models.TimeTable;
using Repository.Models.User;

namespace Service.IServices;

public interface IHospitalizationService
{
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo);
    Task<List<CageResponseDto>> GetAvailableCage();
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize);
    Task<HospitalizationResponseDtoWithDetails> GetHospitalizationById(int hospitalizationId);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationWithFilters(HospitalizationFilterDto filter,
        int pageNumber, int pageSize);
    Task<CageResponseDto> GetCurrentCageByMedicalRecordId(int medicalRecordId);
    Task CreateHospitalization(HospitalizationRequestDto dto ,int staffId);
    Task UpdateHospitalization(HospitalizationUpdateRequestDto dto, int vetId);
    Task DeleteHospitalization(int hospitalizationId, int deleteBy);
    Task<List<HospitalizationResponseDto>> GetListHospitalizationByMRId(int medicalRecordId);
    Task<List<MedicalRecordResponseDto>> GetAllPetInHospitalization();
    Task<List<HospitalizationResponseDto>> CheckHospitalizaionByVetId(int vetId);
    Task<List<HospitalizationResponseDto>> CheckCreateHospitalization(int medicalRecordId);
    Task HospitalDischarge(int medicalRecordId, int VetId);
    HospitalizaionDropdownDto GetHospitalizaionDropdownData();
}