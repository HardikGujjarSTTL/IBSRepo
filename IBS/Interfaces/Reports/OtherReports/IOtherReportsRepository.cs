using IBS.Models;
using IBS.Models.Reports;

namespace IBS.Interfaces.Reports.OtherReports
{
    public interface IOtherReportsRepository
    {
        ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region);
        DTResult<CoIeWiseCallsListModel> GetCoIeWiseCalls(DTParameters dtParameters);//string CO, string Status, string IE,bool IsAllIE, bool IsCallDate);
        CoIeWiseCallsModel GetCoIeWiseCallsReport(string Case_No, string Call_Recv_Date, string Call_SNo);
        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod, string Region, string iecmname, string reporttype);
        public IEWiseTrainingReportModel GetIEWiseTrainingDetails(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea, string Region);

    }
}
