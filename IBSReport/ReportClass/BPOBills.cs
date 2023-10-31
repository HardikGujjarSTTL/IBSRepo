using CrystalDecisions.CrystalReports.Engine;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace IBSReports.ReportClass
{
    public static class BPOBills
    {
        public static OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        public static ReportDocument BPOBill(string Month, string Year, string Region, string ReportType, string FromDate, string ToDate,string Rb1,string Rb2,string Rb5 ,string lstBpo,string ClientType,string Rb3,string Rb4,out DataSet dsCustom)
        {
            string wHdr_YrMth = Month + ", " + Year;
            string wYrMth = Year + Month;
            conn1.Open();
            OracleCommand cmd5 = new OracleCommand("select to_char(LAST_DAY(to_date('" + wYrMth + "','YYYYMM')),'DD-MON-YYYY') from dual", conn1);
            string curr_wYrMth = Convert.ToString(cmd5.ExecuteScalar());
            conn1.Close();
            string wRegion = "";
            string wRecordSelectionFormula = "";
            ReportDocument rd = new ReportDocument();
            dsCustom = new DataSet();
            try
            {
                if (Region == "N") { wRegion = "Northern Region"; }
                else if (Region == "S") { wRegion = "Southern Region"; }
                else if (Region == "E") { wRegion = "Eastern Region"; }
                else if (Region == "W") { wRegion = "Western Region"; }
                else if (Region == "C") { wRegion = "Central Region"; }

                if (ReportType == "P")
                {
                    if (Rb1 == "true")
                    { wRecordSelectionFormula = "{V22_BILL.BPO_CD}='" + lstBpo + "' and "; }
                    else if (Rb2 == "true")
                    { wRecordSelectionFormula = "{V22_BILL.BPO_TYPE}='" + ClientType + "' and "; }
                    else if (Rb5 == "true")
                    { wRecordSelectionFormula = "{V22_BILL.BPO_RLY}='" + lstBpo + "' and "; }

                    if (Rb3 == "true")
                    {
                        wRecordSelectionFormula = wRecordSelectionFormula + "ToText({V22_BILL.BILL_DT},'yyyyMM')='" + wYrMth + "' and ";
                        //rpt.SetParameterValue(1, wHdr_YrMth);
                    }
                    else if (Rb4 == "true")
                    {
                        string from = "", to = "";
                        to = ToDate.Trim();
                        from = FromDate.Trim();
                        if (from == "")
                        { from = "01/04/2008"; }
                        if (to == "")
                        { to = "31/12/2100"; }
                        //rpt.SetParameterValue(1, from + " to " + to);
                        from = dateconcate(from);
                        to = dateconcate(to);
                        wRecordSelectionFormula = wRecordSelectionFormula + "ToText({V22_BILL.BILL_DT},'yyyyMMdd')>='" + from + "' and ToText({V22_BILL.BILL_DT},'yyyyMMdd')<='" + to + "' and ";
                    }
                    
                    wRecordSelectionFormula = wRecordSelectionFormula + "{V22_BILL.REGION_CODE}='" + Region + "'";

                    //MemoryStream oStream=my_rbs.showrep(rpt);
                    rd.Load(HttpContext.Current.Server.MapPath("~/Reports/repBPO_Bills.rpt"));
                    rd.RecordSelectionFormula = wRecordSelectionFormula;
                    rd.SetDatabaseLogon("QA", "QA");

                }
            }
            catch
            {

            }
            return rd;
        }

        public static string dateconcate(string dt)
        {
            string myYear, myMonth, myDay;
            myYear = dt.Substring(6, 4);
            myMonth = dt.Substring(3, 2);
            myDay = dt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            return (dt1);
        }
    }
}