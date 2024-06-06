namespace Service.IServices;

public interface IServiceManager
{
    public IConfigurationService ConfigurationService { get; }
    public IPetService PetService { get; }
}