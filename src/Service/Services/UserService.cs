using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;

namespace Service.Services;

public class UserService(IServiceProvider serviceProvider) : IUserService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();

    public Task<VetResponseDto> CreateVet(VetRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<UserResponseDto>> GetVets()
    {
        var vets = await _userManager.GetUsersInRoleAsync(UserRole.Vet.ToString());
        if (vets == null || vets.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsVet.VET_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(vets);
        // get role of each vet
        foreach (var vet in response)
        {

            vet.Role = UserRole.Vet.ToString();
        }
        return response;
    }
}