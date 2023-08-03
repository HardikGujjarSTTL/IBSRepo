using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionEngineers
    {
        public InspectionEngineersModel FindByID(int UomCd);
        DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters);
        bool Remove(int IeCd, int UserID);
        int InspectionEngineersDetailsInsertUpdate(InspectionEngineersModel model);
    }
}