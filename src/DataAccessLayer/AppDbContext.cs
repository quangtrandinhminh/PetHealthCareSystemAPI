﻿using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utility.Enum;

namespace Repository;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public AppDbContext()
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TimeTable> TimeTables { get; set; }
    public DbSet<Cage> Cage { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Pet)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(m => m.PetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Appointment)
            .WithMany(a => a.MedicalRecords)
            .HasForeignKey(m => m.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Vet)
            .WithMany(u => u.MedicalRecords)
            .HasForeignKey(m => m.VetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MedicalRecord>()
            .HasOne(m => m.Service)
            .WithMany(s => s.MedicalRecords)
            .HasForeignKey(m => m.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }*/

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
        builder.Entity<IdentityRole>().HasData(roles);
    }
}