using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;

namespace Service.IServices;

public interface IUserService
{
    Task<VetResponseDto> CreateVet(VetRequestDto dto);
    Task<IList<UserResponseDto>> GetVets();
}