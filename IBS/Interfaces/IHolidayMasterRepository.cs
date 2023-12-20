using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHolidayMasterRepository
    {
        #region Holiday Master
        DTResult<HolidayMasterModel> GetHolidayMasterList(DTParameters dtParameters);
        HolidayMasterModel FindByID(int id);
        int HolidayMasterSave(HolidayMasterModel model);
        int HolidayMasterDelete(int id, HolidayMasterModel model);
        #endregion

        #region Holiday Detail
        DTResult<HolidayDetailModel> GetHolidayDetailList(DTParameters dtParameters);
        HolidayDetailModel Detail_FindByID(int id);
        int HolidayDetailSave(HolidayDetailModel model);
        int HolidayDetailDelete(int id, HolidayDetailModel model);
        #endregion
    }
}
