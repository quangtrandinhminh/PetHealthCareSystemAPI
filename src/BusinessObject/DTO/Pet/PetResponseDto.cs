namespace BusinessObject.DTO.Pet;

public class PetResponseDto
{
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public bool IsNeutered { get; set; }
}