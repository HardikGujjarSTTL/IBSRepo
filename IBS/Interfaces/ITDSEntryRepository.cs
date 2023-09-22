using IBS.Models;

namespace IBS.Interfaces
{
    public interface ITDSEntryRepository
    {
        public TDSEntryModel GetTextboxValues(string txtBNO, string Region);
        public string TDSdetailSave(TDSEntryModel model);
    }
}
