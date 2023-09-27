namespace IBS.Models.Reports
{
    public class IEAlterMappingReportModel
    {
        public List<IEAlterMappingReport> lstIEAlterMappingReport { get; set; }
    }

    public class IEAlterMappingReport
    {
        public string IE_Name { get; set; }
        public string Altie_Name { get; set; }
        public string Altie_two_name { get; set; }
        public string Altie_three_name { get; set; }
    }
}
