using IBS.Models;

namespace IBS.Interfaces
{
    public interface IRitesDesignationMasterRepository
    {
        public RDMModel FindByID(int RDesigCd);

        public DTResult<RDMModel> GetRDMList(DTParameters dtParameters);

        public int SaveDetails(RDMModel model);

        public bool Remove(int RdmCd, int UserID);
    }
}
