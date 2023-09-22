using IBS.Models;

namespace IBS.Interfaces
{
    public interface INonRlyClientMasterRepository
    {
        int ClientDetailsInsertUpdate(Clientmaster model);
        DTResult<Clientmaster> GetNonClientList(DTParameters dtParameters);
        public Clientmaster FindNonClientByID(int ID);
        bool Remove(int ID, int UserID);
    }
}
