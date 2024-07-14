using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
using Repository.Interfaces;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using System.Collections.Generic;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service.Services;

public class HospitalizationService(IServiceProvider serviceProvider) : IHospitalizationService
{
    private readonly ILogger _logger = Log.Logger;
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepo = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepo = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ITimeTableRepository _timeTableRepo =
        serviceProvider.GetRequiredService<ITimeTableRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();
    private readonly ICageRepository _cageRepository = serviceProvider.GetRequiredService<ICageRepository>();

    public async Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync()
    {
        var timetables = _timeTableRepo.GetAllWithCondition(t => t.Type == TimeTableType.Hospitalization);
        var response = _mapper.Map(timetables);

        return await response.ToListAsync();
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo)
    {
        if (!DateOnly.TryParse(qo.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (qo.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        var vetList = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);

        var hospitalizationsList = _hospitalizationRepo.GetAllWithCondition(e => e.Date == date
                                                                        && e.TimeTableId == qo.TimetableId);

        var freeVetList = vetList.Where(e => !hospitalizationsList.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }

    public async Task<List<CageResponseDto>> GetAvailableCageByDate()
    {
        var listCage = await _cageRepository.FindByConditionAsync(x => x.IsAvailable == true);

        var listCageDto = _mapper.Map(listCage);
        return (List<CageResponseDto>)listCageDto;
    }

    public async Task<List<HospitalizationResponseDto>> GetAllHospitalization()
    {
        var list = await _hospitalizationRepo.GetAllHospitalization();

        foreach (var hospitalization in list)
        {
            hospitalization.TimeTable = _timeTableRepo.GetAll().Where(x => x.Id == hospitalization.TimeTableId).FirstOrDefault();
        }

        var listDto = _mapper.Map(list);

        foreach (var hospitalization in listDto)
        {
            hospitalization.Vet = await _userService.GetVetByIdAsync(hospitalization.VetId);
        }

        return listDto.ToList();
    }

    public async Task<List<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId)
    {
        var findHospitalization = _hospitalizationRepo.GetAll().Where(x => x.MedicalRecordId == medicalRecordId).ToList();

        var findHospitalizationDto = _mapper.Map(findHospitalization);

        return (List<HospitalizationResponseDto>)findHospitalizationDto;
    }

    public async Task<HospitalizationResponseDto> GetHospitalizationById(int hospitalizationId)
    {
        var findHospitalization = _hospitalizationRepo.GetAll().Where(x => x.Id == hospitalizationId).FirstOrDefault();

        var findHospitalizationDto = _mapper.Map(findHospitalization);

        return findHospitalizationDto;
    }

    public List<EnumResponseDto> GetHospitalizationStatus()
    {
        _logger.Information("Get hospitalization status");
        throw new NotImplementedException();
    }

    public async Task CreateHospitalization(HospitalizationRequestDto dto, int staffId)
    {
        var vetList = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);

        var hospitalizationsList = _hospitalizationRepo.GetAllWithCondition(e => e.Date == dto.Date
                                                                        && e.TimeTableId == dto.TimeTableId);

        var VetList = vetList.Where(e => hospitalizationsList.Any(ee => ee.VetId == e.Id)).FirstOrDefault();

        if (VetList != null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
               ResponseMessageConstantsUser.VET_EXISTED);
        }
        else
        {
            var medicalRecord = await _medicalRecordRepo.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId,
            false, mr => mr.MedicalItems, mr => mr.Pet,
            mr => mr.Hospitalization);
            if (medicalRecord == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                    ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
            }
            if (medicalRecord.AdmissionDate == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED,
                    ResponseMessageConstantsHospitalization.MEDICAL_RECORD_NOT_ADMITTED, StatusCodes.Status400BadRequest);
            }
            if (medicalRecord.DischargeDate != null)
            {
                throw new AppException(ResponseCodeConstants.FAILED,
                    ResponseMessageConstantsHospitalization.MEDICAL_RECORD_ALREADY_DISCHARGED, StatusCodes.Status400BadRequest);
            }
            // check vet
            var vet = await _userService.GetVetByIdAsync(dto.VetId);
            if (vet == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND);
            }

            if (dto.Date == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
            }

            // check timetable
            var timeTable = await _timeTableRepo.GetSingleAsync(e => e.Id == dto.TimeTableId && e.Type == TimeTableType.Hospitalization);
            if (timeTable == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                    ResponseMessageConstantsTimetable.TIMETABLE_NOT_FOUND, StatusCodes.Status404NotFound);
            }

            // check cage
            var cage = await _cageRepository.GetSingleAsync(e => e.Id == dto.CageId);
            if (cage == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                   ResponseMessageConstantsCage.CAGE_NOT_FOUND, StatusCodes.Status404NotFound);
            }

            var hospitalzation = _mapper.Map(dto);
            hospitalzation.CreatedBy = staffId;
            hospitalzation.HospitalizationDateStatus = HospitalizationStatus.Admissions;

            cage.IsAvailable = false;

            await _cageRepository.UpdateAsync(cage);

            await _hospitalizationRepo.CreateHospitalizationAsync(hospitalzation);
        }
    }

    public async Task UpdateHospitalization(HospitalizationRequestDto dto, int vetId)
    {
        var medicalRecord = await _medicalRecordRepo.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId,
            false, mr => mr.MedicalItems, mr => mr.Pet,
            mr => mr.Hospitalization);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        if (medicalRecord.AdmissionDate == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED,
                ResponseMessageConstantsHospitalization.MEDICAL_RECORD_NOT_ADMITTED, StatusCodes.Status400BadRequest);
        }
        if (medicalRecord.DischargeDate != null)
        {
            throw new AppException(ResponseCodeConstants.FAILED,
                ResponseMessageConstantsHospitalization.MEDICAL_RECORD_ALREADY_DISCHARGED, StatusCodes.Status400BadRequest);
        }
        // check vet
        var vet = await _userService.GetVetByIdAsync(dto.VetId);
        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND);
        }

        if (dto.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        // check timetable
        var timeTable = await _timeTableRepo.GetSingleAsync(e => e.Id == dto.TimeTableId && e.Type == TimeTableType.Hospitalization);
        if (timeTable == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsTimetable.TIMETABLE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        // check cage
        var cage = await _cageRepository.GetSingleAsync(e => e.Id == dto.CageId);
        if (cage == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsCage.CAGE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var hospitalzation = _mapper.Map(dto);
        hospitalzation.HospitalizationDateStatus = HospitalizationStatus.Monitoring;
        hospitalzation.LastUpdatedBy = vetId;
        hospitalzation.LastUpdatedTime = DateTimeOffset.Now;

        await _hospitalizationRepo.UpdateAsync(hospitalzation);
    }

    public async Task DeleteHospitalization(int id, int deleteBy)
    {
        var hospitalzation = _hospitalizationRepo.GetById(id);
        hospitalzation.DeletedTime = DateTimeOffset.Now;
        hospitalzation.DeletedBy = deleteBy;

        await _hospitalizationRepo.UpdateAsync(hospitalzation);
    }


    public async Task HospitalDischarge(int medicalRecordId, int VetId)
    {
        var findHospitalization = _hospitalizationRepo.GetAll().Where(x => x.MedicalRecordId == medicalRecordId).ToList();

        var cage = _cageRepository.GetById(findHospitalization.ElementAt(0).CageId);
        cage.IsAvailable = true;
        await _cageRepository.UpdateCageAsync(cage);

        foreach (var hospital in findHospitalization)
        {
            hospital.HospitalizationDateStatus = HospitalizationStatus.DischargeDate;
            hospital.LastUpdatedBy = VetId;
            hospital.LastUpdatedTime = DateTimeOffset.Now;
            await _hospitalizationRepo.UpdateAsync(hospital);
        }
    }

    public HospitalizaionDropdownDto GetHospitalizaionDropdownData()
    {
        _logger.Information("Get all dropdown data for Hospitalizaion");
        var hospitalizationStatus = Enum.GetValues(typeof(HospitalizationStatus))
            .Cast<HospitalizationStatus>()
            .Select(e => new EnumResponseDto() { Id = (int)e, Value = e.ToString() })
            .ToList();

        var response = new HospitalizaionDropdownDto
        {
            HospitalizationStatus = hospitalizationStatus,
        };

        return response;
    }
}