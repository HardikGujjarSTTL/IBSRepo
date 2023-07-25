using IBS.Models;

namespace IBS.Interfaces.Administration
{
    public interface IUploadDocRepository
    {
        string DetailsUpdate(UploadDocModel model);
    }
}
