using System.ComponentModel.DataAnnotations;
using System.Composition;

namespace IBS.Models
{
    public class TechReferenceModel
    {
        public string? RegionCd { get; set; }
        [Required(ErrorMessage = "CONTROLLING OFFICER is required")]
        public int? TechCmCd { get; set; }

        public string? TechCmName { get; set; }

        public string? TechIeName { get; set; }
        [Required(ErrorMessage = "INSPECTION ENGINEER is required")]
        public int? TechIeCd { get; set; }
        [Required(ErrorMessage = "ITEM is required")]
        public string? TechItemDes { get; set; }
        [Required(ErrorMessage = "SPEC & DRG is required")]
        public string? TechSpecDrg { get; set; }

        [Required(ErrorMessage = "RITES Letter No is required")]
        public string? TechLetterNo { get; set; }

        [Required(ErrorMessage = "Letter Date is required")]
        public DateTime? TechDate { get; set; }
        [Required(ErrorMessage = "REFERENCE MADE TO is required")]
        public string? TechRefMade { get; set; }
        [Required(ErrorMessage = "CONTACT OF TECHNICAL REFERENCE IN BRIEF is required")]
        public string? TechContent { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public string? TechId { get; set; }

        public byte? Isdeleted { get; set; }

        public DateTime? Createddate { get; set; }

        public int? Createdby { get; set; }

        public DateTime? Updateddate { get; set; }

        public int? Updatedby { get; set; }
        public int? Id { get; set; }

        public string? OUT_TECH_ID { get; set; }
        public string? OUT_ERR_CD { get; set; }
    }
}
