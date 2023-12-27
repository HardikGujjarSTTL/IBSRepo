using IBS.Models;

namespace IBS.Interfaces
{
    public interface IGeneralMessageRepository
    {
        public GeneralMessageModel FindByID(int MessageId);

        DTResult<GeneralMessageModel> GetMessageList(DTParameters dtParameters);

        bool Remove(int MessageId);

        int MessageDetailsInsertUpdate(GeneralMessageModel model);
    }
}
