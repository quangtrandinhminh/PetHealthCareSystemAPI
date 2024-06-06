using BusinessObject.DTO.User;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.IServices;
using Utility.Enum;

namespace Service.Services;

public class AuthenticationService : IAuthenticateService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signinManager;

    public AuthenticationService(UserManager<User> userManager, SignInManager<User> signinManager)
    {
        _userManager = userManager;
        _signinManager = signinManager;
    }

    public async Task<NewUserDto> CreateCustomerAsync(User user, string password)
    {
        IdentityResult? result = null;

        result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            result = await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
        }

        return result;
    }

    public async Task<User> GetUserByUsernameAndPassword(string username, string password)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

        if (user == null)
        {
            return null;
        }

        var result = await _signinManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded) return null;

        return user;
    }
}