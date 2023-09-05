using IBS.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class HologramSearchFormModel
    {
        public string? HgRegion { get; set; }

        [Required(ErrorMessage = "Hologram No. From is require")]        
        public string? HgNoFr { get; set; }

        [Required(ErrorMessage = "Hologram No. To is require")]
        public string? HgNoTo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? HgIssueDt { get; set; }
        public string Display_HgIssueDt { get { return this.HgIssueDt != null ? Common.ConvertDateFormat(this.HgIssueDt.Value) : ""; } }

        [Required(ErrorMessage ="IE Name is require")]
        public int? HgIecd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual T09Ie? HgIecdNavigation { get; set; }

        public virtual string HgIeName { get; set; } = "";

        public string lblHgNoFr { get; set; }
        public string lblHgNoTo { get; set; }

        public string IEStatus { get; set; }
    }    
}
