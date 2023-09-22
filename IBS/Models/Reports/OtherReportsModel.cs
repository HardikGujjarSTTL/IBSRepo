namespace IBS.Models.Reports
{
    public class OtherReportsModel
    {
        public string ReportType { get; set; }

        public string ReportTitle { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class ControllingOfficerIEModel
    {
        public string Region { get; set; }
        public List<ControllingOfficerModel> lstControllingOfficer { get; set; }
        public List<ControllingOfficerModel> lstCluster { get; set; }
        public List<IEModel> lstLocalIE { get; set; }
        public List<IEModel> lstOutstationIE { get; set; }
    }

    public class ControllingOfficerModel
    {
        public int CO_CD { get; set; }
        public string CO_NAME { get; set; }
        public string cluster_name { get; set; }
    }
    public class IEModel
    {
        public int IE_CO_CD { get; set; }
        public string IE_NAME { get; set; }
    }
}
