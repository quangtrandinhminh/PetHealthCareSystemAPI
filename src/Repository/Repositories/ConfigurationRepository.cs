using BusinessObject.Entities;
using DataAccessLayer.DAO;
using Repository.Interfaces;

namespace Repository.Repositories;

public class ConfigurationRepository : IConfigurationRepository
{
    public void CreateConfiguration(Configuration config)
    {
        ConfigurationDao.Add(config);
    }

    public IQueryable<Configuration> GetAllConfig()
    {
        return ConfigurationDao.GetAll().Where(e => e.IsActive == false || e.IsActive == null).AsQueryable();
    }

    public Configuration GetConfigurationByKey(string key)
    {
        return ConfigurationDao.FindByCondition(e => e.ConfigKey == key).FirstOrDefault();
    }
}