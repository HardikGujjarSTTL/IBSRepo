using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabBillingRepository
    {
        public LabBillingModel FindByID(string LabBillPer, string RegionCode);
        DTResult<LabBillingModel> GetLabBillingList(DTParameters dtParameters, string RegionCode);
        
        bool Remove(string LabBillPer, string strRgn);
        
        string LabBillingDetailsInsertUpdate(LabBillingModel model);
    }
}
