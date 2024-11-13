using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<List<Service>> GetAllService();
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(Service service);
    Task CreateServiceAsync(Service service);
}