using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BusinessObject.Entities;

public partial class User : IdentityUser
{
    public string? FullName { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? Role { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public virtual ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
