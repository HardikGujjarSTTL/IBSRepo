using System.ComponentModel.DataAnnotations;

namespace IBS.Models
{
    public class ICCancellationModel
    {
        [Required]
        public string BkNo { get; set; } = null!;

        [Required]
        public string SetNo { get; set; } = null!;

        [Required]
        public int? IssueToIecd { get; set; }

        public string? IcStatus { get; set; }

        public DateTime? StatusDt { get; set; }

        public string Region { get; set; } = null!;

        public string? Remarks { get; set; }
        public int IsEdit { get; set; }
        public int? Createdby { get; set; }

        public DateTimeOffset? Createddate { get; set; }

        public int? Updatedby { get; set; }

        public DateTimeOffset? Updateddate { get; set; }

        public byte? Isdeleted { get; set; }

        public bool Status { get; set; }
        public bool IsAdmin { get; set; }
        public int? Id { get; set; }
    }

    public class ICCancellationListModel
    {
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_NAME { get; set; }
        public string IC_STATUS { get; set; }
        public string STATUS { get; set; }
        public string STATUS_DT { get; set; }
        public string REGION { get; set; }
        public string REGIONV { get;}
        public string REMARKS { get; set; }
        public string IStatus { get; set; }

    }

}
