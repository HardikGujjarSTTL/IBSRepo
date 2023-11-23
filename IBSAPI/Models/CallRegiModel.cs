using IBSAPI.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBSAPI.Models
{
    public class CallRegiModel
    {
        public string CaseNo { get; set; } 
        public DateTime? CallDate { get; set; }
        public int CallSNo { get; set; }

        public string? Purchaser { get; set; }
        public string? Vendor { get; set; }

        public DateTime? PurchaseOrderDate { get; set; }

        public string? PurchaseOrderNo { get; set; }

        public string? CallStatus { get; set; }
        public string? Region { get; set; }

        public string? PlaceofInspection { get; set; }

        public string? ContactPersonName { get; set; }

        public string? ManufacturerEmail { get; set; }

        public string? PhoneNumber { get; set; }

    }

    public class PODetailsModel
    {
        public string PurchaserCd { get; set; }
        public DateTime? PoDt { get; set; }
        public string VendCd { get; set; }

        public string? PoNo { get; set; }
        public string? Rly { get; set; }
        public string? L5noPo { get; set; }
        public string? RlyNonrly { get; set; }

        public string? VendAdd1 { get; set; }

        public string? VendContactPer1 { get; set; }
        public string? VendContactTel1 { get; set; }

        public string? VendStatus { get; set; }

        public DateTime? VendStatusDtFr { get; set; }

        public DateTime? VendStatusDtTo { get; set; }

        public string? VendEmail { get; set; }

        public string CallStage { get; set; }
        public string CallStatus { get; set; }
        public int? CallSno { get; set; }
        public string MsgStatus { get; set; }
        public DateTime? CallMarkDt { get; set; }
        public DateTime? CallStatusDt { get; set; }
        public DateTime? RecvDt { get; set; }
        public DateTime? DtInspDesire { get; set; }
        public string? RegionCode { get; set; }
        public string Region { get; set; }
    }
}
