using IBS.Models;

namespace IBS.Interfaces
{
    public interface IFeedbackSuggestionRepository
    {
        string SaveFeedback(EmailFeedback model);
    }
}
