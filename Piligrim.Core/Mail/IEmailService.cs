using System.Threading.Tasks;
using Piligrim.Core.Models;

namespace Piligrim.Core.Mail
{
    public interface IEmailService
    {
        Task Send(Order order, string shopEmail, string shopPhoneNumber, string templatePath);

        Task Send(string email, string subject, string body);
    }
}
