﻿using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using Repository.Extensions;

namespace Service.IServices;

public interface IAppointmentService
{
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo);
    Task<AppointmentResponseDto> GetAppointmentByAppointmentId(int appointmentId);
    Task<PaginatedList<AppointmentResponseDto>> GetAllAppointmentsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<AppointmentResponseDto>> GetVetAppointmentsAsync(int vetId, string date, int pageNumber, int pageSize);
    Task<PaginatedList<AppointmentResponseDto>> GetUserAppointmentsAsync(int pageNumber, int pageSize, int id, string date);
    Task<PaginatedList<AppointmentResponseDto>> GetAppointmentWithFilter(AppointmentFilterDto filter, int pageNumber,
        int pageSize);
    Task<AppointmentResponseDto> BookAppointmentAsync(AppointmentBookRequestDto appointmentBookRequestDto, int customerId);
    Task<AppointmentResponseDto> UpdateStatusToDone(int appointmentId, int vetId);
    Task<AppointmentResponseDto> UpdateStatusToCancel(int appointmentId, int updatedById);
    Task<AppointmentResponseDto> FeedbackAppointmentAsync(AppointmentFeedbackRequestDto dto, int ownerId);
    Task<AppointmentResponseDto> UpdateOnlinePaymentToTrue(int appointmentId, int updatedById);
    Task<PaginatedList<AppointmentResponseDto>> GetAllCancelAppointmentsAsync(int pageNumber, int pageSize);
    Task<AppointmentResponseDto> UpdateRefundStatusToTrue(int appointmentId, int updatedById);
}