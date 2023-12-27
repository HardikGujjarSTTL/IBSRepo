using IBS.Models;

namespace IBS.Interfaces
{
    public interface IInspectionEngineers
    {
        public InspectionEngineersModel FindByID(int Id);

        public InspectionEngineersModel FindManageByID(int Id);

        DTResult<InspectionEngineersModel> GetInspectionEngineersList(DTParameters dtParameters);

        bool Remove(int IeCd, int UserID);

        string DetailsInsertUpdate(InspectionEngineersModel model);

        string GetMatch(int IeCd, string Region);

        string DeleteIe(int IeCd);

        public DTResult<InspectionEngineersListModel> GetClusterValueList(DTParameters dtParameters, List<InspectionEngineersListModel> ClusterModels);

        string GetUserID(string IeEmpNo);
    }
}