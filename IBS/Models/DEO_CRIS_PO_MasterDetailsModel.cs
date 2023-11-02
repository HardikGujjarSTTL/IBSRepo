using IBS.DataAccess;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class DEO_CRIS_PO_MasterDetailsModel
    {
        public int IMMS_POKEY { get; set; }

        public string IMMS_RLY_CD { get; set; }

        public string ITEM_SRNO { get; set; }

        public string ITEM_DESC { get; set; }
        public DateTime? PoDt { get; set; }

        public string? ItemDesc { get; set; }

        [Required(ErrorMessage = "Consignee is required")]
        public int? ConsigneeCd { get; set; }
        public decimal? UOMFactor { get; set; }

        public string? ConsigneeSearch { get; set; }
        public string? Consignee { get; set; }

        public decimal? Qty { get; set; }

        public decimal? Rate { get; set; }

        public byte? UomCd { get; set; }
        public string? UOM { get; set; }

        public string? IMMS_UOM_CD { get; set; }

        public string? IMMS_UOM_DESC { get; set; }

        public decimal? BasicValue { get; set; }

        public decimal? SalesTaxPer { get; set; }

        public decimal? SalesTax { get; set; }

        public string? ExciseType { get; set; }

        public decimal? ExcisePer { get; set; }

        public decimal? Excise { get; set; }

        public string? OT_CHARGE_TYPE { get; set; }

        public decimal? OT_CHARGE_PER { get; set; }

        public decimal? OT_CHARGES { get; set; }


        public string? DiscountType { get; set; }

        public decimal? DiscountPer { get; set; }

        public decimal? Discount { get; set; }

        public decimal? OtherCharges { get; set; }

        public decimal? Value { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? DelvDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Common.CommonDateFormateForDT)]
        [DataType(DataType.Date)]
        public DateTime? ExtDelvDt { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? OtChargeType { get; set; }

        public decimal? OtChargePer { get; set; }

        public string? BpoCd { get; set; }

        public string? Bpo { get; set; }

        public string? ItemCd { get; set; }

        public string? PlNo { get; set; }
        public string? SBPO { get; set; }
                
        public string? BPO_CD { get; set; }

        public string? IMMS_BPO_CD { get; set; }
        public string? BPO_NAME { get; set; }


        public string? RlyCd { get; set; }
        public string? RlyNonrly { get; set; }
        public string? IMMS_CONSIGNEE_CD { get; set; }
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
    public class DEO_CRIS_PO_MasterDetailsListModel
    {
        public string IMMS_POKEY { get; set; }
        public string IMMS_RLY_CD { get; set; }
        public string ITEM_SRNO { get; set; }
        public string ITEM_DESC { get; set; }
        public string CONSIGNEE_NAME { get; set; }
        public string QTY { get; set; }
        public decimal? RATE { get; set; }
        public decimal? VALUE { get; set; }
    }
}
