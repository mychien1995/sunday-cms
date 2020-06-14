namespace Sunday.Core.Domain.Email
{
    public class ApplicationSmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public ApplicationSmtpSettings()
        {

        }
        public ApplicationSmtpSettings(string host, int port, string uname, string pwd, bool useSsl = true)
        {
            this.Host = host;
            this.Port = port;
            this.Username = uname;
            this.Password = pwd;
            this.EnableSsl = useSsl;
        }
    }
}
