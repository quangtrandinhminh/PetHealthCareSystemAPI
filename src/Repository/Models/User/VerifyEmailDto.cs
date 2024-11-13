namespace Repository.Models.User;

public class VerifyEmailDto
{
    public string Token { get; set; }
    public string UserName { get; set; }
}