using BusinessObject.DTO.Service;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Repositories;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services
{
    public class ServiceService(IServiceProvider serviceProvider) : IService
    {
        private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
        private readonly IServiceRepository _serviceRepo = serviceProvider.GetRequiredService<IServiceRepository>();

        public async Task CreateServiceAsync(ServiceResponseDto service)
        {
            await _serviceRepo.CreateServiceAsync(_mapper.Map(service));
        }

        public async Task DeleteServiceAsync(int id, int deleteBy)
        {
            var findService = _serviceRepo.GetAllWithCondition(x =>x.Id == id).FirstOrDefault();

            findService.DeletedBy = deleteBy;
            findService.DeletedTime = DateTime.Now;

            await _serviceRepo.UpdateAsync(findService);
        }

        public async Task<List<ServiceResponseDto>> GetAllServiceAsync()
        {
            var list = await _serviceRepo.GetAllService();

            var listDto = _mapper.Map(list);

            return listDto.ToList();
        }

        public async Task<ServiceResponseDto> GetBydId(int id)
        {
            var list = _serviceRepo.GetById(id);

            var listDto = _mapper.Map(list);

            return listDto;
        }

        public async Task UpdateServiceAsync(ServiceResponseDto service)
        {
            await _serviceRepo.UpdateServiceAsync(_mapper.Map(service));
        }
    }
}
