using IBS.Models;

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
