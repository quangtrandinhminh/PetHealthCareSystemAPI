using Repository.Models.User;

namespace Service.IServices
{
    public interface IAuthService
    {
        Task<IList<RoleResponseDto>> GetAllRoles();
        Task Register(RegisterDto dto);
        Task RegisterByAdmin(RegisterDto dto, int role);
        Task<LoginResponseDto> Authenticate(LoginDto dto);
        Task<LoginResponseDto> RefreshToken(string token);
        Task VerifyEmail(VerifyEmailDto dto);
        Task ForgotPassword(ForgotPasswordDto model);
        Task ResetPassword(ResetPasswordDto model);
        Task ChangePassword(ChangePasswordDto dto);
        Task ReSendEmail(ResendEmailDto model);
        Task StaffRegistor(RegisterDto dto);
    }
}