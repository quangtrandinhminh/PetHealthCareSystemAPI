using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Mapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Repository.Interfaces;
using Service.IServices;

namespace Service.Services;

public class AppointmentService(IServiceProvider serviceProvider) : IAppointmentService
{
    private readonly ITimeTableRepository _timeTableRepo =
        serviceProvider.GetRequiredService<ITimeTableRepository>();
    private readonly IAppointmentRepository _appointmentRepo =
        serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();

    public async Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync()
    {
        var timetables = await _timeTableRepo.GetAllBookingTimeFramesAsync();
        var response = _mapper.Map(timetables);

        return response.ToList();
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDate(DateOnly date, int timetableId)
    {
        var vetList = (await _userService.GetVetsAsync()).ToList();
        var appointmentList = (await _appointmentRepo.GetAllAsync()).Where(e => e.AppointmentDate == date && e.TimeTableId == timetableId);

        var freeVetList = vetList.Where(e => !appointmentList.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }
}   