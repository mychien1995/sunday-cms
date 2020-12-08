namespace Sunday.Core.Domain.Email
{
    public class ApplicationSmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public ApplicationSmtpSettings()
        {

        }
        public ApplicationSmtpSettings(string host, int port, string userName, string pwd, bool useSsl = true)
        {
            this.Host = host;
            this.Port = port;
            this.Username = userName;
            this.Password = pwd;
            this.EnableSsl = useSsl;
        }
    }
}
