using Repository.Base;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
{
    public async Task<Configuration?> GetValueByKey(string key)
    {
        var config = (await FindByConditionAsync(e => e.ConfigKey == key)).FirstOrDefault();
        return config;
    }
}