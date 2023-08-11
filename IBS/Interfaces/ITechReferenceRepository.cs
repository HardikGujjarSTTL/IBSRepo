using IBS.Models;

namespace IBS.Interfaces
{
    public interface ITechReferenceRepository
    {
        public TechReferenceModel FindByID(string TechId,string rgn);
        DTResult<TechReferenceModel> GetTechReferenceList(DTParameters dtParameters,string Rgn);        
        bool Remove(string TechId, string rgn,int UserId);        
        string TechRefDetailsInsertUpdate(TechReferenceModel model, string Rgn);
    }
}
