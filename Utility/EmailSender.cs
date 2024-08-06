using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Utility
{
    public class EmailSender : IEmailSender
    {
        public string SecretKey { get; set; }
        public EmailSender(IConfiguration _config)
        {
            SecretKey = _config.GetValue<string>("SendGrid:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient client = new(SecretKey);
            EmailAddress from = new("bagdensparrow@gmail.com", "AsptDotNetCoreShop");
            EmailAddress to = new(email);
            SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            return client.SendEmailAsync(message);
        }
    }
}
