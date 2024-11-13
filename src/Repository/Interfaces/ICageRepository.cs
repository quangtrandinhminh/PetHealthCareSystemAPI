using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface ICageRepository : IBaseRepository<Cage>
{
    Task<List<Cage>> GetAllCage();
    Task UpdateCageAsync(Cage cage);
    Task DeleteCageAsync(Cage cage);
    Task CreateCageAsync(Cage cage);
}