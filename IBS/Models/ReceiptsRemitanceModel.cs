using IBS.DataAccess;
using IBS.Models.Reports;

namespace IBS.Models
{
    public class ReceiptsRemitanceModel
    {
        public int SrNo { get; set; }
        public string BPOType { get; set; }
        public string AccCD { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }

    }
}
