using Sunday.Core.Application.Common;
using Sunday.Core.Domain.Email;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Sunday.Core.Implementation.Common
{
    [ServiceTypeOf(typeof(IMailService))]
    public class MailService : IMailService
    {
        public virtual async Task SendEmail(string subject, string template, List<string> recipients, params string[] datas)
        {
            var from = ApplicationSettings.Get("Sunday.Email.From");
            var smtpSetting = GetSmtpSettings();
            var body = BuildBody(template, datas);
            var message = new MailMessage();
            message.From = new MailAddress(from);
            foreach (var recipient in recipients)
            {
                message.To.Add(recipient);
            }
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            using var client = new SmtpClient
            {
                Host = smtpSetting.Host,
                Port = smtpSetting.Port,
                EnableSsl = smtpSetting.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpSetting.Username, smtpSetting.Password)
            };
            await client.SendMailAsync(message);
        }

        public virtual string BuildBody(string template, string[] data)
        {
            return string.Format(template, data);
        }

        public virtual ApplicationSmtpSettings GetSmtpSettings()
        {
            var host = ApplicationSettings.Get("Sunday.Email.Host");
            var port = ApplicationSettings.Get<int>("Sunday.Email.Port");
            var username = ApplicationSettings.Get("Sunday.Email.Username");
            var password = ApplicationSettings.Get("Sunday.Email.Password");
            var useSsl = ApplicationSettings.Get<bool>("Sunday.Email.EnableSsl");
            return new ApplicationSmtpSettings(host, port, username, password, useSsl);
        }
    }
}
