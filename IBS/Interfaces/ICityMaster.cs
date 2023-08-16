using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICityMaster
    {
        public CityMasterModel FindByID(int CityCd);
        DTResult<CityMasterModel>GetCityMasterList(DTParameters dtParameters);
        bool Remove(int CityCd, int UserID);
        int CityMasterDetailsInsertUpdate(CityMasterModel model);
    }
}
