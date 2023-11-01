using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web;

namespace IBSReports.ReportClass
{
    public class DailyIEWiseCall
    {
        public static string Oracle_UserID = System.Configuration.ConfigurationManager.AppSettings["Oracle_UserID"].ToString();
        public static string Oracle_Password = System.Configuration.ConfigurationManager.AppSettings["Oracle_Password"].ToString();

        public static ReportDocument IEWiseCall(string CallMarkDt, string CaseNo, out DataSet dsCustom)
        {
            ReportDocument rd = new ReportDocument();
            string formula = "ToText({T17_CALL_REGISTER.CALL_MARK_DT},'dd/MM/yyyy')='" + CallMarkDt + "' and Left ({T13_PO_MASTER.CASE_NO}, 1)='" + CaseNo + "'";
            
            dsCustom = new DataSet();
            try
            {
                rd.Load(HttpContext.Current.Server.MapPath("~/Reports/rptDailyIEWiseCall.rpt"));
                rd.RecordSelectionFormula = formula;
                rd.SetDatabaseLogon(Oracle_UserID, Oracle_Password);
            }
            catch
            {

            }
            return rd;
        }
    }
}