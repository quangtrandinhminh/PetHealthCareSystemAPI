using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface IMedicalItemRepository : IBaseRepository<MedicalItem>
    {
        Task<List<MedicalItem>> GetAllMedicalItem();
        Task UpdateMedicalItemAsync(MedicalItem medicalItem);
        Task DeleteMedicalItemAsync(MedicalItem medicalItem);
        Task CreateMedicalItemAsync(MedicalItem medicalItem);
    }
}
