using CrystalDecisions.CrystalReports.Engine;
using Net.Codecrete.QrCodeGenerator;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Web;

namespace IBSReports.ReportClass
{
    public static class LabLABInvoice
    {
        private static readonly QrCode.Ecc[] errorCorrectionLevels = { QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High };
        public static OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        static string dateconcate(string dt3)
        {
            string myYear, myMonth;

            myMonth = dt3.Substring(3, 2);
            myYear = dt3.Substring(6, 4);
            string dt4 = myYear + myMonth;
            return (dt4);
        }

        public static ReportDocument LabInvoiceReport(string InvoiceNo, string InvoiceDT, out DataSet dsCustom)
        {
            ReportDocument rd = new ReportDocument();
            dsCustom = new DataSet();
            try
            {
                //conn1.Close();
               
                string str_Sno = "Select max(ITEM_SRNO) from T86_LAB_INVOICE_DETAILS where INVOICE_NO='" + InvoiceNo + "'";
                OracleCommand cmd31 = new OracleCommand(str_Sno, conn1);
                //conn1.Open();
                System.Data.DataSet ds = new DataSet();
                OracleDataAdapter adapter = new OracleDataAdapter(cmd31);
                adapter.Fill(ds);
                int Item_Sno = 0;
                Item_Sno = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                conn1.Close();
                int INV_DT_Check = Convert.ToInt32(dateconcate(InvoiceDT));
                


               
                if (Item_Sno > 3)
                {
                    rd.Load(HttpContext.Current.Server.MapPath("~/Reports/LAB_INVOICE_GEN_NEW.rpt"));
                    dsCustom = funlabbill(InvoiceNo);
                }
                else if (INV_DT_Check >= 202207)
                {
                    rd.Load(HttpContext.Current.Server.MapPath("~/Reports/LAB_INVOICE_GEN_HR.rpt"));
                    dsCustom = funlabbill(InvoiceNo);
                }
                else
                {
                    rd.Load(HttpContext.Current.Server.MapPath("~/Reports/LAB_INVOICE_GEN.rpt"));
                    dsCustom = funlabbill(InvoiceNo);
                }

                rd.SetDataSource(dsCustom);
            }
            catch 
            {

            }
            return rd;
        }

