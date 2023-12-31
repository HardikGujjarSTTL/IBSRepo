﻿namespace IBS.Models.Reports
{
    public class ManagementReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromYearMonth { get; set; }

        public string ToYearMonth { get; set; }

        public string Outstanding { get; set; }

        public string ParticularCM { get; set; }

        public string ParticularSector { get; set; }

        public string Region { get; set; }

        public string Status { get; set; }

        public string ClientType { get; set; }

        public string BPORailway { get; set; }

        public int IeCd { get; set; }

        public string SortedOn { get; set; }

        public string CallRemarkingDate { get; set; }

        public string CallsStatus { get; set; }

    }

    public class IEPerformanceModel
    {
        public int RejectionsIssued { get; set; }

        public int TotalICs { get; set; }

        public int CallsAttendedWithin7Days { get; set; }

        public int CallsAttendedBeyond7Days { get; set; }

        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<IEPerformanceListModel> lstPerformance { get; set; }

        public List<IEPerformanceSummaryListModel> lstPerformanceSummaryList { get; set; }

        public class IEPerformanceListModel
        {
            public string IE_NAME { get; set; }

            public string DEPT { get; set; }

            public decimal C3 { get; set; }

            public decimal C7 { get; set; }

            public decimal CM7 { get; set; }

            public decimal C10 { get; set; }

            public decimal C0 { get; set; }

            public decimal INSP_FEE { get; set; }

            public decimal MATERIAL_VALUE { get; set; }

            public decimal AVERAGE_FEE { get; set; }

            public decimal CALLS { get; set; }

            public decimal CALL_CANCEL { get; set; }

            public decimal REJECTIONS { get; set; }
        }

        public class IEPerformanceSummaryListModel
        {
            public string RLY_NONRLY { get; set; }

            public decimal IC_COUNT { get; set; }

            public decimal MATERIAL_VALUE { get; set; }
        }
    }

    public class ClusterPerformanceModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ClusterPerformanceListModel> lstPerformance { get; set; }

        public class ClusterPerformanceListModel
        {
            public string CLUSTER_NAME { get; set; }

            public string DEPT { get; set; }

            public decimal C3 { get; set; }

            public decimal C7 { get; set; }

            public decimal CM7 { get; set; }

            public decimal C10 { get; set; }

            public decimal C0 { get; set; }

            public decimal INSP_FEE { get; set; }

            public decimal MATERIAL_VALUE { get; set; }

            public decimal AVERAGE_FEE { get; set; }

            public decimal CALLS { get; set; }

            public decimal CALL_CANCEL { get; set; }

            public decimal REJECTIONS { get; set; }
        }
    }

    public class RWBSummaryModel
    {
        public string FromYearMonth { get; set; }

        public string ToYearMonth { get; set; }

        public string FilterTitle { get; set; }

        public List<RWBSummaryListModel> lstRWBSummaryList { get; set; }

        public List<RBWSectorListModel> lstRBWSectorList { get; set; }

        public class RWBSummaryListModel
        {
            public string SECTOR { get; set; }

            public decimal NR_FEE { get; set; }

            public decimal NR_TAX { get; set; }

            public decimal NR_BILL_AMT { get; set; }

            public decimal NR_BILLLS { get; set; }

            public decimal WR_FEE { get; set; }

            public decimal WR_TAX { get; set; }

            public decimal WR_BILL_AMT { get; set; }

            public decimal WR_BILLLS { get; set; }

            public decimal ER_FEE { get; set; }

            public decimal ER_TAX { get; set; }

            public decimal ER_BILL_AMT { get; set; }

            public decimal ER_BILLLS { get; set; }

            public decimal SR_FEE { get; set; }

            public decimal SR_TAX { get; set; }

            public decimal SR_BILL_AMT { get; set; }

            public decimal SR_BILLLS { get; set; }

            public decimal CR_FEE { get; set; }

            public decimal CR_TAX { get; set; }

            public decimal CR_BILL_AMT { get; set; }

            public decimal CR_BILLLS { get; set; }
        }

        public class RBWSectorListModel
        {
            public string SECTOR { get; set; }

            public decimal INSP_FEE { get; set; }

            public decimal BILL_AMOUNT { get; set; }

            public decimal NO_OF_BILLLS { get; set; }
        }
    }

    public class RWCOModel
    {
        public DateTime FromDate { get; set; }

        public string Display_FromDate { get { return Common.ConvertDateFormat(this.FromDate); } }

        public string Outstanding { get; set; }

        public List<RWCOListModel> lsttRWCOList { get; set; }

        public class RWCOListModel
        {
            public string BPO_TYPE { get; set; }

            public string BPO_RLY { get; set; }

            public string BPO_ORGN { get; set; }

            public string BPO_TYPE_CD { get; set; }

            public decimal NR_OUTSTANDING { get; set; }

            public decimal WR_OUTSTANDING { get; set; }

            public decimal ER_OUTSTANDING { get; set; }

            public decimal SR_OUTSTANDING { get; set; }

            public decimal CR_OUTSTANDING { get; set; }

            public decimal TOT_SUSPENSE_NR { get; set; }

            public decimal TOT_SUSPENSE_WR { get; set; }

            public decimal TOT_SUSPENSE_ER { get; set; }

            public decimal TOT_SUSPENSE_SR { get; set; }

            public decimal TOT_SUSPENSE_CR { get; set; }

            public decimal TOT_ALL_OUTSTANDING { get; set; }

            public decimal TOT_ALL_SUSPENSE { get; set; }

        }
    }

    public class ICSubmissionModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ICSubmissionListModel> lstICSubmission { get; set; }

        public class ICSubmissionListModel
        {
            public int ID { get; set; }

            public DateTime? IC_SUBMIT_DATE { get; set; }

            public string Display_IC_SUBMIT_DATE { get { return this.IC_SUBMIT_DATE != null ? Common.ConvertDateFormat(this.IC_SUBMIT_DATE.Value) : ""; } }

            public string IE_NAME { get; set; }

            public string BK_NO { get; set; }

            public string SET_NO { get; set; }

        }
    }

    public class PendingICAgainstCallsModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<PendingICAgainstCallsListModel> lstPendingICAgainstCalls { get; set; }

        public class PendingICAgainstCallsListModel
        {
            public int ID { get; set; }

            public string CASE_NO { get; set; }

            public DateTime? CALL_RECV_DT { get; set; }

            public string Display_CALL_RECV_DT { get { return this.CALL_RECV_DT != null ? Common.ConvertDateFormat(this.CALL_RECV_DT.Value) : ""; } }

            public int CALL_SNO { get; set; }

            public string STATUS { get; set; }

            public string IE_NAME { get; set; }

            public string IE_STATUS { get; set; }

        }
    }

    public class SuperSurpriseDetailsModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<SuperSurpriseListModel> lstSuperSurprise { get; set; }

        public class SuperSurpriseListModel
        {
            public int ID { get; set; }

            public string SuperSurpriseNo { get; set; }

            public DateTime? SuperSurpriseDt { get; set; }

            public string Display_SuperSurpriseDt { get { return this.SuperSurpriseDt != null ? Common.ConvertDateFormat(this.SuperSurpriseDt.Value) : ""; } }

            public string CoName { get; set; }

            public string IeName { get; set; }

            public string Vendor { get; set; }

            public string ItemDesc { get; set; }

            public string NameScopeItem { get; set; }

            public string PreIntRej { get; set; }

            public string Discrepancy { get; set; }

            public string Outcome { get; set; }

            public string SbuHeadRemarks { get; set; }

        }
    }

    public class SuperSurpriseSummaryModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<SuperSurpriseSummaryListModel> lstSuperSurpriseSummary { get; set; }

        public class SuperSurpriseSummaryListModel
        {
            public int? CO_CD { get; set; }

            public string CO_NAME { get; set; }

            public int? IE_CD { get; set; }

            public string IE_NAME { get; set; }

            public int SUP_SUR_NO { get; set; }
        }
    }

    public class ConsignRejectModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ConsignRejectListModel> lstConsignReject { get; set; }

        public class ConsignRejectListModel
        {
            public int ID { get; set; }

            public string TempComplaintId { get; set; }

            public DateTime? TempComplaintDt { get; set; }

            public string Display_TempComplaintDt { get { return this.TempComplaintDt != null ? Common.ConvertDateFormat(this.TempComplaintDt.Value) : ""; } }

            public string ConsigneeName { get; set; }

            public string ConsigneeDesig { get; set; }

            public string ConsigneeEmail { get; set; }

            public string ConsigneeMobile { get; set; }

            public string RejMemoNo { get; set; }

            public DateTime? RejMemoDt { get; set; }

            public string Display_RejMemoDt { get { return this.RejMemoDt != null ? Common.ConvertDateFormat(this.RejMemoDt.Value) : ""; } }

            public string CaseNo { get; set; }

            public string BkNo { get; set; }

            public string SetNo { get; set; }

            public string InspRegion { get; set; }

            public int? IeCd { get; set; }

            public int? CoCd { get; set; }

            public int? ConsigneeCd { get; set; }

            public string ItemDesc { get; set; }

            public string Vendor { get; set; }

            public decimal? QtyRejected { get; set; }

            public decimal? RejectionValue { get; set; }

            public string RejectionReason { get; set; }

            public string Status { get; set; }

            public string TempCompRejReason { get; set; }

            public string ComplaintId { get; set; }

            public string JiRequired { get; set; }

            public string JiSno { get; set; }
        }
    }

    public class OutstandingOverRegionModel
    {
        public DateTime FromDate { get; set; }

        public string Display_FromDate { get { return Common.ConvertDateFormat(this.FromDate); } }

        public List<OutstandingOverRegionListModel> lstOutstandingOverRegion { get; set; }

        public class OutstandingOverRegionListModel
        {
            public int Count { get; set; }

            public string BpoRegion { get; set; }

            public string RegionCode { get; set; }

            public decimal Total { get; set; }
        }
    }

    public class ClientWiseRejectionModel
    {
        public DateTime FromDate { get; set; }

        public string Display_FromDate { get { return Common.ConvertDateFormat(this.FromDate); } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public string ClientType { get; set; }

        public string BPORailway { get; set; }

        public List<ClientWiseRejectionListModel> lstClientWiseRejection { get; set; }

        public class ClientWiseRejectionListModel
        {
            public int ID { get; set; }

            public string BillNo { get; set; }

            public DateTime? BillDt { get; set; }

            public string Display_BillDt { get { return this.BillDt != null ? Common.ConvertDateFormat(this.BillDt.Value) : ""; } }

            public string PoNo { get; set; }

            public DateTime? PoDt { get; set; }

            public string Display_PoDt { get { return this.PoDt != null ? Common.ConvertDateFormat(this.PoDt.Value) : ""; } }

            public string RlyCd { get; set; }

            public string BkNo { get; set; }

            public string SetNo { get; set; }

            public string ReasonReject { get; set; }

            public string IeName { get; set; }

            public string Vendor { get; set; }

            public DateTime? IcDt { get; set; }

            public string Display_IcDt { get { return this.IcDt != null ? Common.ConvertDateFormat(this.IcDt.Value) : ""; } }

            public decimal? BillAmount { get; set; }

            public string ItemDesc { get; set; }

            public decimal? QtyToInsp { get; set; }

            public decimal? QtyRejected { get; set; }
        }
    }

    public class NonConformityModel
    {
        public string FromYearMonth { get; set; }

        public string ToYearMonth { get; set; }

        public int IeCd { get; set; }

        public string IEName { get; set; }

        public List<NonConformityListModel> lstNonConformity { get; set; }

        public class NonConformityListModel
        {
            public int ID { get; set; }

            public string IE_CD { get; set; }

            public string NCR_MON { get; set; }

            public string NCR_MM { get; set; }

            public decimal A01 { get; set; }

            public decimal A02 { get; set; }

            public decimal A03 { get; set; }

            public decimal A04 { get; set; }

            public decimal A05 { get; set; }

            public decimal A06 { get; set; }

            public decimal A07 { get; set; }

            public decimal A08 { get; set; }

            public decimal A09 { get; set; }

            public decimal A10 { get; set; }

            public decimal A11 { get; set; }

            public decimal A12 { get; set; }

            public decimal A99 { get; set; }

            public decimal B01 { get; set; }

            public decimal B02 { get; set; }

            public decimal B03 { get; set; }

            public decimal B04 { get; set; }

            public decimal B05 { get; set; }

            public decimal B06 { get; set; }

            public decimal B07 { get; set; }

            public decimal B08 { get; set; }

            public decimal B09 { get; set; }

            public decimal B10 { get; set; }

            public decimal B11 { get; set; }

            public decimal B12 { get; set; }

            public decimal B13 { get; set; }

            public decimal B14 { get; set; }

            public decimal B99 { get; set; }

            public decimal C01 { get; set; }

            public decimal C02 { get; set; }

            public decimal C03 { get; set; }

            public decimal C04 { get; set; }

            public decimal C05 { get; set; }

            public decimal C06 { get; set; }

            public decimal C99 { get; set; }
        }
    }

    public class PendingCallsModel
    {
        public List<PendingCallsListModel> lstPendingCalls { get; set; }

        public class PendingCallsListModel
        {
            public string REGION { get; set; }

            public string SERIAL_CODE { get; set; }

            public decimal GRO_SEVEN_INSP { get; set; }

            public decimal GRO_SEVEN_NSIC { get; set; }

            public decimal GRO_FIVE_INSP { get; set; }

            public decimal GRO_FIVE_NSIC { get; set; }

            public decimal GRP_SEVEN_INSP { get; set; }

            public decimal GRP_SEVEN_NSIC { get; set; }

            public decimal GRP_FIVE_INSP { get; set; }

            public decimal GRP_FIVE_NSIC { get; set; }

        }
    }

    public class ICIssuedNotReceivedModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<ICIssuedNotReceivedListModel> lstICIssuedNotReceived { get; set; }

        public class ICIssuedNotReceivedListModel
        {
            public string CO_NAME { get; set; }

            public string IE_NAME { get; set; }

            public decimal NO_IC { get; set; }
        }
    }

    public class TentativeInspectionFeeWisePendingCallsModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<TentativeInspectionFeeWisePendingCallsListModel> lstTentativeInspectionFeeWisePendingCalls { get; set; }

        public class TentativeInspectionFeeWisePendingCallsListModel
        {
            public int ID { get; set; }

            public string CASE_NO { get; set; }

            public string CALL_SNO { get; set; }

            public DateTime? CALL_RECV_DT { get; set; }

            public string Display_CALL_RECV_DT { get { return this.CALL_RECV_DT != null ? Common.ConvertDateFormat(this.CALL_RECV_DT.Value) : ""; } }

            public string IE_NAME { get; set; }

            public string CO_NAME { get; set; }

            public string CLIENT_RLY { get; set; }

            public string RLY { get; set; }

            public decimal PENDING_SINCE { get; set; }

            public DateTime? INSP_DESIRE_DATE { get; set; }

            public string Display_INSP_DESIRE_DATE { get { return this.INSP_DESIRE_DATE != null ? Common.ConvertDateFormat(this.INSP_DESIRE_DATE.Value) : ""; } }

            public DateTime? EXT_DELV_DATE { get; set; }

            public string Display_EXT_DELV_DATE { get { return this.EXT_DELV_DATE != null ? Common.ConvertDateFormat(this.EXT_DELV_DATE.Value) : ""; } }

            public decimal MAT_VALUE { get; set; }

            public decimal INSP_FEE { get; set; }

        }
    }

    public class CallRemarkingModel
    {
        public string Region { get; set; }

        public DateTime? FromDate { get; set; }

        public string Display_FromDate { get { return this.FromDate != null ? Common.ConvertDateFormat(this.FromDate.Value) : ""; } }

        public DateTime? ToDate { get; set; }

        public string Display_ToDate { get { return this.ToDate != null ? Common.ConvertDateFormat(this.ToDate.Value) : ""; } }

        public List<CallRemarkingListModel> lstCallRemarking { get; set; }

        public class CallRemarkingListModel
        {
            public int ID { get; set; }

            public string CaseNo { get; set; }

            public DateTime? CallRecvDt { get; set; }

            public string Display_CallRecvDt { get { return this.CallRecvDt != null ? Common.ConvertDateFormat(this.CallRecvDt.Value) : ""; } }

            public int? CallSno { get; set; }

            public string RemarkingStatus { get; set; }

            public string RemarkReason { get; set; }

            public string IE_Name_From { get; set; }

            public string IE_Name_To { get; set; }

            public int? FrIePendingCalls { get; set; }

            public int? ToIePendingCalls { get; set; }

            public string User_Name { get; set; }

            public DateTime? RemInitDatetime { get; set; }

            public string Display_RemInitDatetime { get { return this.RemInitDatetime != null ? Common.ConvertDateTimeFormat(this.RemInitDatetime.Value) : ""; } }

            public string User_Name_App { get; set; }

            public DateTime? RemAppDatetime { get; set; }

            public string Display_RemAppDatetime { get { return this.RemAppDatetime != null ? Common.ConvertDateTimeFormat(this.RemAppDatetime.Value) : ""; } }

            public string CallRemarkStatus { get; set; }

        }
    }

    public class CallDetailsDashboradModel
    {
        public string Region { get; set; }

        public List<CallDetailsDashboradListModel> lstCallDetailsDashborad { get; set; }

        public class CallDetailsDashboradListModel
        {
            public string IE_DEPT { get; set; }

            public decimal CALL_MARKED { get; set; }

            public decimal CALL_ATTENDED { get; set; }

            public decimal CALL_PENDING { get; set; }

            public decimal IE_NO { get; set; }

            public decimal AVG_PEN { get; set; }
        }
    }
}
