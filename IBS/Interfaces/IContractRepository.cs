using IBS.Models;

namespace IBS.Interfaces
{
    public interface IContractRepository
    {
        public ContractModel FindByID(int ContractId);
        DTResult<ContractModel> GetContractList(DTParameters dtParameters);

        bool Remove(int ContractId, int UserID);

        int ContractDetailsInsertUpdate(ContractModel model);
    }
}
