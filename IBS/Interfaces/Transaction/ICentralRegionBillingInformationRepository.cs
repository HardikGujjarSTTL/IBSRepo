using IBS.Models;

namespace IBS.Interfaces.Transaction
{
    public interface ICentralRegionBillingInformationRepository
    {
        public CentralRegionBillingInformationModel FindByID(string BILL_NO);
        DTResult<CentralRegionBillingInformationListModel> GetBillingInformationList(DTParameters dtParameters, string Region);
        bool Remove(string BILL_NO, int UserId);
        bool AlreadyExist(string BILL_NO);
        string BillingInformationInsertUpdate(CentralRegionBillingInformationModel model);

    }
}
