namespace Repository.Interfaces;

public interface IRepositoryManager
{
    IConfigurationRepository Configuration { get; }
    IPetRepository Pet { get; }
}