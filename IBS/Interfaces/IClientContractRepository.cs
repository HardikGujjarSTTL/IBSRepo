using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientContractRepository
    {
        public ClientContractModel FindByID(int ContractId);
        DTResult<ClientContractModel> GetClientContractList(DTParameters dtParameters);
        
        bool Remove(int ContractId, int UserID);
        
        int ClientContractDetailsInsertUpdate(ClientContractModel model);
    }
}
