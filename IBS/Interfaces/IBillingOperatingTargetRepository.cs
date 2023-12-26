using IBS.Models;

namespace IBS.Interfaces
{
    public interface IBillingOperatingTargetRepository
    {
        public BillingOperatingTargetModel FindByID(string BePer, string RegionCode);
        DTResult<BillingOperatingTargetModel> GetBillingOperatingTargetList(DTParameters dtParameters, string RegionCode);

        bool Remove(string BePer, string strRgn);

        string BillingOperatingDetailsInsertUpdate(BillingOperatingTargetModel model);
    }
}
