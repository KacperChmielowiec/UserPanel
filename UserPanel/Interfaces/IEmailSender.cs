using UserPanel.Models;

namespace UserPanel.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Email email);
    }
}
