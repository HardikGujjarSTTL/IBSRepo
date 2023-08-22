using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICentralQOIRepository
    {
        public CentralQOIModel FindByID(string Client, string QtyDate);
        DTResult<CentralQOIModel> GetCentralQOIList(DTParameters dtParameters);
        bool Remove(string Client, string QtyDate, int UserID);
        string InsertUpdateCentralQOI(CentralQOIModel model);
    }
}
