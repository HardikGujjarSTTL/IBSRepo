using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CrystalReportProject.ReportClass
{
    public class DailyIEWiseCall
    {
        public static ReportDocument IEWiseCall(string CallMarkDt, string CaseNo, out DataSet dsCustom)
        {
            
            ReportDocument rd = new ReportDocument();
            string formula = "ToText({T17_CALL_REGISTER.CALL_MARK_DT},'dd/MM/yyyy')='" + CallMarkDt + "' and Left ({T13_PO_MASTER.CASE_NO}, 1)='" + CaseNo + "'";
            
            dsCustom = new DataSet();
            try
            {
                rd.Load(HttpContext.Current.Server.MapPath("~/Reports/rptDailyIEWiseCall.rpt"));
                rd.RecordSelectionFormula = formula;
                //rd.SetDataSource("IBSQc");
                rd.SetDatabaseLogon("QA", "QA");
                
            }
            catch(Exception ex)
            {

            }
            return rd;
        }
    }
}