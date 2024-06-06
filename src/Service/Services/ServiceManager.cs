using Repository.Interfaces;
using Service.IServices;

namespace Service.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IConfigurationService> _configurationService;
    private readonly Lazy<IPetService> _petService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _configurationService = new Lazy<IConfigurationService>(() => new ConfigurationService(repositoryManager));
        _petService = new Lazy<IPetService>(() => new PetService(repositoryManager));
    }

    public IConfigurationService ConfigurationService => _configurationService.Value;

    public IPetService PetService => _petService.Value;
}