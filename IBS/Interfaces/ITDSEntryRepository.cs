using IBS.Models;

namespace IBS.Interfaces
{
    public interface ITDSEntryRepository
    {
        public TDSEntryModel GetBillDetails(string BillNo, string Region);

        public bool SaveDetails(TDSEntryModel model);

        public DTResult<TDSEntryModel> GetTDSHistroyList(DTParameters dtParameters);

    }
}
