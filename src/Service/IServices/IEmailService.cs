using BusinessObject.DTO.User;

namespace Service.IServices;

public interface IEmailService
{
    Task SendMailAsync(SendMailDto model);
}