﻿using IBS.DataAccess;

namespace IBS.Models
{
    public class DEOVendorPurchesOrderModel
    {
        public string CaseNo { get; set; } = null!;

        public int? PurchaserCd { get; set; }

        public string? Purchaser { get; set; }

        public string? StockNonstock { get; set; }

        public string? RlyNonrly { get; set; }

        public string? PoNo { get; set; }

        public DateTime? PoDt { get; set; }

        public string? PoDtS { get; set; }

        public DateTime? RecvDt { get; set; }

        public string? RecvDtS { get; set; }

        public int? VendCd { get; set; }

        public string? RlyCd { get; set; }

        public string? RlyCdDesc { get; set; }

        public string? RegionCode { get; set; }

        public string? RealCaseNo { get; set; }

        public string? Remarks { get; set; }

        public DateTime? Datetime { get; set; }

        public int? PoiCd { get; set; }

        public string? PoOrLetter { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }

        public string? UserId { get; set; }

        public virtual T06Consignee? PurchaserCdNavigation { get; set; }

        public virtual T01Region? RegionCodeNavigation { get; set; }

        public virtual ICollection<T82PoDetail> T82PoDetails { get; set; } = new List<T82PoDetail>();

        public virtual T05Vendor? VendCdNavigation { get; set; }
        public string? VendName { get; set; }
        public bool IsFileExist { get; set; }
        public bool IsPO_DOC { get; set; }
        public string? File { get; set; }
    }
}
