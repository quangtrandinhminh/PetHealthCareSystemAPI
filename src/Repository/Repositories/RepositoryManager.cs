using Repository.Interfaces;

namespace Repository.Repositories;

public class RepositoryManager: IRepositoryManager
{
    private readonly Lazy<IConfigurationRepository> _configurationRepo;
    private readonly Lazy<IPetRepository> _petRepo;

    public RepositoryManager()
    {
        _configurationRepo = new Lazy<IConfigurationRepository>(() => new ConfigurationRepository());
        _petRepo = new Lazy<IPetRepository>(() => new PetRepository());
    }

    public IConfigurationRepository Configuration => _configurationRepo.Value;

    public IPetRepository Pet => _petRepo.Value;
}