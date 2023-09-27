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

}
