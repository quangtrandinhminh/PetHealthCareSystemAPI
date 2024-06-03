using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("User")]
[Index(nameof(User.Username), IsUnique = true)]
[Index(nameof(User.Email), IsUnique = true)]
[Index(nameof(User.Phone), IsUnique = true)]
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string OTP { get; set; }
    public DateTimeOffset? OTPExpiry { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    public UserRole Role { get; set; }
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    
    // for customer
    public ICollection<Pet>? Pets { get; set; }
    
    // for staff and customer
    public ICollection<Appointment>? CreatedAppointments { get; set; }
    
    // for vet
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    public virtual ICollection<TimeTable> TimeTables { get; set; }
}