using IBS.Models;

namespace IBS.Interfaces
{
    public interface IProjectDetailsRepository
    {
        public DTResult<ProjectDetails> GetProductDetailsList(DTParameters dtParameters, List<ProjectDetails> ProductDetailsModels);

        public int SaveProductDetailsList(ProjectDetails model, List<ProjectDetails> ProductDetailsModels);
    }
}
