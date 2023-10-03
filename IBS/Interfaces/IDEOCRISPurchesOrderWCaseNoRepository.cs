using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDEOCRISPurchesOrderWCaseNoRepository
    {
        public DEO_CRIS_PurchesOrderModel FindByID(string ImmsPokey, string ImmsRlyCd);

        DTResult<DEO_CRIS_PurchesOrderListModel> GetDataList(DTParameters dtParameters, string Region);

        int DetailsUpdate(DEOCRISPurchesOrderMAModel model);
    }
}
