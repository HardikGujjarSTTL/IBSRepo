using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ClientCallRptModel
    {
        public string L5NO_PO { get; set; }
        public string RLY_NONRLY { get; set; }
        
        public string CallCode { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DT { get; set; }
        public string PO_NO { get; set; }
        public string PO_DT { get; set; }
        public string RLY_CD { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string REASON_REJECT { get; set; }
        public string IE_NAME { get; set; }
        public string VendorCd { get; set; }
        public string CallDate { get; set; }
        public string DESIREDate { get; set; }
        public string ENGINEER_DEPUTED { get; set; }
        public string ENGINEER_CONTACT_NO { get; set; }
        public string CaseNo { get; set; }
        public string Purchaser { get; set; }
        public string Status { get; set; }
        public string HOLOGRAM_OR_OTHER { get; set; }
        public string VendorName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonPno { get; set; }
        public string CallSrno { get; set; }
        public string CONTROLLING_MANAGER	{ get; set; }
        public string REMARKS { get; set; }
        public string IC_DT { get; set; }
        public string BILL_AMOUNT { get; set; }
        public string ITEM_DESC { get; set; }
        public string QTY_TO_INSP { get; set; }
        public string QTY_REJECTED { get; set; }


        public string Vendor { get; set; }
        public string Manufacturer { get; set; }
        public string VEND_CD { get; set; }
        public string MFG_CD { get; set; }
        public string Consignee { get; set; }
        public string ITEM_DESC_PO { get; set; }
        
        public string CALL_MARK_DT { get; set; }
        
        public string IE_PHONE_NO { get; set; }
        
        public string PO_DATE { get; set; }
        public string CASE_NO { get; set; }
        public string REMARK { get; set; }
        public string DESIRE_DT { get; set; }
        public string CALL_STATUS { get; set; }
        public string COLOUR { get; set; }
        public string MFG_PERS { get; set; }
        public string MFG_PHONE { get; set; }
        public string CALL_SNO { get; set; }
        public string HOLOGRAM { get; set; }
        public string IC_PHOTO { get; set; }
        public string IC_PHOTO_A1 { get; set; }
        public string IC_PHOTO_A2 { get; set; }
        public int COUNT { get; set; }
        public string CO_NAME { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<ClientCallRptModel> lstreport { get; set; }       
    }
        
}
