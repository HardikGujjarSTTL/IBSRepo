using IBS.Models;

namespace IBS.Interfaces
{
    public interface IContractEntryRepository
    {
        int ContractDetailsInsertUpdate(ContractEntry model);
        DTResult<ContractEntry> GetContractList(DTParameters dtParameters);
        public ContractEntry FindByID(int ID);
    }
}
