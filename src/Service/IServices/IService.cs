using Repository.Extensions;
using Repository.Models.Service;

namespace Service.IServices
{
    public interface IService
    {
        Task<List<ServiceResponseDto>> GetAllServiceAsync();
        Task<PaginatedList<ServiceResponseDto>> GetAllServiceAsync(int pageNumber, int pageSize);
        Task<ServiceResponseDto> GetServiceBydId(int serviceId);
        Task CreateServiceAsync(ServiceRequestDto service, int createdById);
        Task UpdateServiceAsync(ServiceUpdateDto service, int updatedById);
        Task DeleteServiceAsync(int serviceId, int deleteBy);
    }
}
