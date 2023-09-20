using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ConsigneePurchaseModel
    {
        public int ConsigneeCd { get; set; }

        public string? ConsigneeType { get; set; }

        public string? CSName { get; set; }

        public string? ConsigneeDesig { get; set; }

        public string? ConsigneeDept { get; set; }

        public string? ConsigneeRailwayCD { get; set; }

        public string? FName { get; set; }

        public string? ConsigneeFirm { get; set; }

        public string? ConsigneeAdd1 { get; set; }

        public string? ConsigneeAdd2 { get; set; }

        [Display(Name = "City")]
        [Required]
        public int? ConsigneeCity { get; set; }

        public string? ConsigneeState { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "GSTIN No")]
        [Required]
        public string? GstinNo { get; set; }

        public string? SapCustCdCon { get; set; }

        public string? LegalName { get; set; }

        [Display(Name = "Pincode")]
        [Required]
        public string? PinCode { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }

    public class ConsigneePurchaseSearchModel
    {
        public string ConsigneeCode { get; set; }
        public string ConsigneeDesig { get; set; }
        public string RailwayPurchase { get; set; }
        public string City { get; set; }
        public string SapCustomerCode { get; set; }
        public string GSTIN_No { get; set; }
    }

    public class ConsigneePurchaseMasterSearchModel
    {
        public int ROWNO { get; set; }
        public string CONSIGNEE_CD { get; set; }
        public string CONSIGNEE_NAME { get; set; }
        public string DESIG_DESC { get; set; }
        public string CONSIGNEE_FIRM { get; set; }
        public string CONSIGNEE_ADD1 { get; set; }
        public string CONSIGNEE_CITY { get; set; }
        public string GSTIN_NO { get; set; }
    }
}
