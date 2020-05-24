using Sunday.Core.Application.Common;
using Sunday.Core.Domain.Email;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sunday.Core.Implementation.Common
{
    [ServiceTypeOf(typeof(IMailService))]
    public class MailService : IMailService
    {
        public async virtual Task SendEmail(string subject, string template, List<string> recipients, params string[] datas)
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
            using (var client = new SmtpClient())
            {
                client.Host = smtpSetting.Host;
                client.Port = smtpSetting.Port;
                client.EnableSsl = smtpSetting.EnableSsl;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpSetting.Username, smtpSetting.Password);
                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {

                }
            }
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
