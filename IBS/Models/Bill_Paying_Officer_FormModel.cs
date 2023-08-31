using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IBS.Models
{
    public class Bill_Paying_Officer_FormModel
    {
        public string BpoCd { get; set; } = null!;

        public string? BpoRegion { get; set; }

        public string? BpoType { get; set; }

        [Display(Name = "Bpo Name")]
        [Required]
        public string? BpoName { get; set; }

        public string? BpoRly { get; set; }

        public string? BpoRlylst { get; set; }

        public string? BpoAdd { get; set; }

        public int BpoCityCd { get; set; }

        public string BpoCity { get; set; }

        public string? BillPassOfficer { get; set; }

        public string? BpoFeeType { get; set; }

        public decimal? BpoFee { get; set; }

        public string? BpoTaxType { get; set; }

        public string? BpoFlg { get; set; }

        public string? BpoAdvFlg { get; set; }

        public string? BpoLocCd { get; set; }

        public string? BpoOrgn { get; set; }

        public string? BpoAdd1 { get; set; }

        public string? BpoAdd2 { get; set; }

        [Display(Name = "State")]
        [Required]
        public string? BpoState { get; set; }

        public string? lblBpoState { get; set; }

        public string? BpoPin { get; set; }

        [Phone]
        public string? BpoPhone { get; set; }

        public string? BpoFax { get; set; }

        [EmailAddress]
        public string? BpoEmail { get; set; }

        public string? PayWindowId { get; set; }

        public string? BpoCdOld { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        [Display(Name = "GSTIN Number")]
        [Required]
        public string? GstinNo { get; set; }

        public string? Au { get; set; }

        public string? AuDesc { get; set; }

        public string? SapCustCdBpo { get; set; }

        public string? LegalName { get; set; }

        [Display(Name = "PINCODE")]
        [Required]
        public string? PinCode { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual T01Region? BpoRegionNavigation { get; set; }

        public virtual ICollection<T14PoBpo> T14PoBpos { get; set; } = new List<T14PoBpo>();

        public virtual ICollection<T20Ic> T20Ics { get; set; } = new List<T20Ic>();

        public string ActionType { get; set; }
    }
}

