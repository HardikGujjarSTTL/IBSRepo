using IBS.DataAccess;
using IBS.Models;
using System.Data;

namespace IBS.Interfaces
{
    public interface IETrainingDetailsRepository
    {

        DTResult<IETrainingDetailsModel> GetBills(DTParameters dtParameters, string Regin);
        IETrainingDetailsModel IEFetchData(string Name);
        IETrainingDetailsModel TrainingDFetchData(string Course);
        bool Save(IETrainingDetailsModel IETrainingDetailsModel);

    }
}
