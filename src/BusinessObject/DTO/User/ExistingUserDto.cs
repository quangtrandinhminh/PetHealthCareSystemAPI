namespace BusinessObject.DTO.User;

public class ExistingUserDto
{
    public string? UserId { get; set; } = string.Empty;
    public string Username { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
    public string? Role { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }
}