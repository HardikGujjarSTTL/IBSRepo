using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRegionalHRDataOfIERepository
    {
        public RegionalHRDataOfIEModel FindByID(int ID);
        DTResult<RegionalHRDataOfIEModel> GetList(DTParameters dtParameters);
        bool Remove(int ID, int UserID);
        int InsertUpdate(RegionalHRDataOfIEModel model);

        public RegionalHRDataOfIEModel GetIEDetails(int ID);
    }
}
