namespace IBS.Models
{
    public class ICCancellationModel
    {
        public string BkNo { get; set; } = null!;

        public string SetNo { get; set; } = null!;

        public int? IssueToIecd { get; set; }

        public string? IcStatus { get; set; }

        public DateTime? StatusDt { get; set; }

        public string Region { get; set; } = null!;

        public string? Remarks { get; set; }
        public int IsEdit { get; set; }
    }

    public class ICCancellationListModel
    {
        public string BK_NO { get; set; }
        public string SET_NO { get; set; }
        public string IE_NAME { get; set; }
        public string IC_STATUS { get; set; }
        public string STATUS_DT { get; set; }
        public string REGION { get; set; }
        public string REMARKS { get; set; }
    }

}
