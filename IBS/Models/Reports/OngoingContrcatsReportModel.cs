namespace IBS.Models.Reports
{
    public class OngoingContrcatsReportModel
    {
        public string StatusOffer { get; set; }
        public string Region { get; set; }
        public string StatusOffertxt { get; set; }
        public string Regiontxt { get; set; }
        public string rdoregionwise { get; set; }
        public string ReportTitle { get; set; }

        public List<OngoingContrcats> lstOngoingContrcats { get; set; }
    }

    public class OngoingContrcats 
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
