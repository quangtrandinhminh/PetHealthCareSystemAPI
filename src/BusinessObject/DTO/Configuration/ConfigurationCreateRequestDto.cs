using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Configuration;

public class ConfigurationCreateRequestDto
{
    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string? ConfigKey { get; set; }
    [Required]
    public string? Value { get; set; }
}