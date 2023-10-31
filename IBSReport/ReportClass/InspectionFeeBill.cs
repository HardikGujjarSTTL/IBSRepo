using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Data;
using System.Web;

namespace IBSReports.ReportClass
{
    public class InspectionFeeBill
    {
        public static string Oracle_UserID = ConfigurationManager.AppSettings["Oracle_UserID"].ToString();
        public static string Oracle_Password = ConfigurationManager.AppSettings["Oracle_Password"].ToString();

        public static ReportDocument NRBillGST(string CaseNo, string BkNo, string SetNo, out DataSet dsCustom)
        {
            ReportDocument rd = new ReportDocument();
            string formula = "{V22_BILL.CASE_NO}='" + CaseNo  + "' and {V22_BILL.BK_NO}<='" + BkNo + "' and {V22_BILL.SET_NO}='" + SetNo + "' and {V22_BILL.REGION_CODE}='" + CaseNo.Substring(0, 1) + "'";
            dsCustom = new DataSet();
            try
            {
                rd.Load(HttpContext.Current.Server.MapPath("~/Reports/NR_Bill_GST.rpt"));
                rd.SetDatabaseLogon(Oracle_UserID, Oracle_Password);
                rd.RecordSelectionFormula = formula;
            }
            catch
            {

            }
            return rd;
        }
    }
}