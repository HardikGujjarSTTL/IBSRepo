using IBS.Models;

namespace IBS.Interfaces
{
    public interface IDEOCRISPurchesOrderWCaseNoRepository
    {
        public DEOCRISPurchesOrderMAModel FindByID(string Rly, int Makey, byte Slno);

        DTResult<DEOCRISPurchesOrderMAModel> GetDataList(DTParameters dtParameters, string Region);

        int DetailsUpdate(DEOCRISPurchesOrderMAModel model);
    }
}
