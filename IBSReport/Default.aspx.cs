using CrystalDecisions.CrystalReports.Engine;
using IBSReports.ReportClass;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;

namespace IBSReports
{
    public partial class _Default : System.Web.UI.Page
    {
        #region "Declarations.."

        OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        protected CrystalDecisions.Web.CrystalReportViewer cri;

        #endregion

        #region "Properites.."

        public string RptFlag
        {
            get
            {
                if ((Request.QueryString["RptFlag"] != null))
                {
                    return Request.QueryString["RptFlag"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet dsCustom = new DataSet();
            conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
            try
            {
                if (RptFlag == "1") // This flag is for Lab Invoice Report
                {
                    string InvoiceNo = Request.QueryString["Invoice"].ToString();
                    string Caseno = Request.QueryString["CaseNo"].ToString();
                    string RegNo = Request.QueryString["RegNo"].ToString();
                    string TranNo = Request.QueryString["TranNo"].ToString();
                    cristalview.ReportSource = LabInvoice.LabInvoiceReport(InvoiceNo, Caseno, RegNo, TranNo, out dsCustom);
                }
                else if (RptFlag == "2") // This flag is for Lab Invoice Download Report
                {
                    //string InvoiceNo = "R0608L22/00182";
                    //string InvoiceDT = "29-07-2022";
                    string InvoiceNo = Request.QueryString["Invoice"].ToString();
                    string InvoiceDT = Request.QueryString["InvoiceDt"].ToString();
                    cristalview.ReportSource = LabLABInvoice.LabInvoiceReport(InvoiceNo, InvoiceDT, out dsCustom);
                }
                else if (RptFlag == "3") // This flag is for Inspection Bill Report
                {
                    //string CaseNo = "E11100692";
                    //string BkNo = "2842";
                    //string SetNo = "051";
                    string CaseNo = Request.QueryString["CaseNo"].ToString();
                    string BkNo = Request.QueryString["BkNo"].ToString();
                    string SetNo = Request.QueryString["SetNo"].ToString();

                    //cristalview.ReportSource = InspectionFeeBill.NRBillGST(CaseNo, BkNo, SetNo, out dsCustom);

                    ReportDocument rd = InspectionFeeBill.NRBillGST(CaseNo, BkNo, SetNo, out dsCustom);
                    System.IO.Stream st = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    System.IO.MemoryStream s = new System.IO.MemoryStream();
                    CopyStream(st, s);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format("inline; filename={0}", "ICReport.pdf"));
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(s.ToArray());
                    Response.End();
                    s.Close();
                    s = null;

                    rd.Close();
                    rd.Dispose();
                    GC.Collect();

                }
                else if (RptFlag == "4") // This flag is for DailyIEWiseCall Report
                {
                    //string callDate = "02-08-2016";
                    //string CaseNo = "N";
                    string callDate = Request.QueryString["callDate"].ToString();
                    string CaseNo = Request.QueryString["RegNo"].ToString();
                    cristalview.ReportSource = DailyIEWiseCall.IEWiseCall(callDate, CaseNo, out dsCustom);
                }
                else if (RptFlag == "5") // This flag is for BPO Wise Report
                {
                    string Month = Request.QueryString["Month"].ToString();
                    string Year = Request.QueryString["Year"].ToString();
                    string Region = Request.QueryString["Region"].ToString();
                    string ReportType = Request.QueryString["ReportType"].ToString();
                    string FromDate = Request.QueryString["FromDate"].ToString();
                    string ToDate = Request.QueryString["ToDate"].ToString();
                    string Rb1 = Request.QueryString["Rb1"].ToString();
                    string Rb2 = Request.QueryString["Rb2"].ToString();
                    string Rb5 = Request.QueryString["Rb5"].ToString();
                    string lstBpo = Request.QueryString["lstBpo"].ToString();
                    string ClientType = Request.QueryString["ClientType"].ToString();
                    string Rb3 = Request.QueryString["Rb3"].ToString();
                    string Rb4 = Request.QueryString["Rb4"].ToString();
                    cristalview.ReportSource = BPOBills.BPOBill(Month, Year, Region, ReportType, FromDate, ToDate, Rb1, Rb2, Rb5, lstBpo, ClientType, Rb3, Rb4, out dsCustom);
                }
                else if (RptFlag == "6") // This flag is for Client Wise Report
                {
                    //string BPOType = "E11100692";
                    //string AccCD = "2842";
                    //string FromDate = "01/04/2008";
                    //string ToDate = "01/04/2008";
                    //string Region = "N";
                    string BPOType = Request.QueryString["BPOType"].ToString();
                    string AccCD = Request.QueryString["AccCD"].ToString();
                    string FromDate = Request.QueryString["FromDate"].ToString();
                    string ToDate = Request.QueryString["ToDate"].ToString();
                    string Region = Request.QueryString["Region"].ToString();
                    
                    //cristalview.ReportSource = CustomerAdvance.CustomerAdv(BPOType, AccCD, FromDate, ToDate, Region, out dsCustom);
                    ReportDocument rd = CustomerAdvance.CustomerAdv(BPOType, AccCD, FromDate, ToDate, Region, out dsCustom);
                    System.IO.Stream st = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    System.IO.MemoryStream s = new System.IO.MemoryStream();
                    CopyStream(st, s);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format("inline; filename={0}", "ICReport.pdf"));
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(s.ToArray());
                    Response.End();
                    s.Close();
                    s = null;

                    rd.Close();
                    rd.Dispose();
                    GC.Collect();
                }
                else if (RptFlag == "7") // This flag is for IC Accountal Report
                {
                    //string FromDate = "01/04/2008";
                    //string ToDate = "01/04/2022";
                    //string Region = "N";
                    //string lstYesNo = "Y";
                    //string rdbGIE = "true";
                    //string lstIE = "21";
                    //string rdbCancelYes = "true";

                    string FromDate = Request.QueryString["FromDate"].ToString();
                    string ToDate = Request.QueryString["ToDate"].ToString();
                    string Region = Request.QueryString["Region"].ToString();
                    string lstYesNo = Request.QueryString["lstYesNo"].ToString();
                    string rdbGIE = Request.QueryString["rdbGIE"].ToString();
                    string lstIE = Request.QueryString["lstIE"].ToString();
                    string rdbCancelYes = Request.QueryString["rdbCancelYes"].ToString();
                    cristalview.ReportSource = ICAccountal.ICAccount(FromDate, ToDate, Region, lstYesNo, rdbGIE, lstIE, rdbCancelYes, out dsCustom);
                }
                else if (RptFlag == "8") // This flag is for IC Accountal Report
                {
                    //string CaseNO = "N23100003";
                    //string Call_Recv_Dt = "2023-10-17";
                    //string CallSNo = "2";
                    //string Consignee_CD = "";
                    //string Region = "N";

                    string CaseNO = Request.QueryString["CaseNO"].ToString();
                    string Call_Recv_Dt = Request.QueryString["Call_Recv_Dt"].ToString();
                    string CallSNo = Request.QueryString["CallSNo"].ToString();
                    string Consignee_CD = Request.QueryString["Consignee_CD"].ToString();
                    string Region = Request.QueryString["Region"].ToString();
                    string BkNo = Request.QueryString["BkNo"].ToString();
                    string SetNo = Request.QueryString["SetNo"].ToString();

                    ReportDocument rd = IC_RPT.IC_Report(CaseNO, Call_Recv_Dt, CallSNo, Consignee_CD, Region, BkNo, SetNo, out dsCustom);

                    System.IO.Stream st = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    System.IO.MemoryStream s = new System.IO.MemoryStream();
                    CopyStream(st, s);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", String.Format("inline; filename={0}", "ICReport.pdf"));
                    Response.ContentType = "application/pdf";
                    Response.BinaryWrite(s.ToArray());
                    Response.End();
                    s.Close();
                    s = null;

                    rd.Close();
                    rd.Dispose();
                    GC.Collect();
                }
            }
            catch 
            {
            }
        }

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}