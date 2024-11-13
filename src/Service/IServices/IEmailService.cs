using Repository.Models.User;

namespace Service.IServices;

public interface IEmailService
{
    void SendMail(SendMailDto model);
}