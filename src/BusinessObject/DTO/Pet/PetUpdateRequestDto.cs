using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Pet;

public class PetUpdateRequestDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string? Species { get; set; }

    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string? Breed { get; set; }

    [Required]
    [Range(0, 30, ErrorMessage = "Your pet old is incorrect")]
    public int? Age { get; set; }
}