using IBS.Models;

namespace IBS.Interfaces.Inspection_Billing
{
    public interface IICCancellationRepository
    {
        public ICCancellationModel FindByID(string REGION,string BK_NO, string SET_NO);
        DTResult<ICCancellationListModel> GetCancellationList(DTParameters dtParameters,string Region);
        bool Remove(string REGION, string BK_NO, string SET_NO, int UserID);
        int ICCancellationSave(ICCancellationModel model);

    }
}
