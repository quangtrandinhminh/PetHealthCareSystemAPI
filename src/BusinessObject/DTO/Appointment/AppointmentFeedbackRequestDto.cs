namespace BusinessObject.DTO.Appointment;

public class AppointmentFeedbackRequestDto
{
    public short Rating { get; set; }
    public string Feedback { get; set; }
    public int AppointmentId { get; set; }
}