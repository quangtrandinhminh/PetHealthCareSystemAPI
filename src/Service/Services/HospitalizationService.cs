using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
using Repository.Interfaces;
using Serilog;
using Service.IServices;

namespace Service.Services;

public class HospitalizationService(IServiceProvider serviceProvider) : IHospitalizationService
{
    private readonly ILogger _logger = Log.Logger;
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepo = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepo = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

    public Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo)
    {
        throw new NotImplementedException();
    }

    public Task<List<CageResponseDto>> GetAvailableCageByDate(DateTimeQueryDto qo)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize)
    {
        _logger.Information("Get all hospitalization");
        throw new NotImplementedException();
    }

    public Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all hospitalization by medical record id {medicalRecordId}");
        throw new NotImplementedException();
    }

    public Task<HospitalizationResponseDtoWithDetails> GetHospitalizationById(int hospitalizationId)
    {
        _logger.Information($"Get hospitalization by id {hospitalizationId}");
        throw new NotImplementedException();
    }

    public List<EnumResponseDto> GetHospitalizationStatus()
    {
        _logger.Information("Get hospitalization status");
        throw new NotImplementedException();
    }

    public Task CreateHospitalization(HospitalizationRequestDto dto, int staffId)
    {
        _logger.Information("Create hospitalization {@dto} by {staffId}", dto, staffId);
        throw new NotImplementedException();
    }

    public Task UpdateHospitalization(HospitalizationRequestDto dto, int vetId)
    {
        _logger.Information("Update hospitalization {@dto} by {vetId}", dto, vetId);
        throw new NotImplementedException();
    }

    public Task DeleteHospitalization(int id, int deleteBy)
    {
        _logger.Information($"Delete hospitalization by id {id} by {deleteBy}");
        throw new NotImplementedException();
    }
}