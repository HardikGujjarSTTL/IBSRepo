using IBS.DataAccess;
using IBS.Models.Reports;

namespace IBS.Models
{
    public class SummaryVendorWiseInspModel
    {
        public int SrNo { get; set; }
        public string VENDOR { get; set; }

        public string NO_OF_INSP { get; set; }

        public string MATERIAL_VALUE { get; set; }

        public string INSP_FEE { get; set; }

        public string Region { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string FromDt { get; set; }
        public string ToDt { get; set; }
        public string ReportType { get; set; }
        public string ReportTitle { get; set; }
        public string ForGiven { get; set; }
        public string ReportBasedon { get; set; }
        public string MaterialValue { get; set; }
        public string ForParticular { get; set; }
        public string lstParticular { get; set; }
        public List<SummaryVendorWiseInspModel> lstSummaryConreport { get; set; }
    }
}
