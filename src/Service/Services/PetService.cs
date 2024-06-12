using BusinessObject.DTO.Pet;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services;

public class PetService(IServiceProvider serviceProvider) : IPetService
{
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly ILogger _logger = Log.Logger;
    private readonly IPetRepository _petRepo = serviceProvider.GetRequiredService<IPetRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

    public async Task<List<PetResponseDto>> GetAllPetsForCustomerAsync(int id)
    {
        var list = await _petRepo.GetAllPetsByCustomerIdAsync(id);

        var listDto = _mapper.Map(list);

        return listDto.ToList();
    }

    public async Task CreatePetAsync(PetRequestDto pet)
    {
        var user = await _userManager.FindByIdAsync(pet.OwnerID.ToString());

        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.OWNER_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        await _petRepo.CreatePetAsync(_mapper.Map(pet));
    }

    public async Task UpdatePetAsync(PetRequestDto pet)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePetAsync(int id, int deleteBy)
    {
        throw new NotImplementedException();
    }
}