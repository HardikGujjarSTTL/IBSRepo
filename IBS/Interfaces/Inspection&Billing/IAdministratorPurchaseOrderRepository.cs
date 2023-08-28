using IBS.Models;

namespace IBS.Interfaces.Inspection_Billing
{
    public interface IAdministratorPurchaseOrderRepository
    {
        public AdministratorPurchaseOrderModel FindByID(string CaseNo);
        DTResult<AdministratorPurchaseOrderListModel> GetPOMasterList(DTParameters dtParameters,string region_code);
        bool Remove(string CaseNo, int UserID);
        string POMasterDetailsInsertUpdate(AdministratorPurchaseOrderModel model);
        public PO_MasterModel FindCaseNo(string CaseNo,int VendCd);
        DTResult<PO_MasterDetailListModel> GetPOMasterDetailsList(DTParameters dtParameters);
        bool RemovePODetails(string CaseNo,string ITEM_SRNO, int UserID);
        public int GenerateITEM_SRNO(string CASE_NO);
        public PO_MasterDetailsModel FindPODetailsByID(string CASE_NO, string ITEM_SRNO);
        DTResult<PO_MasterDetailsModel> FindByUOMDetail(decimal id);
        int POMasterSubDetailsInsertUpdate(PO_MasterDetailsModel model);
        string UpdateRealCaseNo(DEOVendorPurchesOrderModel model);
        string getVendorEmail(string CASE_NO);

        string[] GenerateRealCaseNo(string REGION_CD, string CASE_NO, string USER_ID);
        DTResult<ConsigneeListModel> GetConsigneeDetaisList(DTParameters dtParameters);
        bool ConsigneeDelete(string CASE_NO, string CONSIGNEE_CD,string BPO_CD);
    }
}
