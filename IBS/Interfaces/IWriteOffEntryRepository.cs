using IBS.Models;

namespace IBS.Interfaces
{
    public interface IWriteOffEntryRepository
    {
        DTResult<WriteOffEntryModel> GetWriteOfEntryList(DTParameters dtParameters);

    }
}
