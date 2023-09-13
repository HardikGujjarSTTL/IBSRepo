namespace IBS.Models
{
    public class ConsigneeComplaintsReportModel
    {
        public string IN_REGION { get; set; }
        public string COMPLAINT_ID { get; set; }
        public string JI_SNO { get; set; }
        public string VENDOR { get; set; }
        public string PO_NO { get; set; }
        public string PO_DATE { get; set; }
        public string BK_SET { get; set; }
        public string IC_DATE { get; set; }
        public string ITEM_DESC { get; set; }
        public string CONSIGNEE { get; set; }
        public string IE_NAME { get; set; }
        public string QTY_OFF { get; set; }
        public string QTY_REJECTED { get; set; }
        public string REJECTION_VALUE { get; set; }
        public string DEPT { get; set; }
        public string COMPLAINT_DATE { get; set; }
        public string REJ_MEMO { get; set; }
        public string REJECTION_REASON { get; set; }
        public string NO_JI_RES { get; set; }
        public string JI_DATE { get; set; }
        public string STATUS { get; set; }
        public string DEFECT_DESC { get; set; }
        public string JI_STATUS_DESC { get; set; }
        public string CONCLUSION_DATE { get; set; }
        public string CO_NAME { get; set; }
        public string JI_IE_NAME { get; set; }
        public string ROOT_CAUSE_ANALYSIS { get; set; }
        public string CHK_STATUS { get; set; }
        public string TECH_REF { get; set; }
        public string ACTION_PROPOSED { get; set; }
        public string ANY_OTHER { get; set; }
        public string CAPA_STATUS { get; set; }
        public string DANDAR_STATUS { get; set; }

        public string CASE_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_CO_NAME { get; set; }
        public string COMP_RECV_REGION { get; set; }
        public string JI_REQUIRED { get; set; }
        public string ACTION { get; set; }
        public string ACTION_PROPOSED_DATE { get; set; }        
        public string REMARKS { get; set; }
        
        public string REJECTIONMEMOPATH { get; set; }  //=> "/REJECTION_MEMO/" + CASE_NO + "-" + BK_NO + "-" + SET_NO;
        public string COMPLAINTSCASESPATH { get; set; } //=> "/COMPLAINTS_CASES/" + CASE_NO + "-" + BK_NO + "-" + SET_NO;
        public string COMPLAINTSREPORTPATH { get; set; } //=> "/COMPLAINTS_REPORT/" + CASE_NO + "-" + BK_NO + "-" + SET_NO;

    }
}
