using Repository.Models.Configuration;

namespace Service.IServices;

public interface IConfigurationService
{
    Task<List<ConfigurationResponseDto>> GetConfigurationsAsync();
    Task<ConfigurationResponseDto> UpdateConfiguration(ConfigurationUpdateRequestDto dto);
}