using System.ComponentModel.DataAnnotations;

namespace Repository.Models.User;

public class ForgotPasswordDto
{
    [Required]
    public string UserName { get; set; }
}