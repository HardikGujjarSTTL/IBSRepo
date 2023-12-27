using IBS.Models;

namespace IBS.Interfaces
{
    public interface ITechReferenceRepository
    {
        public TechReferenceModel FindByID(int ID);
        DTResult<TechReferenceModel> GetTechReferenceList(DTParameters dtParameters, string Rgn);
        bool Remove(int ID, int UserId);
        int TechRefDetailsInsertUpdate(TechReferenceModel model, string Rgn);
    }
}
