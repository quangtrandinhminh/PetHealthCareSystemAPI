using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface IMedicalRecordRepository : IBaseRepository<MedicalRecord>
{
    Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord);
}