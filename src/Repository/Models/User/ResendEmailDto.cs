using System.ComponentModel.DataAnnotations;

namespace Repository.Models.User;

public class ResendEmailDto
{
    [Required]
    public string UserName { get; set; }
}