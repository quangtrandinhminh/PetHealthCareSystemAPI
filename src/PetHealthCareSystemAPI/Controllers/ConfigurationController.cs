using BusinessObject.DTO;
using BusinessObject.DTO.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Service.Services;
using Utility.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetHealthCareSystemAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _configurationService.GetConfigurationsAsync();

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] ConfigurationUpdateRequestDto dto)
        {
            var response = await _configurationService.UpdateConfiguration(dto);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsCommon.SUCCESS, response));
        }
    }
}
