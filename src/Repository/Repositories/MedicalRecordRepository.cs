using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class MedicalRecordRepository : BaseRepository<MedicalRecord>, IMedicalRecordRepository
{
    public async Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord)
    {
        try
        {
            await using var db = new AppDbContext();

            foreach (var item in medicalRecord.MedicalItems.ToList())
            {
                var existingService = db.MedicalItems.SingleOrDefault(e => e.Id == item.Id);

                if (existingService != null)
                {
                    // Attach the existing tag
                    db.Entry(existingService).State = EntityState.Unchanged;
                    medicalRecord.MedicalItems.Remove(item);
                    medicalRecord.MedicalItems.Add(existingService);
                }
            }

            var addedMedicalRecord = await db.MedicalRecords.AddAsync(medicalRecord);
            await db.SaveChangesAsync();

            return addedMedicalRecord.Entity;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}