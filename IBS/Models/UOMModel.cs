using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class UOMModel
    {
        public byte UomCd { get; set; }

        [Display(Name = "UOM Long Description")]
        [Required]
        public string? UomLDesc { get; set; }

        [Display(Name = "UOM Short Description")]
        [Required]
		public string? UomSDesc { get; set; }

        [Display(Name = "Division Factor")]
        [Required]
        public decimal? UomFactor { get; set; } = 1;

		public string? UserId { get; set; }

		public DateTime? Datetime { get; set; }

		public string? ImmsUomCd { get; set; }

        public byte? Isdeleted { get; set; } = 0;

		public int? Createdby { get; set; }

		public int? Updatedby { get; set; }

		public DateTime? Createddate { get; set; }

		public DateTime? Updateddate { get; set; }

		public virtual ICollection<T15PoDetail> T15PoDetails { get; set; } = new List<T15PoDetail>();

		public virtual ICollection<T82PoDetail> T82PoDetails { get; set; } = new List<T82PoDetail>();

	}
}
