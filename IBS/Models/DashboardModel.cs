namespace IBS.Models
{
    public class DashboardModel
    {
        public string CASE_NO { get; set; }
        public string IE { get; set; }
        public string Date { get; set; }
        public string Vendor { get; set; }
        public string SampleRegNo { get; set; }
        public string SampleRecDt { get; set; }
        public int TOTAL_INVOICE { get; set; }
        public int FINALIZED_INVOICE { get; set; }
        public int PENDING_FINALIZED_INVOICE { get; set; }
        public int TotalUploaded { get; set; }
        public string TotalBillAmount { get; set; }

        public int Total_Number_Of_Samples { get; set; }
        public int Internal { get; set; }
        public int External { get; set; }
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

        public int NOofBill { get; set; }

        public int ICISSUERECEIVEOFFICENOTBILL { get; set; }

        public int NOOFIEPERCM { get; set; }

        public string IE_NAME { get; set; }

        public string ActionType { get; set; }

        #region "LO"
        public int TotalBillCount { get; set; }

        public int TotalOutstandingCount { get; set; }

        public int TotalPassedCount { get; set; }

        public decimal TotalBillRupees { get; set; }
        #endregion

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
        public List<LABREGISTERModel> lstsampledata { get; set; } = new List<LABREGISTERModel>();
        public List<InstructionsIE> lstInstructionsIE { get; set; }
        public List<AdminViewAllList> lstAdminViewAllList { get; set; }

        #region CM JI Dashboard
        public List<CM_Odlest_Pending_JI_Cases_Model> oldestPendingJICases { get; set; } = new List<CM_Odlest_Pending_JI_Cases_Model>();
        public List<CM_NO_OF_Cons_Comp_Model> ieNoOfComp { get; set; } = new List<CM_NO_OF_Cons_Comp_Model>();
        public List<CM_NO_OF_Cons_Comp_Model> vendorNoOfComp { get; set; } = new List<CM_NO_OF_Cons_Comp_Model>();
        public List<CM_NO_OF_Cons_Comp_Model> clientNoOfComp { get; set; } = new List<CM_NO_OF_Cons_Comp_Model>();
        public Inter_Region_JI_Cons_Comp_Model interRegionJIComp { get; set; } = new Inter_Region_JI_Cons_Comp_Model();
        public string DefectCodeJISummary { get; set; }
        public CM_Defect_Code_JI_Comp_Model defectCodeJIComp { get; set; } = new CM_Defect_Code_JI_Comp_Model();
        public string NoOfJISummary { get; set; }
        public No_Of_JI_Model noOfJI { get; set; } = new No_Of_JI_Model();
        #endregion
        public List<AdminCountListing> lstAdminCountListing { get; set; }
        

        #region CM DFO Dashboard
        public decimal TotalOutstandingAmount { get; set; }
        public decimal SuspenseAmount { get; set; }
        public int NoOfOutstandingBillCount { get; set; }
        public int TotalInvoiceCount { get; set; }
        public int FinalizedInvoiceCount { get; set; }
        public int PendingInvoiceFinalizeCount { get; set; }

        public List<ClientDetailListModel> lstBillingClient { get; set; } = new List<ClientDetailListModel>();
        public List<ClientDetailListModel> lstOutstandingClient { get; set; } = new List<ClientDetailListModel>();
        public List<Billing_Comparison_Model> lstBillingCompare { get; set; } = new List<Billing_Comparison_Model>();

        public string CurrYearSectorBillingSummary { get; set; }
        public List<Sector_Billing_Model> lstCurrYearSectorBilling { get; set; } = new List<Sector_Billing_Model>();

        public string LastYearSectorBillingSummary1 { get; set; }
        public string LastYearSectorBillingSummary2 { get; set; }
        public string LastYearSectorBillingSummary3 { get; set; }
        public List<Sector_Billing_Model> lstLastThreeYearSectorBilling { get; set; } = new List<Sector_Billing_Model>();                
        #endregion
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
        public string CLIENT_NAME { get; set; }
        public string IE_NAME { get; set; }
        public string IE_PHONE_NO { get; set; }
        public string CO_NAME { get; set; }
        public string CO_PHONE_NO { get; set; }
        public string STATUS { get; set; }
    }

    public class RecentPOList
    {
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public string PO_NO { get; set; }
        public DateTime? PO_DT { get; set; }
        public DateTime? RECV_DT { get; set; }
        public string PO_OR_LETTER { get; set; }
        public string CLIENT_NAME { get; set; }
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
        public DateTime? PO_DT { get; set; }
        public DateTime? RECV_DT { get; set; }
        public string PO_OR_LETTER { get; set; }
        public int QTY { get; set; }
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

    public class AdminViewAllList
    {
        public string ClientName { get; set; }
        public string CaseNo { get; set; }
        public int NoofBills { get; set; }
        public decimal Value { get; set; }
        public string PONO { get; set; }
        public string Region { get; set; }
        public string ActionType { get; set; }
        public int NoofConComp { get; set; }
        public DateTime CallDate { get; set; }
    }

    public class VendorViewAllList
    {
        public string CallSno { get; set; }
        public string CaseNo { get; set; }
        public string Client { get; set; }
        public string IE { get; set; }
        public string IEContactNo { get; set; }
        public string CM { get; set; }
        public string CmContactNo { get; set; }
        public string PONO { get; set; }           
        public string Status { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime? PO_DT { get; set; }
        public DateTime? RECV_DT { get; set; }
        public string PO_OR_LETTER { get; set; }
    }

    public class IEViewAllList
    {
        public int MessageID { get; set; }
        public DateTime? MessageDt { get; set; }
        public string Message { get; set; }
        public string LetterNo { get; set; }
        public DateTime? LetterDt { get; set; }
        public string CaseNo { get; set; }
        public DateTime CallDate { get; set; }
        public string CallSno { get; set; }
        public DateTime InspDate { get; set; }
        public string Client { get; set; }
        public string Vendor { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNo { get; set; }
    }

    public class LoListingModel
    {
        public string BillNo { get; set; }

        public string CaseNo { get; set;}

        public string BillAmount { get; set; }

        public string AmountReceived { get; set;}

        public string PassedOutst { get; set; }

        public string ActionType { get; set;}
    }

    public class CM_Odlest_Pending_JI_Cases_Model
    {
        public string CONSIGNEE { get; set; }
        public string IE_NAME { get; set; }
        public string JI_REGION { get; set; }
        public string CASE_NO { get; set; }
        public DateTime? CALL_RECV_DT { get; set; }
        public int CALL_SNO { get; set; }
        public string JI_SNO { get; set; }
        public DateTime? JI_DT { get; set; }
    }

    public class CM_NO_OF_Cons_Comp_Model
    {
        public string NAME { get; set; }
        public int NO_OF_CONSINEE_COMPLAINTS { get; set; }
    }

    public class Inter_Region_JI_Cons_Comp_Model
    {
        public int EE { get; set; }
        public int EN { get; set; }
        public int ES { get; set; }
        public int EW { get; set; }
        public int NE { get; set; }
        public int NN { get; set; }
        public int NS { get; set; }
        public int NW { get; set; }
        public int SE { get; set; }
        public int SN { get; set; }
        public int SS { get; set; }
        public int SW { get; set; }
        public int WE { get; set; }
        public int WN { get; set; }
        public int WS { get; set; }
        public int WW { get; set; }
    }

    public class CM_Defect_Code_JI_Comp_Model
    {
        public int VISUAL { get; set; }
        public int DIAMENSIONAL { get; set; }
        public int CHEMICAL_COMPOSITION { get; set; }
        public int PHYSICAL { get; set; }
        public int SURFACE { get; set; }
        public int LOAD_PERFORMANCE { get; set; }
        public int NDT { get; set; }
        public int MACRO_MICRO { get; set; }
        public int ELECTRICAL { get; set; }
        public int WELDING { get; set; }
        public int OTHER { get; set; }
        public int TOTAL => VISUAL + DIAMENSIONAL + CHEMICAL_COMPOSITION + PHYSICAL + SURFACE + LOAD_PERFORMANCE + NDT + MACRO_MICRO + ELECTRICAL + WELDING + OTHER;
    }

    public class AdminCountListing
    {
        public string CaseNo { get; set; }  
        public DateTime CallRecvDt { get; set; }  
        public DateTime? PoDt { get; set; }  
        public byte? CallInstallNo { get; set; }  
        public int CallSno { get; set; }  
        public string CallStatus { get; set; }  
        public string CallLetterNo { get; set; }  
        public DateTime? CallLetterDt { get; set; }  
        public string Remarks { get; set; }  
        public string PoNo { get; set; }  
        public string IeSname { get; set; }  
        public string Vendor { get; set; }  
        public string RegionCode { get; set; }
        public DateTime? BILLDT { get; set; }
        public string BILLNO { get; set; }
        public decimal? billamount { get; set; }
        public string BKNO { get; set; }
        public string SETNO { get; set; }
        public string IC_NO { get; set; }
        public string ActionType { get; set; }
        public DateTime? IC_DT { get; set; }
    }

    public class CMDARListing
    {
        public string Complaint_ID { get; set;}
        public DateTime? Complaint_DT { get; set;}
        public string Case_No { get; set;}
        public string Rej_Memo_No { get; set;}
        public DateTime? Rej_Memo_Dt { get; set;}
        public decimal? RATE { get; set;}
    }

    public class CLientViewAllList
    {
        public string Vendor { get; set; }
        public int TotalCalls { get; set; }
        public int CallRejected { get; set; }
        public int CallCancelled { get; set; }
        public int CallSno { get; set; }
        public string CaseNo { get; set; }
        public DateTime CallDate { get; set; }
        public DateTime? PODT { get; set; }
        public string Details { get; set; }
        public string IEName { get; set; }
        public string Status { get; set; }
        public string PONO { get; set; }
        public int Qty { get; set; }
        public int NoOfComplaints { get; set; }
        public string Issues { get; set; }
        public string ActionType { get; set; }
        public DateTime? RECV_DT { get; set; }
        public Decimal? Value { get; set; }
        public  string Po_Or_Letter { get; set; }
    }

    public class Billing_Comparison_Model
    {
        public string FINALCIAL_YEAR { get; set; }
        public decimal AMOUNT { get; set; }
    }

    public class Sector_Billing_Model
    {
        public string YEAR { get; set; }
        public string SECTOR { get; set; }
        public decimal AMOUNT { get; set; }
    }

    public class CMDFOListing
    {
        public string CaseNo { get; set; }
        public DateTime CHQ_DT { get; set; }
        public string CHQ_NO { get; set; }
        public string NARRATION { get; set; }
        public decimal SUSPENSE_AMT { get; set; }
        public string VCHR_NO { get; set; }
        public string BILL_NO { get; set; }
        public DateTime BILL_DT { get; set; }
        public decimal MATERIAL_VALUE { get; set; }
        public decimal BILL_AMOUNT { get; set; }
        public string BILL_STATUS { get; set; }
        public string ActionType { get; set; }
        public string REMARKS { get; set; }
    }
    

    public class No_Of_JI_Model
    {
        public int NR { get; set; }
        public int WR { get; set; }
        public int SR { get; set; }
        public int ER { get; set; }
    }

    public class CM_JI_ViewAll_Model
    {
        public  string Type { get; set; }

        #region Oldest Pending JI
        public string CONSIGNEE { get; set; }
        public string IE_NAME { get; set; }
        public string JI_REGION { get; set; }
        public string CASE_NO { get; set; }
        public DateTime CALL_RECV_DT { get; set; }
        public string CALL_SNO { get; set; }
        public string JI_SNO { get; set; }
        public DateTime? JI_DT { get; set; }
        public DateTime? COMPLAINT_DT { get; set; }
        #endregion

        #region IE,Vendor and Client Wise Consignee Complaints
        public string NAME { get; set; }
        public int NO_OF_CONSINEE_COMPLAINTS { get; set; }
        #endregion

        #region IE Wise Performance        
        public int TOTAL_CALL { get; set; }
        public int PENDING_CALL { get; set; }
        public int ACCEPTED_CALL { get; set; }
        public int CANCELLED_CALL { get; set; }
        public int UNDER_LAB_CALL { get; set; }
        public int STILL_INSP_CALL { get; set; }
        public int STAGE_REJECTION_CALL { get; set; }
        #endregion
    }
}
