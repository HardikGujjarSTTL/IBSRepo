using IBS.Models;

namespace IBS.Interfaces
{
    public interface ICityRepository
    {
        public CityMasterModel FindByID(int CityCd);

        DTResult<CityMasterModel>GetCityMasterList(DTParameters dtParameters);

        public int SaveDetails(CityMasterModel model);

        public int GetMaxCityCd();

        public bool Remove(int CityCd);
    }
}
