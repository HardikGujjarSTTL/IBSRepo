using System.ComponentModel.DataAnnotations.Schema;

namespace IBS.Models
{
    public class NCRRegister
    {
        public string Action { get; set; } // "M" for Modify, "A" for Add
        public string SelectedOption { get; set; } // Selected radio button option
        public string CaseNo { get; set; }
        public string? BKNo { get; set; }
        public string PO_NO { get; set; }
        public string? SetNo { get; set; }
        public string? NC_NO { get; set; }
        public string Vendor { get; set; }
        public string InspectingEr { get; set; }
        public string IE_SNAME { get; set; }
        public string IC_NO { get; set; }
        public int CALL_SNO { get; set; }
        public short CALLSNO { get; set; }
        public int ContractNo { get; set; }
        public int QtyPassed { get; set; }
        public int VEND_CD { get; set; }
        public string CONSIGNEE { get; set; }
        public int? CONSIGNEE_CD { get; set; }
        public byte? CONSIGNEECD { get; set; }
        public string Item { get; set; }
        public byte Item_Srno_no { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? IC_DT { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? PO_DT { get; set; }
        public DateTime? ICDate { get; set; }
        public DateTime? NCRDate { get; set; }
        public DateTime? Date { get; set; }
        [NotMapped]
        public string? IeCd { get; set; }
        public byte? Ie_Cd { get; set; }
        public DateTime? CALL_RECV_DT { get; set; }
        public DateTime CALLRECVDT { get; set; }
        [NotMapped]
        public string? SetRegionCode { get; set; }
        public string? NCRClass { get; set; }
        public string? NCRCode { get; set; }
        public string? NcCdSno { get; set; }
        public string? CoFinalRemarks1 { get; set; }
        public string? RegionCode { get; set; }
        public string? UserID { get; set; }
        public NCRRegister Model { get; set; }
        public string JsonData { get; set; }
        public string rdononc { get; set; }
        public string msg { get; set; }

        public List<Remarks> inputValues { get; set; }
    }
}
