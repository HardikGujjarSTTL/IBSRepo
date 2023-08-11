using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHighlightsRepository
    {
        public HighlightsModel FindByID(string HighDt, string RegionCode);
        DTResult<HighlightsModel> GetHighlightsList(DTParameters dtParameters, string RegionCode);
        
        bool Remove(string HighDt, string strRgn);
        
        string HighlightsDetailsInsertUpdate(HighlightsModel model);
    }
}
