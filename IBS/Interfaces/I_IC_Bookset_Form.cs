using IBS.Models;

namespace IBS.Interfaces
{
    public interface I_IC_Bookset_Form
    {
        public IC_Bookset_FormModel FindByID(int BkNo);
        DTResult<IC_Bookset_FormModel> GetBooksetList(DTParameters dtParameters);
        bool Remove(int BkNo, int UserID);
        int BooksetDetailsInsertUpdate(IC_Bookset_FormModel model);
    }
}
