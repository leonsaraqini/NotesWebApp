using System.Configuration;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace NotesWebApp.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Retrieve SMTP settings from Web.config
            var smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            if (smtpSection == null)
                throw new ConfigurationErrorsException("SMTP settings not found in Web.config");

            var smtpClient = new SmtpClient
            {
                Host = smtpSection.Network.Host,
                Port = smtpSection.Network.Port,
                Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password),
                EnableSsl = smtpSection.Network.EnableSsl
            };

            using (var mailMessage = new MailMessage(smtpSection.From, message.Destination)
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = true
            })
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
