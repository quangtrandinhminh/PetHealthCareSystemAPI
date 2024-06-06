using BusinessObject.Entities;

namespace Repository.Interfaces;

public interface IConfigurationRepository
{
    void CreateConfiguration(Configuration config);
    IQueryable<Configuration> GetAllConfig();
    Configuration GetConfigurationByKey(string key);
}