using IBS.Models;

namespace IBS.Interfaces
{
    public interface I_IE_CO_Form
    {
        public IE_CO_FormModel FindByID(int CoCd);
        DTResult<IE_CO_FormModel> GetCOList(DTParameters dtParameters);
        bool Remove(int CoCd, int UserID);
        int CODetailsInsertUpdate(IE_CO_FormModel model);

    }
}
