﻿using Azure;
using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
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
using System.Diagnostics.Eventing.Reader;
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

    public async Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize)
    {
        var list = _hospitalizationRepo.GetAll();

        foreach (var hospitalization in list)
        {
            hospitalization.TimeTable = _timeTableRepo.GetAll().Where(x => x.Id == hospitalization.TimeTableId).FirstOrDefault();
        }

        var listDto = _mapper.Map(list);

       var paginatedList = await PaginatedList<HospitalizationResponseDto>.CreateAsync(listDto, pageNumber, pageSize);

        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(e => e.Id == item.VetId);
            if (vet != null) item.Vet = _mapper.UserToUserResponseDto(vet);
        }
        return paginatedList;
    }

    public async Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize)
    {
        var findHospitalization = _hospitalizationRepo.GetAll().Where(x => x.MedicalRecordId == medicalRecordId);

        var findHospitalizationDto = _mapper.Map(findHospitalization);

        return await PaginatedList<HospitalizationResponseDto>.CreateAsync(findHospitalizationDto, pageNumber, pageSize);
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
        if (!DateOnly.TryParse(dto.Date , out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        var hospitalizationsList = _hospitalizationRepo.GetAllWithCondition(e => e.Date == date
                                                                        && e.TimeTableId == dto.TimeTableId);

        var VetList = _userRepository.GetSingleAsync(e => hospitalizationsList.Any(ee => ee.VetId == e.Id));

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
            hospitalzation.Date = date;

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
        
        if (!DateOnly.TryParse(dto.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

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
        hospitalzation.Date = date;

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

    public async Task<List<MedicalRecordResponseDto>> GetAllPetInHospitalization()
    {
        var medicalRecordList = _medicalRecordRepo.GetAll().Where(e => e.AdmissionDate != null && e.DischargeDate == null);

        var medicalRecordListDto = _mapper.Map(medicalRecordList);

        return (List<MedicalRecordResponseDto>)medicalRecordListDto;
    }

    public async Task<List<HospitalizationResponseDto>> CheckHospitalizaionByVetId(int vetId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var hospitalizationList = _hospitalizationRepo.GetAll().Where(e => e.Date == today && e.VetId == vetId).ToList();

        if (hospitalizationList == null || !hospitalizationList.Any())
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsHospitalization.DO_NOT_HAVE_TIMETABLE);
        }

        foreach (var ho in hospitalizationList)
        {
            ho.TimeTable = _timeTableRepo.GetAll().Where(x => x.Id == ho.TimeTableId).FirstOrDefault();
        }

        var hospitalizationResponseList = _mapper.Map(hospitalizationList);

        foreach (var item in hospitalizationResponseList)
        {
            var vet = await _userRepository.GetSingleAsync(e => e.Id == item.VetId);
            if (vet != null) item.Vet = _mapper.UserToUserResponseDto(vet);
        }

        return (List<HospitalizationResponseDto>)hospitalizationResponseList;
    }

    public async Task<List<HospitalizationResponseDto>> CheckCreateHospitalization(int medicalRecordId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var hospitalizationList = _hospitalizationRepo.GetAll().Where(e => e.MedicalRecordId == medicalRecordId).OrderByDescending(x => x.Date);

        if (hospitalizationList.Any() && hospitalizationList.First().Date == today)
        {
            foreach (var ho in hospitalizationList)
            {
                ho.TimeTable = _timeTableRepo.GetAll().FirstOrDefault(x => x.Id == ho.TimeTableId);
            }

            var hospitalizationResponseList = _mapper.Map(hospitalizationList.ToList());

            foreach (var item in hospitalizationResponseList)
            {
                var vet = await _userRepository.GetSingleAsync(e => e.Id == item.VetId);
                if (vet != null) item.Vet = _mapper.UserToUserResponseDto(vet);
            }

            return (List<HospitalizationResponseDto>)hospitalizationResponseList;
        }
        else
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND);
        }
    }

}