        private static DataSet funlabbill(string InvoiceNo)
        {
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
            DataSet dsReports = new DataSet();
            string sql = "select t55.INVOICE_NO INVOICE_NO,to_char(t55.INVOICE_DT, 'dd/mm/yyyy') INVOICE_DT, t55.CASE_NO CASE_NO,t55.BPO_CD BPO_CD,t55.RECIPIENT_GSTIN_NO RECIPIENT_GSTIN_NO," +
                         "t55.BILL_AMOUNT BILL_AMOUNT,t55.TOTAL_CGST TOTAL_CGST,t55.TOTAL_SGST TOTAL_SGST," +
                         "t55.TOTAL_IGST TOTAL_IGST,t86.ITEM_SRNO ITEM_SRNO,t86.ITEM_DESC ITEM_DESC,t86.QTY QTY," +
                         "t86.RATE RATE,t86.TESTING_CHARGES TESTING_CHARGES,t86.CGST CGST,t86.SGST SGST,t86.IGST IGST," +
                         "B.BPO_NAME BPO_NAME,(NVL2(B.BPO_ADD,B.BPO_ADD||'/','')||NVL2(C.LOCATION,C.CITY||','||C.LOCATION,C.CITY)) BPO_Address," +
                         "NVL(B.BPO_STATE,'') BPO_STATE, t55.transaction_no transaction_no, t55.customer_ref_no customer_ref_no, t55.Region_code Region_code, t09.ie_name ie_name,t55.QR_CODE,t55.IRN_NO,t55.SAMPLE_REG_NO,t55.ACK_NO,t55.ACK_DT " +
                         "from t55_lab_invoice t55, t86_Lab_Invoice_Details t86,T12_BILL_PAYING_OFFICER B, T03_CITY C, T09_IE t09 " +
                         "where t55.invoice_no= t86.invoice_no and B.BPO_CITY_CD= C.CITY_CD and t55.bpo_cd= b.bpo_cd and t55.ie_cd=t09.ie_cd(+) " +
                         "and t55.Invoice_No='" + InvoiceNo + "'";
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn;
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            OracleDataReader readerB = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("INVOICE_NO", typeof(string));
            dt.Columns.Add("INVOICE_DT", typeof(string));
            dt.Columns.Add("CASE_NO", typeof(string));
            dt.Columns.Add("BPO_CD", typeof(string));
            dt.Columns.Add("RECIPIENT_GSTIN_NO", typeof(string));
            dt.Columns.Add("BILL_AMOUNT", typeof(string));
            dt.Columns.Add("TOTAL_CGST", typeof(string));
            dt.Columns.Add("TOTAL_SGST", typeof(string));
            dt.Columns.Add("TOTAL_IGST", typeof(string));
            dt.Columns.Add("ITEM_SRNO", typeof(string));
            dt.Columns.Add("ITEM_DESC", typeof(string));
            dt.Columns.Add("QTY", typeof(string));
            dt.Columns.Add("RATE", typeof(string));
            dt.Columns.Add("TESTING_CHARGES", typeof(string));
            dt.Columns.Add("CGST", typeof(string));
            dt.Columns.Add("SGST", typeof(string));
            dt.Columns.Add("IGST", typeof(string));
            dt.Columns.Add("BPO_NAME", typeof(string));
            dt.Columns.Add("BPO_Address", typeof(string));
            dt.Columns.Add("BPO_STATE", typeof(string));
            dt.Columns.Add("transaction_no", typeof(string));
            dt.Columns.Add("customer_ref_no", typeof(string));
            dt.Columns.Add("Region_code", typeof(string));
            dt.Columns.Add("ie_name", typeof(string));
            dt.Columns.Add("QR_CODE", typeof(byte[]));
            dt.Columns.Add("IRN_NO", typeof(string));
            dt.Columns.Add("SAMPLE_REG_NO", typeof(string));
            dt.Columns.Add("ACK_NO", typeof(string));
            dt.Columns.Add("ACK_DT", typeof(string));
            //dt.Columns.Add("QR_CODENEw", typeof(byte));

            dt.TableName = "Command";
            while (readerB.Read())
            {
                DataRow dr = dt.NewRow();// dsReports.Tables["Command"].Rows.Add();
                dr["INVOICE_NO"] = Convert.ToString(readerB["INVOICE_NO"]);
                dr["INVOICE_DT"] = Convert.ToString(readerB["INVOICE_DT"]);
                dr["CASE_NO"] = Convert.ToString(readerB["CASE_NO"]);
                dr["BPO_CD"] = Convert.ToString(readerB["BPO_CD"]);
                dr["RECIPIENT_GSTIN_NO"] = Convert.ToString(readerB["RECIPIENT_GSTIN_NO"]);
                dr["BILL_AMOUNT"] = Convert.ToString(readerB["BILL_AMOUNT"]);
                dr["TOTAL_CGST"] = Convert.ToString(readerB["TOTAL_CGST"]);
                dr["TOTAL_SGST"] = Convert.ToString(readerB["TOTAL_SGST"]);
                dr["TOTAL_IGST"] = Convert.ToString(readerB["TOTAL_IGST"]);
                dr["ITEM_SRNO"] = Convert.ToString(readerB["ITEM_SRNO"]);
                dr["ITEM_DESC"] = Convert.ToString(readerB["ITEM_DESC"]);
                dr["QTY"] = Convert.ToString(readerB["QTY"]);
                dr["RATE"] = Convert.ToString(readerB["RATE"]);
                dr["TESTING_CHARGES"] = Convert.ToString(readerB["TESTING_CHARGES"]);
                dr["CGST"] = Convert.ToString(readerB["CGST"]);
                dr["SGST"] = Convert.ToString(readerB["SGST"]);
                dr["IGST"] = Convert.ToString(readerB["IGST"]);
                dr["BPO_NAME"] = Convert.ToString(readerB["BPO_NAME"]);
                dr["BPO_Address"] = Convert.ToString(readerB["BPO_Address"]);
                dr["BPO_STATE"] = Convert.ToString(readerB["BPO_STATE"]);
                dr["transaction_no"] = Convert.ToString(readerB["transaction_no"]);
                dr["customer_ref_no"] = Convert.ToString(readerB["customer_ref_no"]);
                dr["Region_code"] = Convert.ToString(readerB["Region_code"]);
                dr["ie_name"] = Convert.ToString(readerB["ie_name"]);
                dr["QR_CODE"] = QRHelper.GeneratePng(readerB["QR_CODE"].ToString());
                dr["IRN_NO"] = Convert.ToString(readerB["IRN_NO"]);
                dr["SAMPLE_REG_NO"] = Convert.ToString(readerB["SAMPLE_REG_NO"]);
                dr["ACK_NO"] = Convert.ToString(readerB["ACK_NO"]);
                dr["ACK_DT"] = Convert.ToString(readerB["ACK_DT"]);
                dt.Rows.Add(dr);
            }
            dsReports.Tables.Add(dt);

            return dsReports;

        }

    }
}