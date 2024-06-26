﻿using BusinessObject.DTO.Service;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IServices;
using Utility.Constants;
using BusinessObject.DTO.MedicalItem;

namespace PetHealthCareSystemAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalItemController : Controller
    {
        private IMedicalItemService _medicalItemRepo;

        public MedicalItemController(IMedicalItemService medicalItem)
        {
            _medicalItemRepo = medicalItem;
        }

        // GET: api/<MedicalItemController>
        [HttpGet]
        [Authorize(Roles = "Staff")]
        [Route("GetAllMedicalItem")]
        public async Task<IActionResult> Get()
        {
            var list = await _medicalItemRepo.GetAllMedicalItem();

            return Ok(BaseResponseDto.OkResponseDto(list, "No additional data"));
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        [Route("Create MedicalItem")]
        public async Task<OkObjectResult> PostAsync([FromBody] MedicalResponseDto dto)
        {
            await _medicalItemRepo.CreateMedicalItem(dto);

            return Ok(BaseResponseDto.OkResponseDto(ResponseMessageConstantsPet.ADD_PET_SUCCESS));
        }

        // PUT api/<ServiceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServiceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
