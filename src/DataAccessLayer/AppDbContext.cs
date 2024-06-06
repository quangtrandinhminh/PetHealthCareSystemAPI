using System;
using System.Collections.Generic;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utility.Enum;

namespace Repository;

public partial class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public AppDbContext()
    {

    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentService> AppointmentServices { get; set; }

    public virtual DbSet<Cage> Cages { get; set; }

    public virtual DbSet<Configuration> Configurations { get; set; }

    public virtual DbSet<Hospitalization> Hospitalizations { get; set; }

    public virtual DbSet<MedicalItem> MedicalItems { get; set; }

    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Timetable> Timetables { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    public string GetConnectionString()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        return configuration.GetConnectionString("DefaultConnection");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = UserRole.Admin.ToString(),
                NormalizedName = UserRole.Admin.ToString().ToUpper()
            },
            new IdentityRole
            {
                Name = UserRole.Customer.ToString(),
                NormalizedName = UserRole.Customer.ToString().ToUpper()
            },
            new IdentityRole
            {
                Name = UserRole.Staff.ToString(),
                NormalizedName = UserRole.Staff.ToString().ToUpper()
            },
            new IdentityRole
            {
                Name = UserRole.Vet.ToString(),
                NormalizedName = UserRole.Vet.ToString().ToUpper()
            },
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}
