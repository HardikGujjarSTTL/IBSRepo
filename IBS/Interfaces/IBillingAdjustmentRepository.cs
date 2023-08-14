using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillingAdjustmentRepository
    {
        public BillingAdjustmentModel FindByID(string AdjstmntTyMonth, string RegionCode);
        DTResult<BillingAdjustmentModel> GetBillingAdjustmentList(DTParameters dtParameters, string RegionCode);
        
        bool Remove(string BePer, string strRgn);
        
        string BillingAdjustmentDetailsInsertUpdate(BillingAdjustmentModel model);
    }
}
