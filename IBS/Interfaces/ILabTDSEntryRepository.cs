using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILabTDSEntryRepository
    {

        public LabTDSEntryModel SearchRegNo(string RegNo, string Region);
        public bool SaveTDSEntry(string RegNo, string TDSAmt, string TDSDate);

    }
}
