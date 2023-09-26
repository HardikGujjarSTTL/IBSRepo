namespace IBS.Models.Reports
{
    public class ContractReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Region { get; set; }
        public string clientname { get; set; }
        public string ReportTitle { get; set; }

        public List<Contrcats> lstContrcats { get; set; }
    }

    public class Contrcats
    {
        public string CONTRACT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CONTRACT_NO { get; set; }
        public string OFFER_DTE { get; set; }
        public string PER_FROM { get; set; }
        public string PER_TO { get; set; }
        public string CONTRACT_FEE_NUM { get; set; }
        public string CO_NAME { get; set; }
        public string CONTRACT_SPECIAL_CONDN { get; set; }
        public string CONTRACT_PANALTY { get; set; }
        public string CONT_INSP_FEE { get; set; }
        public string SCOPE_OF_WORK { get; set; }
        public string REGION { get; set; }
    }
}
