using BusinessObject.DTO.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;

namespace PetHealthCareSystemAPI.Controllers;

[ApiController]
[Route("api/pet")]
public class PetController : ControllerBase
{
    private readonly IServiceManager _service;

    public PetController(IServiceManager serviceManager)
    {
        _service = serviceManager;
    }

    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Customer")]
    public IActionResult GetPetByUserId([FromRoute] string userId)
    {
        var response = _service.PetService.GetPetsByUserId(userId);

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

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public IActionResult CreatePetForUser([FromBody] PetCreateRequestDto petDto)
    {
        var response = _service.PetService.CreatePetForCustomer(petDto);

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

    [HttpPut]
    [Authorize(Roles = "Customer")]
    public IActionResult UpdatePetForUser([FromBody] PetUpdateRequestDto petDto)
    {
        var response = _service.PetService.UpdatePet(petDto);

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