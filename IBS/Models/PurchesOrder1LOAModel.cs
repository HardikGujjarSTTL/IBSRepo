using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class PurchesOrder1LOAModel
    {
        public string CaseNo { get; set; }

        public string PoNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? PoDt { get; set; }

        public string RlyCd { get; set; }

        public string VendName { get; set; }

        public string ConsigneeSName { get; set; }

        public string InspectingAgency { get; set; }

        public string POSource { get; set; }

        public string PoYr { get; set; }

        public string PoDoc { get; set; }

        public string PoDoc1 { get; set; }


        //T15_PO_DETAIL
        public byte ItemSrno { get; set; }

        public int? lstItemDesc { get; set; }

        public string? ItemDesc { get; set; }

        public int? ConsigneeCd { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public byte? UomCd { get; set; }

        public decimal? UOMFactor { get; set; }

        public decimal? BasicValue { get; set; }

        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        public string? ExciseType { get; set; }

        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        public string? DiscountType { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? Value { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ExtDelvDt { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? OtChargeType { get; set; }

        public decimal? OtChargePer { get; set; }

        public string? ItemCd { get; set; }

        public string? PlNo { get; set; }

        public virtual T14PoBpo? C { get; set; }

        public virtual T13PoMaster CaseNoNavigation { get; set; } = null!;

        public virtual T04Uom? UomCdNavigation { get; set; }

        //VIEW_T15_PO_DETAIL
        public string? ConsigneeName { get; set; }

        public string? Createdby { get; set; }

        public string? Updatedby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public decimal? Isdeleted { get; set; }
    }
}
