using IBS.Models;

namespace IBS.Interfaces
{
    public interface IWriteOffEntryRepository
    {
        DTResult<WriteOffEntryModel> GetWriteOfEntryList(DTParameters dtParameters,string Region);
        int UpdateWriteAmtDetails(List<UpdateDataModel> dataArr, WriteOfMaster model);

    }
}
