using System.Threading.Tasks;

namespace Piligrim.Core.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
