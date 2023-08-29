using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionEngineers
    {
        public InspectionEngineersModel FindByID(int IeCd);

        public InspectionEngineersModel FindManageByID(int IeCd,string ActionType,string GetRegionCode);

        DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters);

        bool Remove(int IeCd, int UserID);

        string DetailsInsertUpdate(InspectionEngineersModel model);

        string GetMatch(int IeCd,string GetRegionCode);
    }
}