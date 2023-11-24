namespace IBS.Models
{
    public class DashboardModel
    {
        public int TotalCallsCount { get; set; }

        public int PendingCallsCount { get; set; }

        public int AcceptedCallsCount { get; set; }

        public int CancelledCallsCount { get; set; }

        public int UnderLabTestingCount { get; set; }

        public int StillUnderInspectionCount { get; set; }

        public int StageRejectionCount { get; set; }

        public int DSCExpiryDateCount { get; set; }

        public int NCIsuedAgainstIECount { get; set; }

        public int OutstandingNCCount { get; set; }

        public int NotRecievedCount { get; set; }

        public int ConsigneeCompaintCount { get; set; }

        public int ManualRegCall { get; set; }

        public int OnlineRegCall { get; set; }

        public int POAwaitingCaseNo { get; set; }

        public int PendingCallRemarks { get; set; }

        public int PendingOnlineCallAwaitingMark { get; set; }

        public string IE_NAME { get; set; }

        public string Type { get; set; }

        public List<DashboardModel> IEWisePerformance { get; set; }

        public string ComplaintStatusSummary { get; set; }

        public ComplaintStatusModel complaintStatusSummaryModel { get; set; }

        public List<IEList> lstIE { get; set; }

        public List<POCallStatusList> lstPOCallStatus { get; set; }

        public List<RecentPOList> lstRecentPO { get; set; }

        public List<ClientVENDPOList> lstClientVEND { get; set; }

        public List<ClientRecentReqList> lstClientRecentReq { get; set; }

        public List<ClientRecentPOList> lstClientRecentPO { get; set; }

        public List<ClientVendConCompList> lstClientVendConComp { get; set; }

        public List<ConsigneeComplaint> lstConsigneeComplaint { get; set; }

        public List<NCIssued_Per_IE> lstNCIssued_Per_IE { get; set; }

        public List<ClientDetailListModel> lstHightPayment { get; set; } = new List<ClientDetailListModel>();
        public List<ClientDetailListModel> lstHightOutstanding { get; set; } = new List<ClientDetailListModel>();
        public List<RegionConsigneeComplaintsListModel> lstRegionConsComp { get; set; } = new List<RegionConsigneeComplaintsListModel>();
        public List<PendingOrJICaseListModel> lstPendingCase { get; set; } = new List<PendingOrJICaseListModel>();
        public List<PendingOrJICaseListModel> lstJiCase { get; set; } = new List<PendingOrJICaseListModel>();

        public List<InstructionsIE> lstInstructionsIE { get; set; }
    }

    public class IEList
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public DateTime INSP_DESIRE_DT { get; set; }
        public string CLIENT_NAME { get; set; }
        public string VEND_NAME { get; set; }
        public string CONTACT_PER { get; set; }
        public string CONTACT_NO { get; set; }
    }

    public class POCallStatusList
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public string DETAILS { get; set; }
        public string CLIENT_NAME { get; set; }
        public string IE_NAME { get; set; }
        public string IE_PHONE_NO { get; set; }
        public string CO_NAME { get; set; }
        public string CO_PHONE_NO { get; set; }
    }

    public class RecentPOList
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public string PO_NO { get; set; }
        public string CLIENT_NAME { get; set; }
        public string DETAILS { get; set; }
        public string PURCHASE_ORDER { get; set; }
        public string CALL_STATUS { get; set; }
    }

    public class ClientVENDPOList
    {
        public string RLY_CD { get; set; }
        public string RLY_NONRLY { get; set; }
        public string VEND_NAME { get; set; }
        public int TOTAL_CALL { get; set; }
        public int REJECTED_CALL { get; set; }
        public int CANCELLED_CALL { get; set; }
    }

    public class ClientRecentReqList
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public string VEND_NAME { get; set; }
        public string DETAILS { get; set; }
        public string IE_NAME { get; set; }
        public string CALL_STATUS { get; set; }
    }

    public class ClientRecentPOList
    {
        public string CASE_NO { get; set; }
        public decimal VALUE { get; set; }
        public string VEND_NAME { get; set; }
        public string PO_NO { get; set; }
        public DateTime PO_DT { get; set; }
    }

    public class ClientVendConCompList
    {
        public int NO_OF_COMPLAINTS { get; set; }
        public string VEND_NAME { get; set; }
    }

    public class IE_Per_CM_Model
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public string IE_NAME { get; set; }
        public string VEND_NAME { get; set; }
        public string CALL_STATUS { get; set; }
    }

    public class NCIssued_Per_IE
    {
        public string NC_NO { get; set; }
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public string IE_NAME { get; set; }
        public string BK_NO { get; set; }
        public string SetNo { get; set; }
        public string Consignee { get; set; }
    }

    public class ComplaintStatusModel
    {
        public string REGION { get; set; }
        public int PENDING { get; set; }
        public int ACCEPTED { get; set; }
        public int UPHELD { get; set; }
        public int SORTING { get; set; }
        public int RECTIFICATION { get; set; }
        public int PRICE_REDUCTION { get; set; }
        public int LIFTED_BEFORE_JI { get; set; }
        public int TRANSIT_DEMANGE { get; set; }
        public int UNSTAMPED { get; set; }
        public int NOT_ON_RITES { get; set; }
        public int DELETED { get; set; }
    }

    public class ConsigneeComplaint
    {
        public string CASE_NO { get; set; }
        public string PO_NO { get; set; }
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IC_NO { get; set; }
        public string JiSno { get; set; }
        public string ComplaintId { get; set; }
        public DateTime? PO_DT { get; set; }
    }

    public class ClientDetailListModel
    {
        public string CLIENT_NAME { get; set; }
        public int NO_OF_BILL { get; set; }
        public decimal AMOUNT { get; set; }
    }

    public class PendingOrJICaseListModel
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_DATE { get; set; }
        public string CALL_SNO { get; set; }
        public string PO_NO { get; set; }
    }

    public class RegionConsigneeComplaintsListModel
    {
        public string REGION { get; set; }
        public int NO_OF_CONSINEE_COMPLAINTS { get; set; }
    }

    public class VendorDetailListModel
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string CLIENT_NAME { get; set; }
        public string IE_NAME { get; set; }
        public string PO_NO { get; set; }
    }

    public class InstructionsIE
    {
        public int MessageId { get; set; }

        public string? LetterNo { get; set; }

        public DateTime? LetterDt { get; set; }

        public string? Message { get; set; }

        public DateTime? MessageDt { get; set; }

    }
}
