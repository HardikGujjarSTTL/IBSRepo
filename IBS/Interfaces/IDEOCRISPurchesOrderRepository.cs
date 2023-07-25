using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDEOCRISPurchesOrderRepository
    {
        public DEOCRISPurchesOrderMAModel FindByID(string Rly, int Makey, byte Slno);

        DTResult<DEOCRISPurchesOrderMAModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        int DetailsUpdate(DEOCRISPurchesOrderMAModel model);
    }
}
