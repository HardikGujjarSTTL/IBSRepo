using IBS.Models;

namespace IBS.Interfaces
{
    public interface IProjectDetailsRepository
    {
        DTResult<ProjectModel> GetMasterList(DTParameters dtParameters);
        ProjectModel FindByID(int id);
        public DTResult<ProjectDetailsModel> GetProjectDetailsList(DTParameters dtParameters, List<ProjectDetailsModel> ProductDetailsModels);
        public int SaveProject(ProjectModel model);
        int DeleteProject(int ID, int UserID);
        int SaveDetails(ProjectDetailsModel model);
        int DeleteProjectDetails(int DetailID, int ProjectID);
    }
}
