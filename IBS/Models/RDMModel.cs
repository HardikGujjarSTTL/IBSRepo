using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace IBS.Models
{
	public class RDMModel
	{
        public int RDesigCd { get; set; }

        [Display(Name = "Designation")]
        [Required]
        public string? RDesignation { get; set; }

        public byte? Isdeleted { get; set; } = 0;

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }
    }
}
