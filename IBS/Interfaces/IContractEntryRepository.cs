using IBS.Models;

namespace IBS.Interfaces
{
    public interface IContractEntryRepository
    {
        int ContractDetailsInsertUpdate(ContractEntry model);
        DTResult<ContractEntry> GetContractList(DTParameters dtParameters);
        public ContractEntry FindByID(int ID);
        bool Remove(int ID, int UserID);

        public DTResult<ContractEntryList> GetValueList(DTParameters dtParameters, List<ContractEntryList> ContractList);
    }
}
