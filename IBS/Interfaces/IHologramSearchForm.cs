using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHologramSearchForm
    {
        HologramSearchFormModel FindByID(string HgNoFr, string HgNoTo, string Region);
        DTResult<HologramSearchFormModel> GetHologramSearchFormList(DTParameters dtParameters, string region);
        bool Remove(HologramSearchFormModel model);
        int SaveDetails(HologramSearchFormModel model);
        int CheckDate(string IEDate);
        int CheckHologramNo(HologramSearchFormModel model);
        string IEIssueOrNot(string IE);
        int CheckHologramCancel(HologramSearchFormModel model);
        int MatchHologram(string HgNoFr, string HgNoTo, string Region);

    }
}