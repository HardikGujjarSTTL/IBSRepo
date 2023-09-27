namespace IBS.Models
{
    public class VendorClusterReportModel
    {
        public string department { get; set; }
        public string allreport { get; set; }
        public string departreport { get; set; }
        public string ReportTitle { get; set; }
        public List<VendorClusterList> lstVendorClusterList { get; set; }
    }

    public class VendorClusterList
    {
        public string Department { get; set; }
        public string Cluster_name { get; set; }
        public string geographical_partition { get; set; }
        public string Vend_cd { get; set; }
        public string vendor { get; set; }
        public string vend_add1 { get; set; }
        public string city { get; set; }
        public string Ie_name { get; set; }
    }
}
