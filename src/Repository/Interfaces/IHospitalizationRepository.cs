using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface IHospitalizationRepository : IBaseRepository<Hospitalization>
{
    Task<List<Hospitalization>> GetAllHospitalization();
    Task UpdateHospitalizationAsync(Hospitalization hospitalization);
    Task DeleteHospitalizationAsync(Hospitalization hospitalization);
    Task CreateHospitalizationAsync(Hospitalization hospitalization);
}