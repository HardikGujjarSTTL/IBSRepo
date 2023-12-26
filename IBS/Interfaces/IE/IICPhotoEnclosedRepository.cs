using IBS.Models;

namespace IBS.Interfaces.IE
{
    public interface IICPhotoEnclosedRepository
    {
        DTResult<ICPhotoEnclosedModel> GetDataList(DTParameters dtParameters, string GetRegionCode, int GetIeCd);
    }
}
