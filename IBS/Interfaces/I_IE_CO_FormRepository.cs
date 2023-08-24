using IBS.Models;

namespace IBS.Interfaces
{
    public interface I_IE_CO_FormRepository
    {
        public IE_CO_FormModel FindByID(int CoCd);

        DTResult<IE_CO_FormModel> GetCOList(DTParameters dtParameters);

        public int SaveDetails(IE_CO_FormModel model);

        public bool Remove(int CoCd, int UserID);
    }
}
