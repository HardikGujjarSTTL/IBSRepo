using IBS.DataAccess;
using IBS.Models.Reports;

namespace IBS.Models
{
    public class InspectionStatusModel
    {
        public int SrNo { get; set; }
        public string SearchPurchaser { get; set; }
        public string PurchaserCD { get; set; }
        public string PURCHASER { get; set; }

        public string CASE_NO { get; set; }

        public string PO_NO { get; set; }

        public string PO_DT { get; set; }
        public string PO_DATE { get; set; }
        public string REMARKS { get; set; }
        public string IC_NO { get; set; }
        public string IC_DT { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DATE { get; set; }
        public string IE_SNAME { get; set; }
        public string IE_NAME { get; set; }
        public string BPO { get; set; }
        public string VENDOR { get; set; }
        public string INSP_FEE { get; set; }
        public string VISITS { get; set; }
        public string VALUE { get; set; }
        public string ITEM_DESC { get; set; }
        public string CONSIGNEE { get; set; }

        public string NO_OF_INSP { get; set; }

        public string MATERIAL_VALUE { get; set; }

        

        public string Region { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDt { get; set; }
        public string ToDt { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string ForGiven { get; set; }
        public string ReportBasedon { get; set; }
        public string MaterialValue { get; set; }
        public string TextPurchase { get; set; }
        public string ForParticular { get; set; }
        public string lstParticular { get; set; }
        public string HFromDate { get; set; }
        public string HToDate { get; set; }

        public string rdbGIE { get; set; }
        public string rdbForMonth { get; set; }
        public string ForGPer { get; set; }
        public string ddlVender { get; set; }


        public string CALL_RECV_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string FIRST_INSP_DATE { get; set; }
        public string LAST_INSP_DATE { get; set; }
        public string IC_DATE { get; set; }
        public string ITEM_DESC_PO { get; set; }
        public string QTY_PASSED { get; set; }
        public string QTY_REJECTED { get; set; }

        public string ClientType { get; set; }
        public string SelectType { get; set; }
        public string RLY_NONRLY { get; set; }
        public string RLY_CD { get; set; }
        public string L5NO_PO { get; set; }
        public string QTY_TO_INSP { get; set; }
        
        public string HOLOGRAM { get; set; }
        public string CALL_MARK_DT { get; set; }
        public string IC_PHOTO { get; set; }
        public string MANUFACTURER { get; set; }
        public string IE_PHONE_NO { get; set; }
        public string CALL_STATUS { get; set; }
        public string MFG_PERS { get; set; }
        public string MFG_PHONE { get; set; }
        public string CO_NAME { get; set; }

        public List<InspectionStatusModel> lstSummaryConreport { get; set; }
        public List<InspectionStatusModel> lstCallDetails { get; set; }
    }
}
