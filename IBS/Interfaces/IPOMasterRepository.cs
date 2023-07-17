using IBS.Models;

namespace IBS.Interfaces
{
    public interface IPOMasterRepository
    {
        public PO_MasterModel FindByID(int Id);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters);
        bool Remove(int Id, int UserID);
        int POMasterDetailsInsertUpdate(PO_MasterModel model);
        public PO_MasterModel FindCaseNo(string CaseNo);
    }
}
