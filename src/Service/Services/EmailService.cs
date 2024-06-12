using System.Net;
using System.Net.Mail;
using BusinessObject.DTO.User;
using Service.IServices;
using Utility.Config;

namespace Service.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {

        }

        public async Task SendMailAsync(SendMailDto model)
        {
            try
            {
                // Read HTML content from file
                /*string htmlContent = File.ReadAllText("path/to/emailTemplate.html");*/

                // create mail message
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = "",
                    Body = model.Content,
                    IsBodyHtml = false,

                    // true to send email in html format
                    /*Subject = "Your Subject",
                    Body = htmlContent,
                    IsBodyHtml = true,*/
                };

                // set up mail address from ... to....
                mailMessage.From = new MailAddress
                (
                    MailSettingModel.Instance.FromAddress,
                    MailSettingModel.Instance.FromDisplayName
                );
                mailMessage.To.Add(model.ReceiveAddress);

                // set up smtp client
                var smtp = new SmtpClient()
                {
                    // we need 3 things like below to connect to smtp server
                    EnableSsl = MailSettingModel.Instance.Smtp.EnableSsl,
                    Host = MailSettingModel.Instance.Smtp.Host,
                    Port = MailSettingModel.Instance.Smtp.Port,

                };
                var network = new NetworkCredential
                (
                    MailSettingModel.Instance.Smtp.Username,
                    MailSettingModel.Instance.Smtp.Password
                );
                smtp.Credentials = network;

                await smtp.SendMailAsync(mailMessage);

                smtp.SendCompleted += (s, e) =>
                {
                    // when send mail completed, we need to dispose mail message and smtp client
                    mailMessage.Dispose();
                    smtp.Dispose();
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }


}
