using IBS.Models;

namespace IBS.Interfaces
{
    public interface IHolidayMasterRepository
    {
        DTResult<HolidayMasterModel> GetHolidayMasterList(DTParameters dtParameters);
        HolidayMasterModel FindByID(int id);
        int HolidayMasterSave(HolidayMasterModel model);

        DTResult<HolidayDetailModel> GetHolidayDetailList(DTParameters dtParameters);
    }
}
