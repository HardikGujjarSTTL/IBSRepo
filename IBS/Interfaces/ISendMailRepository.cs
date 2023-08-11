using IBS.Models;

namespace IBS.Interfaces
{
    public interface ISendMailRepository
    {
        bool SendMail(SendMailModel model, IFormFileCollection? Files);
    }
}
