namespace Piligrim.Core.Mail
{
    public class Smtp
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}