using BusinessObject.DTO.Configuration;
using BusinessObject.Entities;

namespace BusinessObject.Mappers;

public static class ConfigurationMapper
{
    public static Configuration ToConfigurationFromCreateRequest(this ConfigurationCreateRequestDto configDto)
    {
        return new Configuration()
        {
            ConfigKey = configDto.ConfigKey,
            Value = configDto.Value,
        };
    }
}