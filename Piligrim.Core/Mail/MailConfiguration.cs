namespace Piligrim.Core.Mail
{
    public class MailConfiguration
    {
        public MailAddress From { get; set; }

        public Smtp Smtp { get; set; }
    }
}