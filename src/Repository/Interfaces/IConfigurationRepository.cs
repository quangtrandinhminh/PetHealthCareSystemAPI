using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface IConfigurationRepository : IBaseRepository<Configuration>
{
    Task<Configuration?> GetValueByKey(string key);
}