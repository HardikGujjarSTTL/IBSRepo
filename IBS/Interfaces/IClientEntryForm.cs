using IBS.Models;

namespace IBS.Interfaces
{
    public interface IClientEntryForm
    {
        public ClientEntryFormModel FindByID(int Mobile);
        DTResult<ClientEntryFormModel>GetClientEntryFormList(DTParameters dtParameters);
        bool Remove(int Mobile, int UserID);
        int ClientEntryFormDetailsInsertUpdate(ClientEntryFormModel model);
    }
}
