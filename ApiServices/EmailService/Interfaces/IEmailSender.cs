using System.Threading.Tasks;

namespace ApiServices.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
