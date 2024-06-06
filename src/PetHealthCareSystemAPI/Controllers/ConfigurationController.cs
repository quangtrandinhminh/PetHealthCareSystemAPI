using BusinessObject.DTO.Configuration;
using BusinessObject.QueryObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace PetHealthCareSystemAPI.Controllers
{
    [ApiController]
    [Route("api/configuration")]
    [Authorize(Roles = "Admin")]
    public class ConfigurationController : Controller
    {
        private readonly IServiceManager _service;

        public ConfigurationController(IServiceManager service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CreateConfig([FromBody] ConfigurationCreateRequestDto configDto)
        {
            var response = _service.ConfigurationService.CreateConfig(configDto);

            switch (response.StatusCode)
            {
                case >= 100 and <= 299:
                    return Ok(response);
                case >= 400 and <= 599:
                    return BadRequest(response);
                default:
                    return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult GetAllConfig([FromQuery] ConfigurationQuery query)
        {
            var response = _service.ConfigurationService.GetAllConfig(query);

            switch (response.StatusCode)
            {
                case >= 100 and <= 299:
                    return Ok(response);
                case >= 400 and <= 599:
                    return BadRequest(response);
                default:
                    return Ok(response);
            }
        }
    }
}
