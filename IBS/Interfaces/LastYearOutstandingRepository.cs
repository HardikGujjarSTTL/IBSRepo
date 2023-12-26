using IBS.Models;

namespace IBS.Interfaces
{
    public interface ILastYearOutstandingRepository
    {
        public LastYearOutstandingModel FindByID(string LyPer, string RegionCode);
        DTResult<LastYearOutstandingModel> GetLastYearOutstandingList(DTParameters dtParameters, string RegionCode);

        bool Remove(string LyPer, string strRgn);

        string LastYearOutstandingDetailsInsertUpdate(LastYearOutstandingModel model);
    }
}
