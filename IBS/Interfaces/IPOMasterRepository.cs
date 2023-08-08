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
        DTResult<PO_MasterDetailListModel> GetPOMasterDetailsList(DTParameters dtParameters);
        bool RemovePODetails(string CaseNo,string ITEM_SRNO, int UserID);
        public int GenerateITEM_SRNO(string CASE_NO);
        public PO_MasterDetailsModel FindPODetailsByID(string CASE_NO, string ITEM_SRNO);
        DTResult<PO_MasterDetailsModel> FindByUOMDetail(decimal id);
        int POMasterSubDetailsInsertUpdate(PO_MasterDetailsModel model);
    }
}
