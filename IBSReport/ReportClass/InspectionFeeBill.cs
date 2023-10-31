using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Web;

namespace IBSReports.ReportClass
{
    public class InspectionFeeBill
    {
        public static ReportDocument NRBillGST(string CaseNo, string BkNo, string SetNo, out DataSet dsCustom)
        {
            ReportDocument rd = new ReportDocument();
            string formula = "{V22_BILL.CASE_NO}='" + CaseNo  + "' and {V22_BILL.BK_NO}<='" + BkNo + "' and {V22_BILL.SET_NO}='" + SetNo + "' and {V22_BILL.REGION_CODE}='" + CaseNo.Substring(0, 1) + "'";

            dsCustom = new DataSet();

            try
            {
                rd.Load(HttpContext.Current.Server.MapPath("~/Reports/NR_Bill_GST.rpt"));
                rd.RecordSelectionFormula = formula;
                rd.SetDatabaseLogon("QA", "QA");
            }
            catch
            {

            }
            return rd;
        }
    }
}