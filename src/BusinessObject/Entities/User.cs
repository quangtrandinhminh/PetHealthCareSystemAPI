using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("User")]
public class User : IdentityUser
{
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
    // Base Property
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    // for customer
    public ICollection<Pet>? Pets { get; set; }
    
    // for customer
    public ICollection<Appointment>? CreatedAppointments { get; set; }
    
    // for vet
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}