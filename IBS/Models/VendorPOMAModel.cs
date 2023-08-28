using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorPOMAModel
    {
        public string? CASE_NO { get; set; }

        public string PO_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PO_DT { get; set; }

        public string MA_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? MA_DT { get; set; }

        public string RLY_NONRLY { get; set; }

        public string RLY_CD { get; set; }

        public string PO_OR_LETTER { get; set; }

        public string MA_SNO { get; set; }

        public string MA_FIELD { get; set; }

        public string MA_STATUS { get; set; }

        public string S_RCE { get; set; }

        public string VEND_CD { get; set; }

        public string VEND_CD_S { get; set; }

        public string BPO_TYPE { get; set; }

        public string MA_DESC { get; set; }

        public string OLD_PO_VALUE { get; set; }

        public string NEW_PO_VALUE { get; set; }

        public string PO_SRC { get; set; }

        public string MA_REMARKS { get; set; }

        public string VENDOR { get; set; }

        public string PO_SOURCE { get; set; }

        public string IMMS_RLY_CD { get; set; }

        public string REMARKS { get; set; }

        public string VEND_REMARKS { get; set; }

    }
    public class PODetailsModel
    {
        public string CaseNo { get; set; } = null!;

        public byte ItemSrno { get; set; }

        public string? ItemDesc { get; set; }

        public decimal? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ExtDelvDt { get; set; }

        public decimal? Qty_Passed { get; set; }

        public decimal? Qty_Rejected { get; set; }

        public decimal? Qty_Balance { get; set; }

    }

    public class POIREPSModel
    {
        public int? MAKEY { get; set; }

        public byte SLNO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? MAKEY_DATE { get; set; }

        public string MA_FLD_DESCR { get; set; }

        public string OLD_VALUE { get; set; }

        public string NEW_VALUE { get; set; }

        public string RITES_CASE_NO { get; set; }

        public string IMMS_RLY_CD { get; set; }

        public decimal? IMMS_POKEY { get; set; }

        public string MA_NO { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? MA_DT { get; set; }

        public string MA_STATUS { get; set; }

    }

    public class PCallDetailsModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallRecvDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallLetterDt { get; set; }

        public short CallSno { get; set; }

        public byte? CallInstallNo { get; set; }

        public string? IeName { get; set; }

        public string? CallStatus { get; set; }

        public string? ReasonReject { get; set; }

        public string? Reason { get; set; }

        public string CaseNo { get; set; } = null!;

    }

    public class CComplaintsModel
    {
        public string ITEM_DESC { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? REJ_MEMO_DT { get; set; }

        public string REJECTION_REASON { get; set; }

        public string BK_NO { get; set; }

        public string SET_NO { get; set; }

        public string? CONSIGNEE { get; set; }

        public string JI_STATUS_DESC { get; set; }

        public string BK_SET_NO { get; set; }
    }

    public class RVendorPlaceModel
    {
        public string? BillNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? IcDt { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public string? ReasonReject { get; set; }

        public string? IeName { get; set; }

        public string? Vendor { get; set; }

        public string? ItemDescPo { get; set; }

        public bool? IcTypeId { get; set; }

        public int? VendCd { get; set; }

        public string CaseNo { get; set; } = null!;
    }   
}
