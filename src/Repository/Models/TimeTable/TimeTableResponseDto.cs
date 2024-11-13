namespace Repository.Models.TimeTable;

public class TimeTableResponseDto
{
    public int Id { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}