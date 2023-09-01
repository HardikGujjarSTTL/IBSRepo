using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientMasterRepository
    {
        DTResult<Clientmaster> GetClientList(DTParameters dtParameters);

        int ClientDetailsInsertUpdate(Clientmaster model);
        public Clientmaster FindClientByID(int ID);
        bool Remove(int ID, int UserID);
    }
}
