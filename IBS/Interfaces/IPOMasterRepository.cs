using IBS.Models;

namespace IBS.Interfaces
{
    public interface IPOMasterRepository
    {
        public PO_MasterModel FindByID(string CaseNo);
        DTResult<PO_MasterModel> GetPOMasterList(DTParameters dtParameters,int VendCd);
        bool Remove(string CaseNo, int UserID);
        string POMasterDetailsInsertUpdate(PO_MasterModel model);
        public PO_MasterModel FindCaseNo(string CaseNo,int VendCd);
    }
}
