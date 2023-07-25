using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRitesDesignationMaster
    {
        public RDMModel FindByID(int RDesigCd);
        DTResult<RDMModel> GetRDMList(DTParameters dtParameters);
        bool Remove(int RdmCd, int UserID);
        int RDMDetailsInsertUpdate(RDMModel model);
    }
}
