using IBS.Models;

namespace IBS.Interfaces.InspectionBilling
{
    public interface IInspectionCertRepository
    {
        DTResult<InspectionCertModel> GetDataList(DTParameters dtParameters, string GetRegionCode);
    }
}
