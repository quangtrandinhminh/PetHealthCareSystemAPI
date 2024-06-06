using BusinessObject.DTO.Pet;
using BusinessObject.Entities;

namespace BusinessObject.Mappers;

public static class PetMapper
{
    public static Pet ToPetFromCreateRequestDto(this PetCreateRequestDto petDto)
    {
        return new Pet()
        {
            Name = petDto.Name,
            Species = petDto.Species,
            Breed = petDto.Breed,
            Age = petDto.Age,
            OwnerId = petDto.OwnerId,
        };
    }

    public static PetDto ToPetDto(this Pet pet)
    {
        return new PetDto()
        {
            Species = pet.Species,
            Breed = pet.Breed,
            Age = pet.Age,
            Name = pet.Name,
            Id = pet.Id
        };
    }

    public static Pet ToPetFromUpdateRequestDto(this PetUpdateRequestDto petDto, Pet existingPet)
    {
        existingPet.Name = petDto.Name;
        existingPet.Species = petDto.Species;
        existingPet.Breed = petDto.Breed;
        existingPet.Age = petDto.Age;

        return existingPet;
    }
}