using IBS.DataAccess;

namespace IBS.Models
{
    public class PO_MasterModel
    {
        public string CaseNo { get; set; } = null!;

        public int? PurchaserCd { get; set; }

        public string? StockNonstock { get; set; }

        public string? RlyNonrly { get; set; }

        public string? PoOrLetter { get; set; }

        public string? PoNo { get; set; }

        public string? L5noPo { get; set; }

        public DateTime? PoDt { get; set; }

        public DateTime? RecvDt { get; set; }

        public int? VendCd { get; set; }

        public string? RlyCd { get; set; }

        public string? RegionCode { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? Remarks { get; set; }

        public string? InspectingAgency { get; set; }

        public int? PoiCd { get; set; }

        public string? PoSource { get; set; }

        public byte? PendingCharges { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Id { get; set; }

        public virtual T06Consignee? PurchaserCdNavigation { get; set; }

        public virtual T01Region? RegionCodeNavigation { get; set; }

        public virtual ICollection<T14PoBpo> T14PoBpos { get; set; } = new List<T14PoBpo>();

        public virtual T14aPoNonrly? T14aPoNonrly { get; set; }

        public virtual ICollection<T15PoDetail> T15PoDetails { get; set; } = new List<T15PoDetail>();

        public virtual ICollection<T25RvDetail> T25RvDetails { get; set; } = new List<T25RvDetail>();

        public virtual ICollection<T40ConsigneeComplaint> T40ConsigneeComplaints { get; set; } = new List<T40ConsigneeComplaint>();

        public virtual T05Vendor? VendCdNavigation { get; set; }
    }
}
