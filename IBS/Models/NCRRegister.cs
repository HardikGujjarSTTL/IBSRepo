using System.ComponentModel.DataAnnotations.Schema;

namespace IBS.Models
{
    public class NCRRegister
    {
        public string Action { get; set; } // "M" for Modify, "A" for Add
        public string SelectedOption { get; set; } // Selected radio button option
        public string CaseNo { get; set; }
        public string BKNo { get; set; }
        public string SetNo { get; set; }
        public string ICNO { get; set; }
        public string NC_NO { get; set; }
        public string Vendor { get; set; }
        public string InspectingEr { get; set; }
        public string IE_SNAME { get; set; }
        public int CALL_SNO { get; set; }
        public int ContractNo { get; set; }
        public int QtyPassed { get; set; }
        public string CONSIGNEE { get; set; }
        public string Item { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? ICDate { get; set; }
        public DateTime? NCRDate { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public int? IeCd { get; set; }
        public DateTime? CALL_RECV_DT { get; set; }
        [NotMapped]
        public string? SetRegionCode { get; set; }
        public string? NCRClass { get; set; }
        public string? NCRCode { get; set; }
    }
}
