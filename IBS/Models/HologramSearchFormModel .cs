using IBS.DataAccess;

namespace IBS.Models
{
    public class HologramSearchFormModel
    {
        public string? HgRegion { get; set; }

        public string? HgNoFr { get; set; }

        public string? HgNoTo { get; set; }

        public DateTime? HgIssueDt { get; set; }

        public int? HgIecd { get; set; }

        public string? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public byte? Isdeleted { get; set; }

        public int? Createdby { get; set; }

        public int? Updatedby { get; set; }

        public DateTime? Createddate { get; set; }

        public DateTime? Updateddate { get; set; }

        public virtual T09Ie? HgIecdNavigation { get; set; }
    }
}
