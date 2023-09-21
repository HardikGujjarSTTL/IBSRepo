﻿
namespace IBS.Models.Reports
{
    public class NCRReport
    {
        public string month { get; set; }
        public string year { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AllCM { get; set; }
        public string forCM { get; set; }
        public string All { get; set; }
        public string Outstanding { get; set; }
        public string formonth { get; set; }
        public string monthChar { get; set; }
        public string forperiod { get; set; }
        public string ReportTitle { get; set; }
        public string controllingmanager { get; set; }
        public string reporttype { get; set; }

        public List<AllNCRCMIE> lstAllNCRCMIE { get; set; }
        public List<IECMWise> lstIECMWise { get; set; }
    }

    public class AllNCRCMIE
    {
        public string IECMName { get; set; }
        public string Total_NO_Call { get; set; }
        public string Total_NC { get; set; }
        public string Total_Minor { get; set; }
        public string Total_Major { get; set; }
        public string Total_Critical { get; set; }
        public string NO_NC { get; set; }
    }

    public class IECMWise
    {
        public string CASE_NO { get; set; }
        public string NCR_NO { get; set; }
        public string ITEM { get; set; }
        public string VENDOR { get; set; }
        public string IE_NAME { get; set; }
        public string CO_NAME { get; set; }
        public string NC { get; set; }
        public string NC_CD_SNO { get; set; }
        public string IE_ACTION1 { get; set; }
        public string IE_ACTION_DATE { get; set; }
        public string CO_FINAL_REMARKS1 { get; set; }
        public string CO_REMARK_DATE { get; set; }
    }
}
