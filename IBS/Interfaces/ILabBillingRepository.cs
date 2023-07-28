using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabBillingRepository
    {
        public LabBillingModel FindByID(int Id);
        DTResult<LabBillingModel> GetLabBillingList(DTParameters dtParameters);
        
        bool Remove(int Id, int UserID);
        
        int LabBillingDetailsInsertUpdate(LabBillingModel model);
    }
}
