using IBS.Models;

namespace IBS.Interfaces
{
    public interface I_ICBooksetFormRepository
    {
        public IC_Bookset_FormModel FindByID(int BkNo);

        DTResult<IC_Bookset_FormModel> GetBooksetList(DTParameters dtParameters);

        public string IsExists(IC_Bookset_FormModel model);

        public string SaveDetails(IC_Bookset_FormModel model);

        public bool Remove(int Id);

    }
}
