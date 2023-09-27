using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICalls_Marked_ReportRepository
    {
        public Calls_Marked_ReportModel Query1(string pDtFr, string pDtTo, string pRegion, string pSortKey, int UserID, string wRgn_Name);
    }
}
