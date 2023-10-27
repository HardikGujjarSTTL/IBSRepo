using IBSAPI.Models;

namespace IBSAPI.Interfaces
{
    public interface ISendMailRepository
    {
        bool SendMail(SendMailModel model, IFormFileCollection? Files);
    }
}
