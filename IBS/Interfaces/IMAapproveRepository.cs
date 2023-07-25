using IBS.Models;

namespace IBS.Interfaces
{
    public interface IMAapproveRepository
    {
        public MAapproveModel FindByID(string CaseNo, string MaNo, string MaDtc, byte MaSno);

        DTResult<MAapproveModel> GetDataList(DTParameters dtParameters, string GetRegionCode);

        int DetailsUpdate(MAapproveModel model);
    }
}
