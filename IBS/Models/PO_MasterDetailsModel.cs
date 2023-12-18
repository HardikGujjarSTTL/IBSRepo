using IBS.DataAccess;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class PO_MasterDetailsModel
    {
        public string CaseNo { get; set; } = null!;

        public int ItemSrno { get; set; }

        public DateTime? PoDt { get; set; }

        public string? ItemDesc { get; set; }

        [Required(ErrorMessage = "Consignee is required")]
        public int? ConsigneeCd { get; set; }
        public decimal? UOMFactor { get; set; }

        public string? ConsigneeSearch { get; set; }
        public string? Consignee { get; set; }

        [Required(ErrorMessage = "Qty is required")]
        public decimal? Qty { get; set; }

        [Required(ErrorMessage = "Rate is required")]
        public decimal? Rate { get; set; }

        public byte? UomCd { get; set; }

        public decimal? BasicValue { get; set; }

        [Required(ErrorMessage = "GST/Sale Tax(%) is required")]
        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        //[Required(ErrorMessage = "Excise Type is required")]
        public string? ExciseType { get; set; }

        [Required(ErrorMessage = "Excise Per is required")]
        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        //[Required(ErrorMessage = "Discount Type is required")]
        public string? DiscountType { get; set; }

        [Required(ErrorMessage = "Discount Per is required")]
        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? Value { get; set; }

        [Required(ErrorMessage = "Last Date of Supply Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        [Required(ErrorMessage = "Delivery Deadline Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ExtDelvDt { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        //[Required(ErrorMessage = "Other Charges Type is required")]
        public string? OtChargeType { get; set; }
        //[Required(ErrorMessage = "Other Charges Per is required")]
        public decimal? OtChargePer { get; set; }

        public string? BpoCd { get; set; }

        public string? Bpo { get; set; }

        public string? ItemCd { get; set; }

        public string? PlNo { get; set; }
        public string? SBPO { get; set; }
                
        public string? BPO_CD { get; set; }
        public string? BPO_NAME { get; set; }

        public string? RlyCd { get; set; }
        public string? RlyNonrly { get; set; }
        public string? CONSIGNEE_CD { get; set; }
        public string? CONSIGNEE_NAME { get; set; }

        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }

        public virtual T80PoMaster CaseNoNavigation { get; set; } = null!;

        public virtual T04Uom? UomCdNavigation { get; set; }
        public string? DrawingNo { get; set; }
        public string? SpecificationNo { get; set; }
    }
    public class PO_MasterDetailListModel
    {
        public string CASE_NO { get; set; }
        public byte ITEM_SRNO { get; set; }
        public string ITEM_DESC { get; set; }
        public string CONSIGNEE_NAME { get; set; }
        public decimal? QTY { get; set; }
        public decimal? RATE { get; set; }
        public decimal? DATAVALUE { get; set; }
    }
}
