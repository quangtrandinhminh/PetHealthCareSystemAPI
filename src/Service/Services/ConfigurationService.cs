using BusinessObject.DTO;
using BusinessObject.DTO.Configuration;
using BusinessObject.Entities;
using BusinessObject.Mappers;
using BusinessObject.QueryObject;
using Repository.Interfaces;
using Service.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Service.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IRepositoryManager _repository;

    public ConfigurationService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public BaseResponseDto CreateConfig(ConfigurationCreateRequestDto configDto)
    {
        _repository.Configuration.CreateConfiguration(configDto.ToConfigurationFromCreateRequest());

        return new BaseResponseDto(200, "Ok", "Create successfully");
    }

    public BaseResponseDto GetAllConfig(ConfigurationQuery query)
    {
        var existingConfig = FindConfigByKey(query.ConfigKey);

        if (existingConfig != null)
        {
            return new BaseResponseDto(400, "Fail", "No data", "No additional data", "Duplicate key");
        }

        var configs = _repository.Configuration.GetAllConfig();
        var totalCount = configs.Count();

        if (!string.IsNullOrWhiteSpace(query.ConfigKey))
        {
            configs = configs.Where(e => e.ConfigKey.ToLower().Contains(query.ConfigKey.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("ConfigKey", StringComparison.OrdinalIgnoreCase))
            {
                configs = query.IsDecsending ? configs.OrderByDescending(e => e.ConfigKey) : configs.OrderBy(e => e.ConfigKey);
            }
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                configs = query.IsDecsending ? configs.OrderByDescending(e => e.Value) : configs.OrderBy(e => e.Value);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        var resultData = configs.Skip(skipNumber).Take(query.PageSize).ToList();

        return new BaseResponseDto(200, "Success", resultData, new { totalCount = totalCount, pageSize = query.PageSize, pageNumber = query.PageNumber }, "No message");
    }

    public Configuration FindConfigByKey(string key)
    {
        return _repository.Configuration.GetConfigurationByKey(key);
    }

    public BaseResponseDto GetConfigByKey(string key)
    {
        var config = FindConfigByKey(key);

        return new BaseResponseDto(200, "Success", config, "No additional data", "No message");
    }
}