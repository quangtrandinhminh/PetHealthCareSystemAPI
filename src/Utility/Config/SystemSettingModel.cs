using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Utility.Config
{
    public class SystemSettingModel
    {
        public static SystemSettingModel Instance { get; set; }

        public static IConfiguration Configs { get; set; }
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public string? Domain { get; set; }
        public string SecretKey { get; set; }
        public string SecretCode { get; set; }
    }

    public class MailSettingModel
    {
        public static MailSettingModel Instance { get; set; }
        public SmtpSetting Smtp { get; set; }
        public string FromAddress { get; set; }
        public string FromDisplayName { get; set; }
    }

    public class SmtpSetting
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public bool UsingCredential { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}