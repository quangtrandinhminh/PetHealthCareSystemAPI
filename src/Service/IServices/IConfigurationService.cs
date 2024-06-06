using BusinessObject.DTO;
using BusinessObject.DTO.Configuration;
using BusinessObject.Entities;
using BusinessObject.QueryObject;

namespace Service.IServices;

public interface IConfigurationService
{
    BaseResponseDto CreateConfig(ConfigurationCreateRequestDto configDto);
    BaseResponseDto GetAllConfig(ConfigurationQuery query);
    Configuration FindConfigByKey(string key);
    BaseResponseDto GetConfigByKey(string key);
}