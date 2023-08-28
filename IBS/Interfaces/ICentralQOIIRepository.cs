using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICentralQOIIRepository
    {
        public CentralQOIIModel FindByID(string Client, string QtyDate, string Weight, string QoiLength);
        DTResult<CentralQOIIModel> GetCentralQOIIList(DTParameters dtParameters);
        bool Remove(string Client, string QtyDate, int UserID, string Weight, string QoiLength);
        string InsertUpdateCentralQOII(CentralQOIIModel model);
    }
}
