using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ClientCallRptModel
    {
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
    }

}
