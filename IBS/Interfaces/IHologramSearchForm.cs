using IBS.DataAccess;
using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHologramSearchForm
    {
        public HologramSearchFormModel FindByID(int HgNoFr);
        DTResult<HologramSearchFormModel>GetHologramSearchFormList(DTParameters dtParameters);
        bool Remove(string HgNoFr, int UserID);
        int HologramSearchFormDetailsInsertUpdate(HologramSearchFormModel model);
    }
}