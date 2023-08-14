using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class VendorCallsMarkedForSpecificPOModel
    {
        public string L5NoPo { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string RlyNorly { get; set; }

        public string RlyCd { get; set; }
    }
    public class VendorCallsForSpecificPOReportModel
    {
        public string L5NoPo { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string RlyNorly { get; set; }

        public string? RlyCd { get; set; }

        public string? Vendor { get; set; }

        public string? Manufacturer { get; set; }

        public int VendCd { get; set; }

        public int? MfgCd { get; set; }

        public string? Consignee { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyToInsp { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public string? IeName { get; set; }

        public string? IePhoneNo { get; set; }

        public string CaseNo { get; set; } = null!;

        public string? Remark { get; set; }

        public string? CallStatus { get; set; }

        public string? Colour { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public short CallSno { get; set; }

        public string? Hologram { get; set; }

        public string? IcPhoto { get; set; }

        public string? IcPhotoA1 { get; set; }

        public string? IcPhotoA2 { get; set; }

        public decimal? Count { get; set; }

        public string? CoName { get; set; }



    }

    public class VendorCallsMarkedForSpecificICModel
    {
        public string? Purchaser { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? IcNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? IcDt { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public string? BillNo { get; set; }

        public string? IeName { get; set; }

        public string? Vendor { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public string QtyPassReject { get; set; }

        public string? Hologram { get; set; }

        public string? IcPhoto { get; set; }

        public string? IcPhotoA1 { get; set; }

        public string? IcPhotoA2 { get; set; }

        public string? L5noPo { get; set; }

        public string? RlyNonrly { get; set; }

        public string? RlyCd { get; set; }

        public int? VendCd { get; set; }
    }

    public class VendorCallsMarkedForSpecificICSubModel
    {
        public string? Purchaser { get; set; }

        public string? PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string? IcNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? IcDt { get; set; }

        public string? BkNo { get; set; }

        public string? SetNo { get; set; }

        public string? BillNo { get; set; }

        public string? IeName { get; set; }

        public string? IePhoneNo { get; set; }

        public string? Vendor { get; set; }

        public string? Manufacturer { get; set; }

        public string? ItemDescPo { get; set; }

        public decimal? QtyToInsp { get; set; }

        public decimal? QtyPassed { get; set; }

        public decimal? QtyRejected { get; set; }

        public string QtyPassReject { get; set; }

        public string? Hologram { get; set; }

        public string? IcPhoto { get; set; }

        public string? IcPhotoA1 { get; set; }

        public string? IcPhotoA2 { get; set; }

        public string? L5noPo { get; set; }

        public string? RlyNonrly { get; set; }

        public string? RlyCd { get; set; }

        public int? VendCd { get; set; }

        public int? MfgCd { get; set; }

        public string? Consignee { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? CallMarkDt { get; set; }

        public string CaseNo { get; set; } = null!;

        public string? Remark { get; set; }

        public string? CallStatus { get; set; }

        public string? Colour { get; set; }

        public string? MfgPers { get; set; }

        public string? MfgPhone { get; set; }

        public short CallSno { get; set; }

        public decimal? Count { get; set; }

        public string? CoName { get; set; }
    }

}
