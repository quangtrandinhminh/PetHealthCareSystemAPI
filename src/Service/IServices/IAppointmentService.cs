using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;

namespace Service.IServices;

public interface IAppointmentService
{
    /*Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync();
    Task CreateAppointmentAsync(AppointmentRequestDto appointment);
    Task UpdateAppointmentAsync(AppointmentUpdateRequestDto appointment);
    Task DeleteAppointmentAsync(int id);*/
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateOnly date, int timetableId);
}