namespace IBS.Models
{
    public class CallsMarkedToIEModel
    {
        public string? Vendor { get; set; }

        public string? Manufacturer { get; set; }

        public int VendCd { get; set; }

        public int? MfgCd { get; set; }

        public string? Consignee { get; set; }

        public string? ItemDescPo { get; set; }

        public DateTime? ExtDelvDt { get; set; }

        public DateTime? CallMarkDt { get; set; }

        public DateTime? DtInspDesire { get; set; }

        public DateTime CallRecvDt { get; set; }

        public string? NewVendor { get; set; }

        public string? IeName { get; set; }

        public string? IePhoneNo { get; set; }

        public string? PoNo { get; set; }

        public DateTime? PoDt { get; set; }

        public string? PoYr { get; set; }

        public string? PoSource { get; set; }

        public string? Source { get; set; }

        public string? RlyCd { get; set; }

        public string CaseNo { get; set; } = null!;

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? Remarks { get; set; }

        public string? CallStatus { get; set; }

        public string? CallStatusFull { get; set; }

        public string? Colour { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public int CallSno { get; set; }

        public string? DocsSubmitted { get; set; }

        public int? IeCd { get; set; }

        public decimal? Count { get; set; }

        public string? callDocAny { get; set; }

        public string? PType { get; set; }

        public List<CallsMarkedToIEModelList> ReportLst { get; set; }

        public List<CallsMarkedToIEModelListNew> ReportLstNew { get; set; }

        public string FilePath1 { get; set; }
        public string FilePath2 { get; set; }
    }

    public class CallsMarkedToIEModelList
    {
        public string? Vendor { get; set; }

        public string? NewVendor { get; set; }

        public string? Consignee { get; set; }

        public string? ItemDescPo { get; set; }

        public DateTime? ExtDelvDt { get; set; }

        public DateTime? DtInspDesire { get; set; }

        public DateTime? CallMarkDt { get; set; }

        public int CallSno { get; set; }

        public string? callDocAny { get; set; }

        public string? PoSource { get; set; }

        public string? CallStatus { get; set; }

        public string? Remarks { get; set; }

        public string? PoNo { get; set; }

        public DateTime? PoDt { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string CaseNo { get; set; }

        public DateTime CallRecvDt { get; set; }

        public decimal? cnt { get; set; }

        public int? VendCd { get; set; }

        public int? MfgCd { get; set; }

        public string Manufacturer { get; set; }

        public string? Source { get; set; }

        public string? CallStatusFull { get; set; }

        public int? IeCd { get; set; }

    }

    public class CallsMarkedToIEModelListNew
    {
        public string VENDOR { get; set; }

        public string NEWVENDOR { get; set; }

        public string CONSIGNEE { get; set; }

        public string ITEM_DESC_PO { get; set; }

        public string EXT_DELV_DT { get; set; }

        public string INSP_DESIRE_DT { get; set; }

        public string CALL_MARK_DT { get; set; }

        public string CALL_SNO { get; set; }

        //public string? callDocAny { get; set; }

        public string PO_SOURCE { get; set; }

        public string CALL_STATUS { get; set; }

        public string REMARKS { get; set; }

        public string PO_NO { get; set; }

        public string PO_DATE { get; set; }

        public string PO_YR { get; set; }

        public string MFG_PERS { get; set; }

        public string MFG_PHONE { get; set; }

        public string USER_ID { get; set; }

        public string DATETIME { get; set; }

        public string CASE_NO { get; set; }

        public string CALL_RECV_DT { get; set; }

        public int COUNT { get; set; }

        public string VEND_CD { get; set; }

        public string MFG_CD { get; set; }

        public string MANUFACTURER { get; set; }

        public string SOURCE { get; set; }

        public string CALL_STATUS_FULL { get; set; }

        public string IE_CD { get; set; }

        public string IMMS_RLY_CD { get; set; }

        public string COLOUR { get; set; }

        public string LAB_STATUS { get; set; }

        public int PAYMENT_RECIEPT { get; set; }

        public string CM_APPROVAL { get; set; }

        
    }

}
