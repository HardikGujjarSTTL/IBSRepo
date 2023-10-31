using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Net.Codecrete.QrCodeGenerator;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;

namespace IBSReports.ReportClass
{
    public class IC_RPT : Page
    {
        private static readonly QrCode.Ecc[] errorCorrectionLevels = { QrCode.Ecc.Low, QrCode.Ecc.Medium, QrCode.Ecc.Quartile, QrCode.Ecc.High };
        public static OracleConnection conn1 = new OracleConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);

        public static ReportDocument IC_Report(string CaseNO, string Call_Recv_Dt, string CallSNo, string Consignee_CD, string Region, string BkNo, string SetNo, out DataSet dsCustom)
        {
            ReportDocument rd = new ReportDocument();
            dsCustom = new DataSet();
            try
            {
                string str1 = "SELECT man_type FROM IC_INTERMEDIATE WHERE CONSIGNEE_CD='" + Convert.ToString(Consignee_CD) + "' AND CASE_NO='" + CaseNO.Trim() + "' AND CALL_RECV_DT= TO_date('" + Call_Recv_Dt + "', 'dd/mm/yyyy') AND CALL_SNO='" + CallSNo + "' ORDER BY DATETIME DESC";
                OracleCommand cmd31 = new OracleCommand(str1, conn1);
                DataSet ds = new DataSet();
                OracleDataAdapter adapter = new OracleDataAdapter(cmd31);
                adapter.Fill(ds);
                string Manu_Type = "";
                Manu_Type = Convert.ToString(ds.Tables[0].Rows[0][0]);
                conn1.Close();
                string caseNo = CaseNO.Trim();
                string callSNo = CallSNo;
                string myYear, myMonth, myDay;
                myYear = Call_Recv_Dt.Substring(6, 4);
                myMonth = Call_Recv_Dt.Substring(3, 2);
                myDay = Call_Recv_Dt.Substring(0, 2);
                string recvDtRpt = myMonth + "/" + myDay + "/" + myYear;

                if (Region == "C")
                {
                    if (Manu_Type == "B")
                    {
                        rd.Load(HttpContext.Current.Server.MapPath("~/Reports/InspectionCertificateNew_CR.rpt"));
                        dsCustom = funInspectionCertificate_cr(caseNo, callSNo, recvDtRpt, Consignee_CD, Region);
                    }
                    else if (Manu_Type == "J")
                    {
                        rd.Load(HttpContext.Current.Server.MapPath("~/Reports/InspectionCertificateNew_CR_R.rpt"));
                        dsCustom = funInspectionCertificate_cr_r(caseNo, callSNo, recvDtRpt, Consignee_CD, Region);
                    }
                    else if (Manu_Type == "O")
                    {
                        rd.Load(HttpContext.Current.Server.MapPath("~/Reports/InspectionCertificateNew.rpt"));
                        dsCustom = funInspectionCertificate(caseNo, callSNo, recvDtRpt, Consignee_CD, Region);
                    }
                }
                else
                {
                    rd.Load(HttpContext.Current.Server.MapPath("~/Reports/InspectionCertificateNew.rpt"));
                    dsCustom = funInspectionCertificate(caseNo, callSNo, recvDtRpt, Consignee_CD, Region);
                }

                rd.SetDataSource(dsCustom);
                rd.ExportToDisk(ExportFormatType.PortableDocFormat, "ICReport.pdf");

                //ExportOptions exportOptions = new ExportOptions();
                //ExcelFormatOptions excelFormatOptions = new ExcelFormatOptions();
                //exportOptions.ExportFormatType = ExportFormatType.Excel;
                //exportOptions.ExportFormatOptions = excelFormatOptions;

                //// Create a MemoryStream and export the report to it

                //Stream oStream = rd.ExportToStream(ExportFormatType.Excel);
                //byte[] data = new byte[oStream.Length];
                //oStream.Read(data, 0, (int)oStream.Length);
                //oStream.Close();

                //// Convert the byte array to a base64 string
                //string file = Convert.ToBase64String(data);
                //XmlDocument doc = new XmlDocument();
                //XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                //doc.AppendChild(docNode);

                //XmlNode requestNode = doc.CreateElement("request");
                //doc.AppendChild(requestNode);

                //XmlNode commandNode = doc.CreateElement("command");
                //commandNode.AppendChild(doc.CreateTextNode("pkiNetworkSign"));
                //requestNode.AppendChild(commandNode);

                //XmlNode tsNode = doc.CreateElement("ts");
                //string tym = DateTime.Now.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                //tsNode.AppendChild(doc.CreateTextNode(tym));
                //requestNode.AppendChild(tsNode);
                //Random random = new Random();
                //string otp = Convert.ToString(random.Next(1000, 9999));

                //XmlNode txnNode = doc.CreateElement("txn");
                //txnNode.AppendChild(doc.CreateTextNode(otp));
                //requestNode.AppendChild(txnNode);

                //XmlNode certNode = doc.CreateElement("certificate");
                //requestNode.AppendChild(certNode);

                //XmlNode nameNode1 = doc.CreateElement("attribute");
                //XmlAttribute nameNode1Attr = doc.CreateAttribute("name");
                //nameNode1Attr.Value = "CN";
                //nameNode1.Attributes.Append(nameNode1Attr);
                //certNode.AppendChild(nameNode1);

                //XmlNode nameNode2 = doc.CreateElement("attribute");
                //XmlAttribute nameNode2Attr = doc.CreateAttribute("name");
                //nameNode2Attr.Value = "O";
                //nameNode2.Attributes.Append(nameNode2Attr);
                //certNode.AppendChild(nameNode2);

                //XmlNode nameNode3 = doc.CreateElement("attribute");
                //XmlAttribute nameNode3Attr = doc.CreateAttribute("name");
                //nameNode3Attr.Value = "OU";
                //nameNode3.Attributes.Append(nameNode3Attr);
                //certNode.AppendChild(nameNode3);

                //XmlNode nameNode4 = doc.CreateElement("attribute");
                //XmlAttribute nameNode4Attr = doc.CreateAttribute("name");
                //nameNode4Attr.Value = "T";
                //nameNode4.Attributes.Append(nameNode4Attr);
                //certNode.AppendChild(nameNode4);

                //XmlNode nameNode5 = doc.CreateElement("attribute");
                //XmlAttribute nameNode5Attr = doc.CreateAttribute("name");
                //nameNode5Attr.Value = "E";
                //nameNode5.Attributes.Append(nameNode5Attr);
                //certNode.AppendChild(nameNode5);

                //XmlNode nameNode6 = doc.CreateElement("attribute");
                //XmlAttribute nameNode6Attr = doc.CreateAttribute("name");
                //nameNode6Attr.Value = "SN";
                //nameNode6.Attributes.Append(nameNode6Attr);
                //certNode.AppendChild(nameNode6);

                //XmlNode nameNode7 = doc.CreateElement("attribute");
                //XmlAttribute nameNode7Attr = doc.CreateAttribute("name");
                //nameNode7Attr.Value = "CA";
                //nameNode7.Attributes.Append(nameNode7Attr);
                //certNode.AppendChild(nameNode7);

                //XmlNode nameNode8 = doc.CreateElement("attribute");
                //XmlAttribute nameNode8Attr = doc.CreateAttribute("name");
                //nameNode8Attr.Value = "TC";
                //nameNode8.Attributes.Append(nameNode8Attr);
                //nameNode8.AppendChild(doc.CreateTextNode("SG"));
                //certNode.AppendChild(nameNode8);

                //XmlNode nameNode9 = doc.CreateElement("attribute");
                //XmlAttribute nameNode9Attr = doc.CreateAttribute("name");
                //nameNode9Attr.Value = "AP";
                //nameNode9.Attributes.Append(nameNode9Attr);
                //nameNode9.AppendChild(doc.CreateTextNode("1"));
                //certNode.AppendChild(nameNode9);

                //XmlNode nameNode10 = doc.CreateElement("attribute");
                //XmlAttribute nameNode10Attr = doc.CreateAttribute("name");
                //nameNode10Attr.Value = "VD";
                //nameNode10.Attributes.Append(nameNode10Attr);
                //certNode.AppendChild(nameNode10);

                //XmlNode fileNode = doc.CreateElement("file");
                //requestNode.AppendChild(fileNode);

                //XmlNode nameNode11 = doc.CreateElement("attribute");
                //XmlAttribute nameNode11Attr = doc.CreateAttribute("name");
                //nameNode11Attr.Value = "type";
                //nameNode11.Attributes.Append(nameNode11Attr);
                //nameNode11.AppendChild(doc.CreateTextNode("pdf"));
                //fileNode.AppendChild(nameNode11);

                //XmlNode pdfNode = doc.CreateElement("pdf");
                //requestNode.AppendChild(pdfNode);

                //XmlNode pageNode = doc.CreateElement("page");
                //pageNode.AppendChild(doc.CreateTextNode("1"));
                //pdfNode.AppendChild(pageNode);

                //XmlNode coodNode = doc.CreateElement("cood");
                //if (Region == "C")
                //{
                //    coodNode.AppendChild(doc.CreateTextNode("450,40"));
                //}
                //else
                //{
                //    coodNode.AppendChild(doc.CreateTextNode("400,45"));
                //}
                //pdfNode.AppendChild(coodNode);

                //XmlNode sizeNode = doc.CreateElement("size");
                //if (Region == "C")
                //{
                //    sizeNode.AppendChild(doc.CreateTextNode("120,60"));
                //}
                //else
                //{
                //    sizeNode.AppendChild(doc.CreateTextNode("165,60"));
                //}

                //pdfNode.AppendChild(sizeNode);

                //XmlNode dataNode = doc.CreateElement("data");
                //dataNode.AppendChild(doc.CreateTextNode(file));
                //requestNode.AppendChild(dataNode);

                //StringWriter sw = new StringWriter();
                //XmlTextWriter tx = new XmlTextWriter(sw);
                //doc.WriteTo(tx);

                //string aa = sw.ToString();

                //string fname = callSNo.Trim() + "-" + BkNo.Trim() + "-" + SetNo.Trim();
                //doc.Save(HttpContext.Current.Server.MapPath("IC_XML/" + fname + ".xml"));
                //RegisterStartupScript("abc", "<script language=JavaScript> abc('" + aa + "','" + fname + "'); </script>");
            }
            catch
            {

            }
            
            return rd;
        }

        private static DataSet funInspectionCertificate_cr(string pcaseno_cr, string pcsno_cr, string recvdt_cr, string pconsidecd_cr, string pregion_cr)
        {
            DataSet dsReports = new DataSet();
            string sql_cr = "SELECT     CLR.CASE_NO, CLR.Call_SNO, to_date(TO_CHAR(CLR.Call_Recv_dt, 'mm/dd/yyyy'), 'mm/dd/yyyy')Call_Recv_dt, POM.Region_Code, BOF.BPO_RLY as RLY_CD, CLR.Call_Install_No, TIE.IE_Sname, VND.Vend_Name, VND.Vend_Add1, NVL(VND.Vend_Add2, ' ') AS Vend_Add2,VCT.City AS Vend_City, MFG.Vend_Name AS MFG_Name, MFG.Vend_Add1 AS MFG_Add1,NVL(MFG.Vend_Add2, ' ') AS MFG_Add2, NVL( MCT.City,' ') AS MFG_City, CLR.MFG_PLACE, POM.PO_NO, TO_CHAR(POM.PO_DT,'dd/mm/yyyy') PO_DT, NVL(CON.CONSIGNEE_DESIG, ' ')  AS CONSIGNEE_DESIG, " +
                " CON.CONSIGNEE_CD, CCT.City AS CONSIGNEE_CITYNAME, NVL(CON.CONSIGNEE_DEPT, ' ') AS CONSIGNEE_DEPT, NVL(CON.CONSIGNEE_FIRM, ' ') AS CONSIGNEE_FIRM, NVL(PUR.CONSIGNEE_DESIG, ' ') AS PUR_DESIG, PUR.CONSIGNEE_CD AS PUR_CD,  NVL(PUR.CONSIGNEE_DEPT, ' ') AS PUR_DEPT, NVL(PUR.CONSIGNEE_FIRM, ' ') AS PUR_FIRM, NVL(PCT.City, ' ') AS PUR_City, CDT.ITEM_SRNO_PO, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.ITEM_DESC_PO ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN  " +
                " CDT.ITEM_DESC_PO ELSE CDT.ITEM_DESC_PO END END AS ITEM_DESC_PO, UOM.UOM_S_DESC, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_ORDERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_ORDERED,0) ELSE NVL(CDT.QTY_ORDERED,0) END END AS QTY_ORDERED, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_OFFERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_OFFERED,0)  ELSE NVL(CDT.CUM_QTY_PREV_OFFERED,0) END END AS CUM_QTY_PREV_OFFERED,  " +
                " CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_PASSED,0) ELSE NVL(CDT.CUM_QTY_PREV_PASSED,0) END END AS CUM_QTY_PREV_PASSED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_TO_INSP,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_TO_INSP,0) ELSE NVL(CDT.QTY_TO_INSP,0) END END AS QTY_TO_INSP, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' " +
                " THEN NVL(CDT.QTY_PASSED,0) ELSE NVL(CDT.QTY_PASSED,0) END END AS QTY_PASSED,   CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_REJECTED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_REJECTED,0) ELSE NVL(CDT.QTY_REJECTED,0) END END AS QTY_REJECTED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_DUE,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_DUE,0) ELSE NVL(CDT.QTY_DUE,0)END END AS QTY_DUE, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.HOLOGRAM ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN ICI.HOLOGRAM ELSE ICI.HOLOGRAM " +
                " END END AS HOLOGRAM, ICI.NUM_VISITS, PRM.VISIT_DATES, BOF.BPO_NAME, BOF.BPO_ORGN, BCT.City, NVL(CLR.HOLOGRAM, '    ')  AS HOLOGRAMORG, NVL(ICI.REMARK, ' ') AS REMARK, CLR.DT_INSP_DESIRE, NVL(ICI.ITEM_REMARK, 'NO REMARK') AS ITEM_REMARK, substr(ICI.AMENDMENT_1, 0, 99) AS AMENDMENT_1, substr(ICI.AMENDMENT_1, 100, 20) AS AMENDMENTDT_1, substr(ICI.AMENDMENT_2, 0, 99) AS AMENDMENT_2, substr(ICI.AMENDMENT_2, 100, 20) AS AMENDMENTDT_2, substr(ICI.AMENDMENT_3, 0, 99) AS AMENDMENT_3, substr(ICI.AMENDMENT_3, 100, 20) AS AMENDMENTDT_3, substr(ICI.AMENDMENT_4, 0, 99) AS AMENDMENT_4, substr(ICI.AMENDMENT_4, 100, 20) " +
                " AS AMENDMENTDT_4, ICI.BK_NO, ICI.SET_NO, ICI.VISITS_DATES, ICI.IE_STAMP_IMAGE, ICI.IE_STAMP_IMAGE1, TO_CHAR(ICI.LAB_TST_RECT_DT,'dd/mm/yyyy') LAB_TST_RECT_DT,ICI.PASSED_INST_NO, NVL(CONSIGNEE_DTL, '') AS CONSIGNEE_DTL, NVL(BPO_DTL, '') AS BPO_DTL,NVL(GOV_BILL_AUTH, '') AS GOV_BILL_AUTH, NVL(PUR_DTL, '') AS PUR_DTL, NVL(PUR_AUT_DTL, '') AS PUR_AUT_DTL, NVL(OFF_INST_NO_DTL, '') AS OFF_INST_NO_DTL, NVL(UNIT_DTL, '') AS UNIT_DTL, NVL(DISPATCH_PACKING_NO, '') As DISPATCH_PACKING_NO, NVL(INVOICE_NO, '') As INVOICE_NO, NVL(NAME_OF_IE, '') As NAME_OF_IE, NVL(MAN_TYPE, '') As MAN_TYPE, NVL(CONSIGNEE_DESG, '') As CONSIGNEE_DESG,TO_CHAR(ICI.DATETIME,'dd/mm/yyyy') DATETIME,ICI.CONSGN_CALL_STATUS FROM RPT_PRM_Inspection_Certificate PRM INNER JOIN  T17_Call_Register CLR ON CLR.CASE_NO = PRM.CASE_NO AND CLR.Call_Recv_dt = PRM.CALL_RECV_DT AND " +
                " CLR.Call_SNO = PRM.Call_SNO INNER JOIN T18_Call_Details CDT ON CLR.CASE_NO = CDT.CASE_NO AND CLR.Call_Recv_dt = CDT.Call_Recv_dt AND CLR.Call_SNO = CDT.Call_SNO INNER JOIN T13_PO_Master POM ON CLR.CASE_NO = POM.CASE_NO LEFT OUTER JOIN T15_PO_DETAIL POD ON CLR.CASE_NO = POD.CASE_NO AND CDT.ITEM_SRNO_PO = POD.ITEM_SRNO LEFT OUTER JOIN T04_UOM UOM ON POD.UOM_CD = UOM.UOM_CD LEFT OUTER JOIN " +
                " T09_IE TIE ON CLR.IE_CD = TIE.IE_CD LEFT OUTER JOIN T05_Vendor VND ON POM.Vend_Cd = VND.Vend_Cd LEFT OUTER JOIN   T03_City VCT ON VND.Vend_City_Cd = VCT.City_Cd LEFT OUTER JOIN T05_Vendor MFG ON CLR.MFG_CD = MFG.Vend_Cd LEFT OUTER JOIN  T03_City MCT ON MFG.Vend_City_Cd = MCT.City_Cd LEFT OUTER JOIN T14_PO_BPO BPO ON CLR.CASE_NO = BPO.CASE_NO AND CDT.CONSIGNEE_CD = BPO.CONSIGNEE_CD LEFT OUTER  " +
                " JOIN T12_BILL_PAYING_OFFICER BOF ON BPO.BPO_CD = BOF.BPO_CD LEFT OUTER JOIN  T03_City BCT ON BOF.BPO_City_Cd = BCT.City_Cd LEFT OUTER JOIN  T06_Consignee CON ON BPO.CONSIGNEE_CD = CON.CONSIGNEE_CD LEFT OUTER JOIN T03_City CCT ON CON.CONSIGNEE_CITY = CCT.City_Cd LEFT OUTER JOIN  T06_Consignee PUR ON POM.PURCHASER_CD = PUR.CONSIGNEE_CD LEFT OUTER JOIN T03_City PCT ON PUR.CONSIGNEE_CITY = PCT.City_Cd INNER JOIN " +
                " IC_INTERMEDIATE ICI ON ICI.CASE_NO = CDT.CASE_NO AND ICI.Call_Recv_dt = CDT.Call_Recv_dt AND ICI.Call_SNO = CDT.Call_SNO AND CDT.CONSIGNEE_CD = ICI.CONSIGNEE_CD AND CDT.ITEM_SRNO_PO = ICI.ITEM_SRNO_PO AND PRM.CONSIGNEE_CD = ICI.CONSIGNEE_CD WHERE ICI.ITEM_SRNO_PO IS NOT NULL and ICI.CASE_NO='" + pcaseno_cr + "' " +
                " and CLR.Call_SNO=" + pcsno_cr + " and CLR.Call_Recv_dt=to_date('" + recvdt_cr + "','mm/dd/yyyy')  and CON.CONSIGNEE_CD='" + pconsidecd_cr + "' ORDER BY CON.CONSIGNEE_CD, POD.ITEM_SRNO";
            OracleCommand cmd_cr = new OracleCommand();
            cmd_cr.CommandText = sql_cr;
            cmd_cr.Connection = conn1;
            if (conn1.State == ConnectionState.Closed)
                conn1.Open();
            OracleDataReader readerB_cr = cmd_cr.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CASE_NO", typeof(string));
            dt.Columns.Add("Call_SNO", typeof(string));
            dt.Columns.Add("Call_Recv_dt", typeof(string));
            dt.Columns.Add("Region_Code", typeof(string));
            dt.Columns.Add("RLY_CD", typeof(string));
            dt.Columns.Add("Call_Install_No", typeof(string));
            dt.Columns.Add("IE_Sname", typeof(string));
            dt.Columns.Add("Vend_Name", typeof(string));
            dt.Columns.Add("Vend_Add1", typeof(string));
            dt.Columns.Add("Vend_Add2", typeof(string));
            dt.Columns.Add("Vend_City", typeof(string));
            dt.Columns.Add("MFG_Name", typeof(string));
            dt.Columns.Add("MFG_Add1", typeof(string));
            dt.Columns.Add("MFG_Add2", typeof(string));
            dt.Columns.Add("MFG_City", typeof(string));
            dt.Columns.Add("MFG_PLACE", typeof(string));
            dt.Columns.Add("PO_NO", typeof(string));
            dt.Columns.Add("PO_DT", typeof(string));
            dt.Columns.Add("CONSIGNEE_DESIG", typeof(string));
            dt.Columns.Add("CONSIGNEE_CD", typeof(Int32));
            dt.Columns.Add("CONSIGNEE_CITYNAME", typeof(string));
            dt.Columns.Add("CONSIGNEE_DEPT", typeof(string));
            dt.Columns.Add("CONSIGNEE_FIRM", typeof(string));
            dt.Columns.Add("PUR_DESIG", typeof(string));
            dt.Columns.Add("PUR_CD", typeof(string));
            dt.Columns.Add("PUR_DEPT", typeof(string));
            dt.Columns.Add("PUR_FIRM", typeof(string));
            dt.Columns.Add("PUR_City", typeof(string));
            dt.Columns.Add("ITEM_SRNO_PO", typeof(string));
            dt.Columns.Add("ITEM_DESC_PO", typeof(string));
            dt.Columns.Add("UOM_S_DESC", typeof(string));
            dt.Columns.Add("QTY_ORDERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_OFFERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_PASSED", typeof(double));
            dt.Columns.Add("QTY_TO_INSP", typeof(double));
            dt.Columns.Add("QTY_PASSED", typeof(double));
            dt.Columns.Add("QTY_REJECTED", typeof(double));
            dt.Columns.Add("QTY_DUE", typeof(double));
            dt.Columns.Add("HOLOGRAM", typeof(string));
            dt.Columns.Add("NUM_VISITS", typeof(Int32));
            dt.Columns.Add("VISIT_DATES", typeof(string));
            dt.Columns.Add("BPO_NAME", typeof(string));
            dt.Columns.Add("BPO_ORGN", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("HOLOGRAMORG", typeof(string));
            dt.Columns.Add("REMARK", typeof(string));
            dt.Columns.Add("DT_INSP_DESIRE", typeof(string));
            dt.Columns.Add("ITEM_REMARK", typeof(string));
            dt.Columns.Add("AMENDMENT_1", typeof(string));
            dt.Columns.Add("AMENDMENTDT_1", typeof(string));
            dt.Columns.Add("AMENDMENT_2", typeof(string));
            dt.Columns.Add("AMENDMENTDT_2", typeof(string));
            dt.Columns.Add("AMENDMENT_3", typeof(string));
            dt.Columns.Add("AMENDMENTDT_3", typeof(string));
            dt.Columns.Add("AMENDMENT_4", typeof(string));
            dt.Columns.Add("AMENDMENTDT_4", typeof(string));
            dt.Columns.Add("BK_NO", typeof(string));
            dt.Columns.Add("SET_NO", typeof(string));
            dt.Columns.Add("VISITS_DATES", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE1", typeof(string));
            dt.Columns.Add("LAB_TST_RECT_DT", typeof(string));
            dt.Columns.Add("PASSED_INST_NO", typeof(string));
            dt.Columns.Add("CONSIGNEE_DTL", typeof(string));
            dt.Columns.Add("BPO_DTL", typeof(string));
            dt.Columns.Add("GOV_BILL_AUTH", typeof(string));
            dt.Columns.Add("PUR_DTL", typeof(string));
            dt.Columns.Add("PUR_AUT_DTL", typeof(string));
            dt.Columns.Add("OFF_INST_NO_DTL", typeof(string));
            dt.Columns.Add("UNIT_DTL", typeof(string));
            dt.Columns.Add("DISPATCH_PACKING_NO", typeof(string));
            dt.Columns.Add("INVOICE_NO", typeof(string));
            dt.Columns.Add("NAME_OF_IE", typeof(string));
            dt.Columns.Add("MAN_TYPE", typeof(string));
            dt.Columns.Add("CONSIGNEE_DESG", typeof(string));
            dt.Columns.Add("DATETIME", typeof(string));
            dt.Columns.Add("CONSGN_CALL_STATUS", typeof(string));
            dt.Columns.Add("pRegion", typeof(string));
            dt.TableName = "Command";
            while (readerB_cr.Read())
            {
                DataRow dr_cr = dt.NewRow();// dsReports.Tables["Command"].Rows.Add();
                dr_cr["CASE_NO"] = Convert.ToString(readerB_cr["CASE_NO"]);
                dr_cr["Call_SNO"] = Convert.ToString(readerB_cr["Call_SNO"]);
                dr_cr["Call_Recv_dt"] = Convert.ToString(readerB_cr["Call_Recv_dt"]);
                dr_cr["Region_Code"] = Convert.ToString(readerB_cr["Region_Code"]);
                dr_cr["RLY_CD"] = Convert.ToString(readerB_cr["RLY_CD"]);
                dr_cr["Call_Install_No"] = Convert.ToString(readerB_cr["Call_Install_No"]);
                dr_cr["IE_Sname"] = Convert.ToString(readerB_cr["IE_Sname"]);
                dr_cr["Vend_Name"] = Convert.ToString(readerB_cr["Vend_Name"]);
                dr_cr["Vend_Add1"] = Convert.ToString(readerB_cr["Vend_Add1"]);
                dr_cr["Vend_Add2"] = Convert.ToString(readerB_cr["Vend_Add2"]);
                dr_cr["Vend_City"] = Convert.ToString(readerB_cr["Vend_City"]);
                dr_cr["MFG_Name"] = Convert.ToString(readerB_cr["MFG_Name"]);
                dr_cr["MFG_Add1"] = Convert.ToString(readerB_cr["MFG_Add1"]);
                dr_cr["MFG_Add2"] = Convert.ToString(readerB_cr["MFG_Add2"]);
                dr_cr["MFG_City"] = Convert.ToString(readerB_cr["MFG_City"]);
                dr_cr["MFG_PLACE"] = Convert.ToString(readerB_cr["MFG_PLACE"]);
                dr_cr["PO_NO"] = Convert.ToString(readerB_cr["PO_NO"]);
                dr_cr["PO_DT"] = Convert.ToString(readerB_cr["PO_DT"]);
                dr_cr["CONSIGNEE_DESIG"] = Convert.ToString(readerB_cr["CONSIGNEE_DESIG"]);
                dr_cr["CONSIGNEE_CD"] = Convert.ToString(readerB_cr["CONSIGNEE_CD"]);
                dr_cr["CONSIGNEE_CITYNAME"] = Convert.ToString(readerB_cr["CONSIGNEE_CITYNAME"]);
                dr_cr["CONSIGNEE_DEPT"] = Convert.ToString(readerB_cr["CONSIGNEE_DEPT"]);
                dr_cr["CONSIGNEE_FIRM"] = Convert.ToString(readerB_cr["CONSIGNEE_FIRM"]);
                dr_cr["PUR_DESIG"] = Convert.ToString(readerB_cr["PUR_DESIG"]);
                dr_cr["PUR_CD"] = Convert.ToString(readerB_cr["PUR_CD"]);
                dr_cr["PUR_DEPT"] = Convert.ToString(readerB_cr["PUR_DEPT"]);
                dr_cr["PUR_FIRM"] = Convert.ToString(readerB_cr["PUR_FIRM"]);
                dr_cr["PUR_City"] = Convert.ToString(readerB_cr["PUR_City"]);
                dr_cr["ITEM_SRNO_PO"] = Convert.ToString(readerB_cr["ITEM_SRNO_PO"]);
                dr_cr["ITEM_DESC_PO"] = Convert.ToString(readerB_cr["ITEM_DESC_PO"]);
                dr_cr["UOM_S_DESC"] = Convert.ToString(readerB_cr["UOM_S_DESC"]);
                dr_cr["QTY_ORDERED"] = Convert.ToString(readerB_cr["QTY_ORDERED"]);
                dr_cr["CUM_QTY_PREV_OFFERED"] = Convert.ToString(readerB_cr["CUM_QTY_PREV_OFFERED"]);
                dr_cr["CUM_QTY_PREV_PASSED"] = Convert.ToString(readerB_cr["CUM_QTY_PREV_PASSED"]);
                dr_cr["QTY_TO_INSP"] = Convert.ToString(readerB_cr["QTY_TO_INSP"]);
                dr_cr["QTY_PASSED"] = Convert.ToString(readerB_cr["QTY_PASSED"]);
                dr_cr["QTY_REJECTED"] = Convert.ToString(readerB_cr["QTY_REJECTED"]);
                dr_cr["QTY_DUE"] = Convert.ToString(readerB_cr["QTY_DUE"]);
                dr_cr["HOLOGRAM"] = Convert.ToString(readerB_cr["HOLOGRAM"]);
                dr_cr["NUM_VISITS"] = Convert.ToString(readerB_cr["NUM_VISITS"]);
                dr_cr["VISIT_DATES"] = Convert.ToString(readerB_cr["VISIT_DATES"]);
                dr_cr["BPO_NAME"] = Convert.ToString(readerB_cr["BPO_NAME"]);
                dr_cr["BPO_ORGN"] = Convert.ToString(readerB_cr["BPO_ORGN"]);
                dr_cr["City"] = Convert.ToString(readerB_cr["City"]);
                dr_cr["HOLOGRAMORG"] = Convert.ToString(readerB_cr["HOLOGRAMORG"]);
                dr_cr["REMARK"] = Convert.ToString(readerB_cr["REMARK"]);
                dr_cr["DT_INSP_DESIRE"] = Convert.ToString(readerB_cr["DT_INSP_DESIRE"]);
                dr_cr["ITEM_REMARK"] = Convert.ToString(readerB_cr["ITEM_REMARK"]);
                dr_cr["AMENDMENT_1"] = Convert.ToString(readerB_cr["AMENDMENT_1"]);
                dr_cr["AMENDMENTDT_1"] = Convert.ToString(readerB_cr["AMENDMENTDT_1"]);
                dr_cr["AMENDMENT_2"] = Convert.ToString(readerB_cr["AMENDMENT_2"]);
                dr_cr["AMENDMENTDT_2"] = Convert.ToString(readerB_cr["AMENDMENTDT_2"]);
                dr_cr["AMENDMENT_3"] = Convert.ToString(readerB_cr["AMENDMENT_3"]);
                dr_cr["AMENDMENTDT_3"] = Convert.ToString(readerB_cr["AMENDMENTDT_3"]);
                dr_cr["AMENDMENT_4"] = Convert.ToString(readerB_cr["AMENDMENT_4"]);
                dr_cr["AMENDMENTDT_4"] = Convert.ToString(readerB_cr["AMENDMENTDT_4"]);
                dr_cr["BK_NO"] = Convert.ToString(readerB_cr["BK_NO"]);
                dr_cr["SET_NO"] = Convert.ToString(readerB_cr["SET_NO"]);
                dr_cr["VISITS_DATES"] = Convert.ToString(readerB_cr["VISITS_DATES"]);
                dr_cr["IE_STAMP_IMAGE"] = Convert.ToString(readerB_cr["IE_STAMP_IMAGE"]);
                dr_cr["IE_STAMP_IMAGE1"] = Convert.ToString(readerB_cr["IE_STAMP_IMAGE1"]);
                dr_cr["LAB_TST_RECT_DT"] = Convert.ToString(readerB_cr["LAB_TST_RECT_DT"]);
                dr_cr["PASSED_INST_NO"] = Convert.ToString(readerB_cr["PASSED_INST_NO"]);
                dr_cr["CONSIGNEE_DTL"] = Convert.ToString(readerB_cr["CONSIGNEE_DTL"]);
                dr_cr["BPO_DTL"] = Convert.ToString(readerB_cr["BPO_DTL"]);
                dr_cr["GOV_BILL_AUTH"] = Convert.ToString(readerB_cr["GOV_BILL_AUTH"]);
                dr_cr["PUR_DTL"] = Convert.ToString(readerB_cr["PUR_DTL"]);
                dr_cr["PUR_AUT_DTL"] = Convert.ToString(readerB_cr["PUR_AUT_DTL"]);
                dr_cr["OFF_INST_NO_DTL"] = Convert.ToString(readerB_cr["OFF_INST_NO_DTL"]);
                dr_cr["UNIT_DTL"] = Convert.ToString(readerB_cr["UNIT_DTL"]);
                dr_cr["DISPATCH_PACKING_NO"] = Convert.ToString(readerB_cr["DISPATCH_PACKING_NO"]);
                dr_cr["INVOICE_NO"] = Convert.ToString(readerB_cr["INVOICE_NO"]);
                dr_cr["NAME_OF_IE"] = Convert.ToString(readerB_cr["NAME_OF_IE"]);
                dr_cr["MAN_TYPE"] = Convert.ToString(readerB_cr["MAN_TYPE"]);
                dr_cr["CONSIGNEE_DESG"] = Convert.ToString(readerB_cr["CONSIGNEE_DESG"]);
                dr_cr["DATETIME"] = Convert.ToString(readerB_cr["DATETIME"]);
                dr_cr["CONSGN_CALL_STATUS"] = Convert.ToString(readerB_cr["CONSGN_CALL_STATUS"]);
                dr_cr["pRegion"] = pregion_cr;
                dt.Rows.Add(dr_cr);
            }
            dsReports.Tables.Add(dt);
            return dsReports;

        }

        private static DataSet funInspectionCertificate(string pcaseno, string pcsno, string recvdt, string pconsidecd, string pregion)
        {
            DataSet dsReports = new DataSet();
            //			string sql = "SELECT     CLR.CASE_NO, CLR.Call_SNO, to_date(TO_CHAR(CLR.Call_Recv_dt, 'mm/dd/yyyy'), 'mm/dd/yyyy')Call_Recv_dt, POM.Region_Code, BOF.BPO_RLY as RLY_CD, CLR.Call_Install_No, TIE.IE_Sname, VND.Vend_Name, VND.Vend_Add1, NVL(VND.Vend_Add2, ' ') AS Vend_Add2,VCT.City AS Vend_City, MFG.Vend_Name AS MFG_Name, MFG.Vend_Add1 AS MFG_Add1,NVL(MFG.Vend_Add2, ' ') AS MFG_Add2, NVL( MCT.City,' ') AS MFG_City, CLR.MFG_PLACE, POM.PO_NO, TO_CHAR(POM.PO_DT,'dd/mm/yyyy') PO_DT, NVL(CON.CONSIGNEE_DESIG, ' ')  AS CONSIGNEE_DESIG, "+
            //				" CON.CONSIGNEE_CD, CCT.City AS CONSIGNEE_CITYNAME, NVL(CON.CONSIGNEE_DEPT, ' ') AS CONSIGNEE_DEPT, NVL(CON.CONSIGNEE_FIRM, ' ') AS CONSIGNEE_FIRM, NVL(PUR.CONSIGNEE_DESIG, ' ') AS PUR_DESIG, PUR.CONSIGNEE_CD AS PUR_CD,  NVL(PUR.CONSIGNEE_DEPT, ' ') AS PUR_DEPT, NVL(PUR.CONSIGNEE_FIRM, ' ') AS PUR_FIRM, NVL(PCT.City, ' ') AS PUR_City, CDT.ITEM_SRNO_PO, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.ITEM_DESC_PO ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN  "+
            //				" CDT.ITEM_DESC_PO ELSE CDT.ITEM_DESC_PO END END AS ITEM_DESC_PO, UOM.UOM_S_DESC, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_ORDERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_ORDERED,0) ELSE NVL(CDT.QTY_ORDERED,0) END END AS QTY_ORDERED, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_OFFERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_OFFERED,0)  ELSE NVL(CDT.CUM_QTY_PREV_OFFERED,0) END END AS CUM_QTY_PREV_OFFERED,  "+
            //				" CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_PASSED,0) ELSE NVL(CDT.CUM_QTY_PREV_PASSED,0) END END AS CUM_QTY_PREV_PASSED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_TO_INSP,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_TO_INSP,0) ELSE NVL(CDT.QTY_TO_INSP,0) END END AS QTY_TO_INSP, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' "+
            //				" THEN NVL(CDT.QTY_PASSED,0) ELSE NVL(CDT.QTY_PASSED,0) END END AS QTY_PASSED,   CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_REJECTED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_REJECTED,0) ELSE NVL(CDT.QTY_REJECTED,0) END END AS QTY_REJECTED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_DUE,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_DUE,0) ELSE NVL(CDT.QTY_DUE,0)END END AS QTY_DUE, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.HOLOGRAM ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN ICI.HOLOGRAM ELSE ICI.HOLOGRAM "+
            //				" END END AS HOLOGRAM, ICI.NUM_VISITS, PRM.VISIT_DATES, BOF.BPO_NAME, BOF.BPO_ORGN, BCT.City, NVL(CLR.HOLOGRAM, '    ')  AS HOLOGRAMORG, NVL(ICI.REMARK, ' ') AS REMARK, CLR.DT_INSP_DESIRE, NVL(ICI.ITEM_REMARK, 'NO REMARK') AS ITEM_REMARK, substr(ICI.AMENDMENT_1, 0, 99) AS AMENDMENT_1, substr(ICI.AMENDMENT_1, 100, 20) AS AMENDMENTDT_1, substr(ICI.AMENDMENT_2, 0, 99) AS AMENDMENT_2, substr(ICI.AMENDMENT_2, 100, 20) AS AMENDMENTDT_2, substr(ICI.AMENDMENT_3, 0, 99) AS AMENDMENT_3, substr(ICI.AMENDMENT_3, 100, 20) AS AMENDMENTDT_3, substr(ICI.AMENDMENT_4, 0, 99) AS AMENDMENT_4, substr(ICI.AMENDMENT_4, 100, 20) "+
            //				" AS AMENDMENTDT_4, ICI.BK_NO, ICI.SET_NO, ICI.VISITS_DATES, ICI.IE_STAMP_IMAGE, ICI.IE_STAMP_IMAGE1, TO_CHAR(ICI.LAB_TST_RECT_DT,'dd/mm/yyyy') LAB_TST_RECT_DT,ICI.PASSED_INST_NO, NVL(CONSIGNEE_DTL, '') AS CONSIGNEE_DTL, NVL(BPO_DTL, '') AS BPO_DTL, NVL(PUR_DTL, '') AS PUR_DTL, NVL(PUR_AUT_DTL, '') AS PUR_AUT_DTL, NVL(OFF_INST_NO_DTL, '') AS OFF_INST_NO_DTL, NVL(UNIT_DTL, '') AS UNIT_DTL  ,TO_CHAR(ICI.DATETIME,'dd/mm/yyyy') DATETIME,ICI.CONSGN_CALL_STATUS FROM RPT_PRM_Inspection_Certificate PRM INNER JOIN  T17_Call_Register CLR ON CLR.CASE_NO = PRM.CASE_NO AND CLR.Call_Recv_dt = PRM.CALL_RECV_DT AND "+
            //				" CLR.Call_SNO = PRM.Call_SNO INNER JOIN T18_Call_Details CDT ON CLR.CASE_NO = CDT.CASE_NO AND CLR.Call_Recv_dt = CDT.Call_Recv_dt AND CLR.Call_SNO = CDT.Call_SNO INNER JOIN T13_PO_Master POM ON CLR.CASE_NO = POM.CASE_NO LEFT OUTER JOIN T15_PO_DETAIL POD ON CLR.CASE_NO = POD.CASE_NO AND CDT.ITEM_SRNO_PO = POD.ITEM_SRNO LEFT OUTER JOIN T04_UOM UOM ON POD.UOM_CD = UOM.UOM_CD LEFT OUTER JOIN "+
            //				" T09_IE TIE ON CLR.IE_CD = TIE.IE_CD LEFT OUTER JOIN T05_Vendor VND ON POM.Vend_Cd = VND.Vend_Cd LEFT OUTER JOIN   T03_City VCT ON VND.Vend_City_Cd = VCT.City_Cd LEFT OUTER JOIN T05_Vendor MFG ON CLR.MFG_CD = MFG.Vend_Cd LEFT OUTER JOIN  T03_City MCT ON MFG.Vend_City_Cd = MCT.City_Cd LEFT OUTER JOIN T14_PO_BPO BPO ON CLR.CASE_NO = BPO.CASE_NO AND CDT.CONSIGNEE_CD = BPO.CONSIGNEE_CD LEFT OUTER  "+
            //				" JOIN T12_BILL_PAYING_OFFICER BOF ON BPO.BPO_CD = BOF.BPO_CD LEFT OUTER JOIN  T03_City BCT ON BOF.BPO_City_Cd = BCT.City_Cd LEFT OUTER JOIN  T06_Consignee CON ON BPO.CONSIGNEE_CD = CON.CONSIGNEE_CD LEFT OUTER JOIN T03_City CCT ON CON.CONSIGNEE_CITY = CCT.City_Cd LEFT OUTER JOIN  T06_Consignee PUR ON POM.PURCHASER_CD = PUR.CONSIGNEE_CD LEFT OUTER JOIN T03_City PCT ON PUR.CONSIGNEE_CITY = PCT.City_Cd INNER JOIN "+
            //				" IC_INTERMEDIATE ICI ON ICI.CASE_NO = CDT.CASE_NO AND ICI.Call_Recv_dt = CDT.Call_Recv_dt AND ICI.Call_SNO = CDT.Call_SNO AND CDT.CONSIGNEE_CD = ICI.CONSIGNEE_CD AND CDT.ITEM_SRNO_PO = ICI.ITEM_SRNO_PO AND PRM.CONSIGNEE_CD = ICI.CONSIGNEE_CD WHERE ICI.ITEM_SRNO_PO IS NOT NULL and ICI.CASE_NO='"+pcaseno+"' "+
            //				" and CLR.Call_SNO="+pcsno+" and CLR.Call_Recv_dt=to_date('"+recvdt+"','mm/dd/yyyy')  and CON.CONSIGNEE_CD='"+pconsidecd+"' ORDER BY CON.CONSIGNEE_CD, POD.ITEM_SRNO";

            string sql = "SELECT     CLR.CASE_NO, CLR.Call_SNO, to_date(TO_CHAR(CLR.Call_Recv_dt, 'mm/dd/yyyy'), 'mm/dd/yyyy')Call_Recv_dt, to_char(CLR.Call_Recv_dt, 'dd/mm/yyyy')Call_Recv_dt_conv,POM.Region_Code, BOF.BPO_RLY as RLY_CD, CLR.Call_Install_No, TIE.IE_Sname, VND.Vend_Name, VND.Vend_Add1, NVL(VND.Vend_Add2, ' ') AS Vend_Add2,VCT.City AS Vend_City, MFG.Vend_Name AS MFG_Name, MFG.Vend_Add1 AS MFG_Add1,NVL(MFG.Vend_Add2, ' ') AS MFG_Add2, NVL( MCT.City,' ') AS MFG_City, CLR.MFG_PLACE, POM.PO_NO, TO_CHAR(POM.PO_DT,'dd/mm/yyyy') PO_DT, NVL(CON.CONSIGNEE_DESIG, ' ')  AS CONSIGNEE_DESIG, " +
                " CON.CONSIGNEE_CD, CCT.City AS CONSIGNEE_CITYNAME, NVL(CON.CONSIGNEE_DEPT, ' ') AS CONSIGNEE_DEPT, NVL(CON.CONSIGNEE_FIRM, ' ') AS CONSIGNEE_FIRM, NVL(PUR.CONSIGNEE_DESIG, ' ') AS PUR_DESIG, PUR.CONSIGNEE_CD AS PUR_CD,  NVL(PUR.CONSIGNEE_DEPT, ' ') AS PUR_DEPT, NVL(PUR.CONSIGNEE_FIRM, ' ') AS PUR_FIRM, NVL(PCT.City, ' ') AS PUR_City, CDT.ITEM_SRNO_PO, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.ITEM_DESC_PO ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN  " +
                " CDT.ITEM_DESC_PO ELSE CDT.ITEM_DESC_PO END END AS ITEM_DESC_PO, UOM.UOM_S_DESC, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_ORDERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_ORDERED,0) ELSE NVL(CDT.QTY_ORDERED,0) END END AS QTY_ORDERED, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_OFFERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_OFFERED,0)  ELSE NVL(CDT.CUM_QTY_PREV_OFFERED,0) END END AS CUM_QTY_PREV_OFFERED,  " +
                " CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_PASSED,0) ELSE NVL(CDT.CUM_QTY_PREV_PASSED,0) END END AS CUM_QTY_PREV_PASSED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_TO_INSP,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_TO_INSP,0) ELSE NVL(CDT.QTY_TO_INSP,0) END END AS QTY_TO_INSP, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' " +
                " THEN NVL(CDT.QTY_PASSED,0) ELSE NVL(CDT.QTY_PASSED,0) END END AS QTY_PASSED,   CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_REJECTED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_REJECTED,0) ELSE NVL(CDT.QTY_REJECTED,0) END END AS QTY_REJECTED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_DUE,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_DUE,0) ELSE NVL(CDT.QTY_DUE,0)END END AS QTY_DUE, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.HOLOGRAM ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN ICI.HOLOGRAM ELSE ICI.HOLOGRAM " +
                " END END AS HOLOGRAM, ICI.NUM_VISITS, PRM.VISIT_DATES, BOF.BPO_NAME, BOF.BPO_ORGN, BCT.City, NVL(CLR.HOLOGRAM, '    ')  AS HOLOGRAMORG, NVL(ICI.REMARK, ' ') AS REMARK, CLR.DT_INSP_DESIRE, to_char(CLR.DT_INSP_DESIRE, 'dd/mm/yyyy')DT_INSP_DESIRE_conv, NVL(ICI.ITEM_REMARK, 'NO REMARK') AS ITEM_REMARK, substr(ICI.AMENDMENT_1, 0, 99) AS AMENDMENT_1, substr(ICI.AMENDMENT_1, 100, 20) AS AMENDMENTDT_1, substr(ICI.AMENDMENT_2, 0, 99) AS AMENDMENT_2, substr(ICI.AMENDMENT_2, 100, 20) AS AMENDMENTDT_2, substr(ICI.AMENDMENT_3, 0, 99) AS AMENDMENT_3, substr(ICI.AMENDMENT_3, 100, 20) AS AMENDMENTDT_3, substr(ICI.AMENDMENT_4, 0, 99) AS AMENDMENT_4, substr(ICI.AMENDMENT_4, 100, 20) " +
                " AS AMENDMENTDT_4, ICI.BK_NO, ICI.SET_NO, ICI.VISITS_DATES, ICI.IE_STAMP_IMAGE, ICI.IE_STAMP_IMAGE1, TO_CHAR(ICI.LAB_TST_RECT_DT,'dd/mm/yyyy') LAB_TST_RECT_DT,ICI.PASSED_INST_NO, NVL(CONSIGNEE_DTL, '') AS CONSIGNEE_DTL, NVL(BPO_DTL, '') AS BPO_DTL, NVL(PUR_DTL, '') AS PUR_DTL, NVL(PUR_AUT_DTL, '') AS PUR_AUT_DTL, NVL(OFF_INST_NO_DTL, '') AS OFF_INST_NO_DTL, NVL(UNIT_DTL, '') AS UNIT_DTL  ,TO_CHAR(ICI.DATETIME,'dd/mm/yyyy') DATETIME,ICI.CONSGN_CALL_STATUS FROM RPT_PRM_Inspection_Certificate PRM INNER JOIN  T17_Call_Register CLR ON CLR.CASE_NO = PRM.CASE_NO AND CLR.Call_Recv_dt = PRM.CALL_RECV_DT AND " +
                " CLR.Call_SNO = PRM.Call_SNO INNER JOIN T18_Call_Details CDT ON CLR.CASE_NO = CDT.CASE_NO AND CLR.Call_Recv_dt = CDT.Call_Recv_dt AND CLR.Call_SNO = CDT.Call_SNO INNER JOIN T13_PO_Master POM ON CLR.CASE_NO = POM.CASE_NO LEFT OUTER JOIN T15_PO_DETAIL POD ON CLR.CASE_NO = POD.CASE_NO AND CDT.ITEM_SRNO_PO = POD.ITEM_SRNO LEFT OUTER JOIN T04_UOM UOM ON POD.UOM_CD = UOM.UOM_CD LEFT OUTER JOIN " +
                " T09_IE TIE ON CLR.IE_CD = TIE.IE_CD LEFT OUTER JOIN T05_Vendor VND ON POM.Vend_Cd = VND.Vend_Cd LEFT OUTER JOIN   T03_City VCT ON VND.Vend_City_Cd = VCT.City_Cd LEFT OUTER JOIN T05_Vendor MFG ON CLR.MFG_CD = MFG.Vend_Cd LEFT OUTER JOIN  T03_City MCT ON MFG.Vend_City_Cd = MCT.City_Cd LEFT OUTER JOIN T14_PO_BPO BPO ON CLR.CASE_NO = BPO.CASE_NO AND CDT.CONSIGNEE_CD = BPO.CONSIGNEE_CD LEFT OUTER  " +
                " JOIN T12_BILL_PAYING_OFFICER BOF ON BPO.BPO_CD = BOF.BPO_CD LEFT OUTER JOIN  T03_City BCT ON BOF.BPO_City_Cd = BCT.City_Cd LEFT OUTER JOIN  T06_Consignee CON ON BPO.CONSIGNEE_CD = CON.CONSIGNEE_CD LEFT OUTER JOIN T03_City CCT ON CON.CONSIGNEE_CITY = CCT.City_Cd LEFT OUTER JOIN  T06_Consignee PUR ON POM.PURCHASER_CD = PUR.CONSIGNEE_CD LEFT OUTER JOIN T03_City PCT ON PUR.CONSIGNEE_CITY = PCT.City_Cd INNER JOIN " +
                " IC_INTERMEDIATE ICI ON ICI.CASE_NO = CDT.CASE_NO AND ICI.Call_Recv_dt = CDT.Call_Recv_dt AND ICI.Call_SNO = CDT.Call_SNO AND CDT.CONSIGNEE_CD = ICI.CONSIGNEE_CD AND CDT.ITEM_SRNO_PO = ICI.ITEM_SRNO_PO AND PRM.CONSIGNEE_CD = ICI.CONSIGNEE_CD WHERE ICI.ITEM_SRNO_PO IS NOT NULL and ICI.CASE_NO='" + pcaseno + "' " +
                " and CLR.Call_SNO=" + pcsno + " and CLR.Call_Recv_dt=to_date('" + recvdt + "','mm/dd/yyyy')  and CON.CONSIGNEE_CD='" + pconsidecd + "' ORDER BY CON.CONSIGNEE_CD, POD.ITEM_SRNO";
            //
            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = sql;
            cmd.Connection = conn1;
            if (conn1.State == ConnectionState.Closed)
                conn1.Open();
            OracleDataReader readerB = cmd.ExecuteReader();
            DataTable dt = new DataTable(); ;
            dt.Columns.Add("CASE_NO", typeof(string));
            dt.Columns.Add("Call_SNO", typeof(string));
            dt.Columns.Add("Call_Recv_dt", typeof(string));
            dt.Columns.Add("Region_Code", typeof(string));
            dt.Columns.Add("RLY_CD", typeof(string));
            dt.Columns.Add("Call_Install_No", typeof(string));
            dt.Columns.Add("IE_Sname", typeof(string));
            dt.Columns.Add("Vend_Name", typeof(string));
            dt.Columns.Add("Vend_Add1", typeof(string));
            dt.Columns.Add("Vend_Add2", typeof(string));
            dt.Columns.Add("Vend_City", typeof(string));
            dt.Columns.Add("MFG_Name", typeof(string));
            dt.Columns.Add("MFG_Add1", typeof(string));
            dt.Columns.Add("MFG_Add2", typeof(string));
            dt.Columns.Add("MFG_City", typeof(string));
            dt.Columns.Add("MFG_PLACE", typeof(string));
            dt.Columns.Add("PO_NO", typeof(string));
            dt.Columns.Add("PO_DT", typeof(string));
            dt.Columns.Add("CONSIGNEE_DESIG", typeof(string));
            dt.Columns.Add("CONSIGNEE_CD", typeof(Int32));
            dt.Columns.Add("CONSIGNEE_CITYNAME", typeof(string));
            dt.Columns.Add("CONSIGNEE_DEPT", typeof(string));
            dt.Columns.Add("CONSIGNEE_FIRM", typeof(string));
            dt.Columns.Add("PUR_DESIG", typeof(string));
            dt.Columns.Add("PUR_CD", typeof(string));
            dt.Columns.Add("PUR_DEPT", typeof(string));
            dt.Columns.Add("PUR_FIRM", typeof(string));
            dt.Columns.Add("PUR_City", typeof(string));
            dt.Columns.Add("ITEM_SRNO_PO", typeof(string));
            dt.Columns.Add("ITEM_DESC_PO", typeof(string));
            dt.Columns.Add("UOM_S_DESC", typeof(string));
            dt.Columns.Add("QTY_ORDERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_OFFERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_PASSED", typeof(double));
            dt.Columns.Add("QTY_TO_INSP", typeof(double));
            dt.Columns.Add("QTY_PASSED", typeof(double));
            dt.Columns.Add("QTY_REJECTED", typeof(double));
            dt.Columns.Add("QTY_DUE", typeof(double));
            dt.Columns.Add("HOLOGRAM", typeof(string));
            dt.Columns.Add("NUM_VISITS", typeof(Int32));
            dt.Columns.Add("VISIT_DATES", typeof(string));
            dt.Columns.Add("BPO_NAME", typeof(string));
            dt.Columns.Add("BPO_ORGN", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("HOLOGRAMORG", typeof(string));
            dt.Columns.Add("REMARK", typeof(string));
            dt.Columns.Add("DT_INSP_DESIRE", typeof(string));
            dt.Columns.Add("ITEM_REMARK", typeof(string));
            dt.Columns.Add("AMENDMENT_1", typeof(string));
            dt.Columns.Add("AMENDMENTDT_1", typeof(string));
            dt.Columns.Add("AMENDMENT_2", typeof(string));
            dt.Columns.Add("AMENDMENTDT_2", typeof(string));
            dt.Columns.Add("AMENDMENT_3", typeof(string));
            dt.Columns.Add("AMENDMENTDT_3", typeof(string));
            dt.Columns.Add("AMENDMENT_4", typeof(string));
            dt.Columns.Add("AMENDMENTDT_4", typeof(string));
            dt.Columns.Add("BK_NO", typeof(string));
            dt.Columns.Add("SET_NO", typeof(string));
            dt.Columns.Add("VISITS_DATES", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE1", typeof(string));
            dt.Columns.Add("LAB_TST_RECT_DT", typeof(string));
            dt.Columns.Add("PASSED_INST_NO", typeof(string));
            dt.Columns.Add("CONSIGNEE_DTL", typeof(string));
            dt.Columns.Add("BPO_DTL", typeof(string));
            dt.Columns.Add("PUR_DTL", typeof(string));
            dt.Columns.Add("PUR_AUT_DTL", typeof(string));
            dt.Columns.Add("OFF_INST_NO_DTL", typeof(string));
            dt.Columns.Add("UNIT_DTL", typeof(string));
            dt.Columns.Add("DATETIME", typeof(string));
            dt.Columns.Add("CONSGN_CALL_STATUS", typeof(string));
            dt.Columns.Add("pRegion", typeof(string));
            dt.Columns.Add("Call_Recv_dt_conv", typeof(string));
            dt.Columns.Add("DT_INSP_DESIRE_conv", typeof(string));
            dt.TableName = "Command";
            while (readerB.Read())
            {
                DataRow dr = dt.NewRow();// dsReports.Tables["Command"].Rows.Add();
                dr["CASE_NO"] = Convert.ToString(readerB["CASE_NO"]);
                dr["Call_SNO"] = Convert.ToString(readerB["Call_SNO"]);
                dr["Call_Recv_dt"] = Convert.ToString(readerB["Call_Recv_dt"]);
                dr["Region_Code"] = Convert.ToString(readerB["Region_Code"]);
                dr["RLY_CD"] = Convert.ToString(readerB["RLY_CD"]);
                dr["Call_Install_No"] = Convert.ToString(readerB["Call_Install_No"]);
                dr["IE_Sname"] = Convert.ToString(readerB["IE_Sname"]);
                dr["Vend_Name"] = Convert.ToString(readerB["Vend_Name"]);
                dr["Vend_Add1"] = Convert.ToString(readerB["Vend_Add1"]);
                dr["Vend_Add2"] = Convert.ToString(readerB["Vend_Add2"]);
                dr["Vend_City"] = Convert.ToString(readerB["Vend_City"]);
                dr["MFG_Name"] = Convert.ToString(readerB["MFG_Name"]);
                dr["MFG_Add1"] = Convert.ToString(readerB["MFG_Add1"]);
                dr["MFG_Add2"] = Convert.ToString(readerB["MFG_Add2"]);
                dr["MFG_City"] = Convert.ToString(readerB["MFG_City"]);
                dr["MFG_PLACE"] = Convert.ToString(readerB["MFG_PLACE"]);
                dr["PO_NO"] = Convert.ToString(readerB["PO_NO"]);
                dr["PO_DT"] = Convert.ToString(readerB["PO_DT"]);
                dr["CONSIGNEE_DESIG"] = Convert.ToString(readerB["CONSIGNEE_DESIG"]);
                dr["CONSIGNEE_CD"] = Convert.ToString(readerB["CONSIGNEE_CD"]);
                dr["CONSIGNEE_CITYNAME"] = Convert.ToString(readerB["CONSIGNEE_CITYNAME"]);
                dr["CONSIGNEE_DEPT"] = Convert.ToString(readerB["CONSIGNEE_DEPT"]);
                dr["CONSIGNEE_FIRM"] = Convert.ToString(readerB["CONSIGNEE_FIRM"]);
                dr["PUR_DESIG"] = Convert.ToString(readerB["PUR_DESIG"]);
                dr["PUR_CD"] = Convert.ToString(readerB["PUR_CD"]);
                dr["PUR_DEPT"] = Convert.ToString(readerB["PUR_DEPT"]);
                dr["PUR_FIRM"] = Convert.ToString(readerB["PUR_FIRM"]);
                dr["PUR_City"] = Convert.ToString(readerB["PUR_City"]);
                dr["ITEM_SRNO_PO"] = Convert.ToString(readerB["ITEM_SRNO_PO"]);
                dr["ITEM_DESC_PO"] = Convert.ToString(readerB["ITEM_DESC_PO"]);
                dr["UOM_S_DESC"] = Convert.ToString(readerB["UOM_S_DESC"]);
                dr["QTY_ORDERED"] = Convert.ToString(readerB["QTY_ORDERED"]);
                dr["CUM_QTY_PREV_OFFERED"] = Convert.ToString(readerB["CUM_QTY_PREV_OFFERED"]);
                dr["CUM_QTY_PREV_PASSED"] = Convert.ToString(readerB["CUM_QTY_PREV_PASSED"]);
                dr["QTY_TO_INSP"] = Convert.ToString(readerB["QTY_TO_INSP"]);
                dr["QTY_PASSED"] = Convert.ToString(readerB["QTY_PASSED"]);
                dr["QTY_REJECTED"] = Convert.ToString(readerB["QTY_REJECTED"]);
                dr["QTY_DUE"] = Convert.ToString(readerB["QTY_DUE"]);
                dr["HOLOGRAM"] = Convert.ToString(readerB["HOLOGRAM"]);
                dr["NUM_VISITS"] = Convert.ToString(readerB["NUM_VISITS"]);
                dr["VISIT_DATES"] = Convert.ToString(readerB["VISIT_DATES"]);
                dr["BPO_NAME"] = Convert.ToString(readerB["BPO_NAME"]);
                dr["BPO_ORGN"] = Convert.ToString(readerB["BPO_ORGN"]);
                dr["City"] = Convert.ToString(readerB["City"]);
                dr["HOLOGRAMORG"] = Convert.ToString(readerB["HOLOGRAMORG"]);
                dr["REMARK"] = Convert.ToString(readerB["REMARK"]);
                dr["DT_INSP_DESIRE"] = Convert.ToString(readerB["DT_INSP_DESIRE"]);
                dr["ITEM_REMARK"] = Convert.ToString(readerB["ITEM_REMARK"]);
                dr["AMENDMENT_1"] = Convert.ToString(readerB["AMENDMENT_1"]);
                dr["AMENDMENTDT_1"] = Convert.ToString(readerB["AMENDMENTDT_1"]);
                dr["AMENDMENT_2"] = Convert.ToString(readerB["AMENDMENT_2"]);
                dr["AMENDMENTDT_2"] = Convert.ToString(readerB["AMENDMENTDT_2"]);
                dr["AMENDMENT_3"] = Convert.ToString(readerB["AMENDMENT_3"]);
                dr["AMENDMENTDT_3"] = Convert.ToString(readerB["AMENDMENTDT_3"]);
                dr["AMENDMENT_4"] = Convert.ToString(readerB["AMENDMENT_4"]);
                dr["AMENDMENTDT_4"] = Convert.ToString(readerB["AMENDMENTDT_4"]);
                dr["BK_NO"] = Convert.ToString(readerB["BK_NO"]);
                dr["SET_NO"] = Convert.ToString(readerB["SET_NO"]);
                dr["VISITS_DATES"] = Convert.ToString(readerB["VISITS_DATES"]);
                dr["IE_STAMP_IMAGE"] = Convert.ToString(readerB["IE_STAMP_IMAGE"]);
                dr["IE_STAMP_IMAGE1"] = Convert.ToString(readerB["IE_STAMP_IMAGE1"]);
                dr["LAB_TST_RECT_DT"] = Convert.ToString(readerB["LAB_TST_RECT_DT"]);
                dr["PASSED_INST_NO"] = Convert.ToString(readerB["PASSED_INST_NO"]);
                dr["CONSIGNEE_DTL"] = Convert.ToString(readerB["CONSIGNEE_DTL"]);
                dr["BPO_DTL"] = Convert.ToString(readerB["BPO_DTL"]);
                dr["PUR_DTL"] = Convert.ToString(readerB["PUR_DTL"]);
                dr["PUR_AUT_DTL"] = Convert.ToString(readerB["PUR_AUT_DTL"]);
                dr["OFF_INST_NO_DTL"] = Convert.ToString(readerB["OFF_INST_NO_DTL"]);
                dr["UNIT_DTL"] = Convert.ToString(readerB["UNIT_DTL"]);
                dr["DATETIME"] = Convert.ToString(readerB["DATETIME"]);
                dr["CONSGN_CALL_STATUS"] = Convert.ToString(readerB["CONSGN_CALL_STATUS"]);
                dr["pRegion"] = pregion;
                dr["Call_Recv_dt_conv"] = Convert.ToString(readerB["Call_Recv_dt_conv"]);
                dr["DT_INSP_DESIRE_conv"] = Convert.ToString(readerB["DT_INSP_DESIRE_conv"]);
                dt.Rows.Add(dr);
            }
            dsReports.Tables.Add(dt);
            return dsReports;
        }

        private static DataSet funInspectionCertificate_cr_r(string pcaseno_cr_r, string pcsno_cr_r, string recvdt_cr_r, string pconsidecd_cr_r, string pregion_cr_r)
        {
            DataSet dsReports = new DataSet();
            string sql_cr_r = "SELECT     CLR.CASE_NO, CLR.Call_SNO, to_date(TO_CHAR(CLR.Call_Recv_dt, 'mm/dd/yyyy'), 'mm/dd/yyyy')Call_Recv_dt, POM.Region_Code, BOF.BPO_RLY as RLY_CD, CLR.Call_Install_No, TIE.IE_Sname, VND.Vend_Name, VND.Vend_Add1, NVL(VND.Vend_Add2, ' ') AS Vend_Add2,VCT.City AS Vend_City, MFG.Vend_Name AS MFG_Name, MFG.Vend_Add1 AS MFG_Add1,NVL(MFG.Vend_Add2, ' ') AS MFG_Add2, NVL( MCT.City,' ') AS MFG_City, CLR.MFG_PLACE, POM.PO_NO, TO_CHAR(POM.PO_DT,'dd/mm/yyyy') PO_DT, NVL(CON.CONSIGNEE_DESIG, ' ')  AS CONSIGNEE_DESIG, " +
                " CON.CONSIGNEE_CD, CCT.City AS CONSIGNEE_CITYNAME, NVL(CON.CONSIGNEE_DEPT, ' ') AS CONSIGNEE_DEPT, NVL(CON.CONSIGNEE_FIRM, ' ') AS CONSIGNEE_FIRM, NVL(PUR.CONSIGNEE_DESIG, ' ') AS PUR_DESIG, PUR.CONSIGNEE_CD AS PUR_CD,  NVL(PUR.CONSIGNEE_DEPT, ' ') AS PUR_DEPT, NVL(PUR.CONSIGNEE_FIRM, ' ') AS PUR_FIRM, NVL(PCT.City, ' ') AS PUR_City, CDT.ITEM_SRNO_PO, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.ITEM_DESC_PO ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN  " +
                " CDT.ITEM_DESC_PO ELSE CDT.ITEM_DESC_PO END END AS ITEM_DESC_PO, UOM.UOM_S_DESC, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_ORDERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_ORDERED,0) ELSE NVL(CDT.QTY_ORDERED,0) END END AS QTY_ORDERED, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_OFFERED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_OFFERED,0)  ELSE NVL(CDT.CUM_QTY_PREV_OFFERED,0) END END AS CUM_QTY_PREV_OFFERED,  " +
                " CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.CUM_QTY_PREV_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.CUM_QTY_PREV_PASSED,0) ELSE NVL(CDT.CUM_QTY_PREV_PASSED,0) END END AS CUM_QTY_PREV_PASSED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_TO_INSP,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_TO_INSP,0) ELSE NVL(CDT.QTY_TO_INSP,0) END END AS QTY_TO_INSP, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_PASSED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' " +
                " THEN NVL(CDT.QTY_PASSED,0) ELSE NVL(CDT.QTY_PASSED,0) END END AS QTY_PASSED,   CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_REJECTED,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_REJECTED,0) ELSE NVL(CDT.QTY_REJECTED,0) END END AS QTY_REJECTED,  CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN NVL(ICI.QTY_DUE,0) ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN NVL(CDT.QTY_DUE,0) ELSE NVL(CDT.QTY_DUE,0)END END AS QTY_DUE, CASE WHEN CONSGN_CALL_STATUS = 'Z' THEN ICI.HOLOGRAM ELSE CASE WHEN CONSGN_CALL_STATUS = 'A' THEN ICI.HOLOGRAM ELSE ICI.HOLOGRAM " +
                " END END AS HOLOGRAM, ICI.NUM_VISITS, PRM.VISIT_DATES, BOF.BPO_NAME, BOF.BPO_ORGN, BCT.City, NVL(CLR.HOLOGRAM, '    ')  AS HOLOGRAMORG, NVL(ICI.REMARK, ' ') AS REMARK, CLR.DT_INSP_DESIRE, NVL(ICI.ITEM_REMARK, 'NO REMARK') AS ITEM_REMARK, substr(ICI.AMENDMENT_1, 0, 99) AS AMENDMENT_1, substr(ICI.AMENDMENT_1, 100, 20) AS AMENDMENTDT_1, substr(ICI.AMENDMENT_2, 0, 99) AS AMENDMENT_2, substr(ICI.AMENDMENT_2, 100, 20) AS AMENDMENTDT_2, substr(ICI.AMENDMENT_3, 0, 99) AS AMENDMENT_3, substr(ICI.AMENDMENT_3, 100, 20) AS AMENDMENTDT_3, substr(ICI.AMENDMENT_4, 0, 99) AS AMENDMENT_4, substr(ICI.AMENDMENT_4, 100, 20) " +
                " AS AMENDMENTDT_4, ICI.BK_NO, ICI.SET_NO, ICI.VISITS_DATES, ICI.IE_STAMP_IMAGE, ICI.IE_STAMP_IMAGE1, TO_CHAR(ICI.LAB_TST_RECT_DT,'dd/mm/yyyy') LAB_TST_RECT_DT,ICI.PASSED_INST_NO, NVL(CONSIGNEE_DTL, '') AS CONSIGNEE_DTL, NVL(BPO_DTL, '') AS BPO_DTL,NVL(GOV_BILL_AUTH, '') AS GOV_BILL_AUTH, NVL(PUR_DTL, '') AS PUR_DTL, NVL(PUR_AUT_DTL, '') AS PUR_AUT_DTL, NVL(OFF_INST_NO_DTL, '') AS OFF_INST_NO_DTL, NVL(UNIT_DTL, '') AS UNIT_DTL, NVL(DISPATCH_PACKING_NO, '') As DISPATCH_PACKING_NO, NVL(INVOICE_NO, '') As INVOICE_NO, NVL(NAME_OF_IE, '') As NAME_OF_IE, NVL(MAN_TYPE, '') As MAN_TYPE, NVL(CONSIGNEE_DESG, '') As CONSIGNEE_DESG,TO_CHAR(ICI.DATETIME,'dd/mm/yyyy') DATETIME,ICI.CONSGN_CALL_STATUS FROM RPT_PRM_Inspection_Certificate PRM INNER JOIN  T17_Call_Register CLR ON CLR.CASE_NO = PRM.CASE_NO AND CLR.Call_Recv_dt = PRM.CALL_RECV_DT AND " +
                " CLR.Call_SNO = PRM.Call_SNO INNER JOIN T18_Call_Details CDT ON CLR.CASE_NO = CDT.CASE_NO AND CLR.Call_Recv_dt = CDT.Call_Recv_dt AND CLR.Call_SNO = CDT.Call_SNO INNER JOIN T13_PO_Master POM ON CLR.CASE_NO = POM.CASE_NO LEFT OUTER JOIN T15_PO_DETAIL POD ON CLR.CASE_NO = POD.CASE_NO AND CDT.ITEM_SRNO_PO = POD.ITEM_SRNO LEFT OUTER JOIN T04_UOM UOM ON POD.UOM_CD = UOM.UOM_CD LEFT OUTER JOIN " +
                " T09_IE TIE ON CLR.IE_CD = TIE.IE_CD LEFT OUTER JOIN T05_Vendor VND ON POM.Vend_Cd = VND.Vend_Cd LEFT OUTER JOIN   T03_City VCT ON VND.Vend_City_Cd = VCT.City_Cd LEFT OUTER JOIN T05_Vendor MFG ON CLR.MFG_CD = MFG.Vend_Cd LEFT OUTER JOIN  T03_City MCT ON MFG.Vend_City_Cd = MCT.City_Cd LEFT OUTER JOIN T14_PO_BPO BPO ON CLR.CASE_NO = BPO.CASE_NO AND CDT.CONSIGNEE_CD = BPO.CONSIGNEE_CD LEFT OUTER  " +
                " JOIN T12_BILL_PAYING_OFFICER BOF ON BPO.BPO_CD = BOF.BPO_CD LEFT OUTER JOIN  T03_City BCT ON BOF.BPO_City_Cd = BCT.City_Cd LEFT OUTER JOIN  T06_Consignee CON ON BPO.CONSIGNEE_CD = CON.CONSIGNEE_CD LEFT OUTER JOIN T03_City CCT ON CON.CONSIGNEE_CITY = CCT.City_Cd LEFT OUTER JOIN  T06_Consignee PUR ON POM.PURCHASER_CD = PUR.CONSIGNEE_CD LEFT OUTER JOIN T03_City PCT ON PUR.CONSIGNEE_CITY = PCT.City_Cd INNER JOIN " +
                " IC_INTERMEDIATE ICI ON ICI.CASE_NO = CDT.CASE_NO AND ICI.Call_Recv_dt = CDT.Call_Recv_dt AND ICI.Call_SNO = CDT.Call_SNO AND CDT.CONSIGNEE_CD = ICI.CONSIGNEE_CD AND CDT.ITEM_SRNO_PO = ICI.ITEM_SRNO_PO AND PRM.CONSIGNEE_CD = ICI.CONSIGNEE_CD WHERE ICI.ITEM_SRNO_PO IS NOT NULL and ICI.CASE_NO='" + pcaseno_cr_r + "' " +
                " and CLR.Call_SNO=" + pcsno_cr_r + " and CLR.Call_Recv_dt=to_date('" + recvdt_cr_r + "','mm/dd/yyyy')  and CON.CONSIGNEE_CD='" + pconsidecd_cr_r + "' ORDER BY CON.CONSIGNEE_CD, POD.ITEM_SRNO";
            OracleCommand cmd_cr_r = new OracleCommand();
            cmd_cr_r.CommandText = sql_cr_r;
            cmd_cr_r.Connection = conn1;
            if (conn1.State == ConnectionState.Closed)
                conn1.Open();
            OracleDataReader readerB_cr_r = cmd_cr_r.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CASE_NO", typeof(string));
            dt.Columns.Add("Call_SNO", typeof(string));
            dt.Columns.Add("Call_Recv_dt", typeof(string));
            dt.Columns.Add("Region_Code", typeof(string));
            dt.Columns.Add("RLY_CD", typeof(string));
            dt.Columns.Add("Call_Install_No", typeof(string));
            dt.Columns.Add("IE_Sname", typeof(string));
            dt.Columns.Add("Vend_Name", typeof(string));
            dt.Columns.Add("Vend_Add1", typeof(string));
            dt.Columns.Add("Vend_Add2", typeof(string));
            dt.Columns.Add("Vend_City", typeof(string));
            dt.Columns.Add("MFG_Name", typeof(string));
            dt.Columns.Add("MFG_Add1", typeof(string));
            dt.Columns.Add("MFG_Add2", typeof(string));
            dt.Columns.Add("MFG_City", typeof(string));
            dt.Columns.Add("MFG_PLACE", typeof(string));
            dt.Columns.Add("PO_NO", typeof(string));
            dt.Columns.Add("PO_DT", typeof(string));
            dt.Columns.Add("CONSIGNEE_DESIG", typeof(string));
            dt.Columns.Add("CONSIGNEE_CD", typeof(Int32));
            dt.Columns.Add("CONSIGNEE_CITYNAME", typeof(string));
            dt.Columns.Add("CONSIGNEE_DEPT", typeof(string));
            dt.Columns.Add("CONSIGNEE_FIRM", typeof(string));
            dt.Columns.Add("PUR_DESIG", typeof(string));
            dt.Columns.Add("PUR_CD", typeof(string));
            dt.Columns.Add("PUR_DEPT", typeof(string));
            dt.Columns.Add("PUR_FIRM", typeof(string));
            dt.Columns.Add("PUR_City", typeof(string));
            dt.Columns.Add("ITEM_SRNO_PO", typeof(string));
            dt.Columns.Add("ITEM_DESC_PO", typeof(string));
            dt.Columns.Add("UOM_S_DESC", typeof(string));
            dt.Columns.Add("QTY_ORDERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_OFFERED", typeof(double));
            dt.Columns.Add("CUM_QTY_PREV_PASSED", typeof(double));
            dt.Columns.Add("QTY_TO_INSP", typeof(double));
            dt.Columns.Add("QTY_PASSED", typeof(double));
            dt.Columns.Add("QTY_REJECTED", typeof(double));
            dt.Columns.Add("QTY_DUE", typeof(double));
            dt.Columns.Add("HOLOGRAM", typeof(string));
            dt.Columns.Add("NUM_VISITS", typeof(Int32));
            dt.Columns.Add("VISIT_DATES", typeof(string));
            dt.Columns.Add("BPO_NAME", typeof(string));
            dt.Columns.Add("BPO_ORGN", typeof(string));
            dt.Columns.Add("City", typeof(string));
            dt.Columns.Add("HOLOGRAMORG", typeof(string));
            dt.Columns.Add("REMARK", typeof(string));
            dt.Columns.Add("DT_INSP_DESIRE", typeof(string));
            dt.Columns.Add("ITEM_REMARK", typeof(string));
            dt.Columns.Add("AMENDMENT_1", typeof(string));
            dt.Columns.Add("AMENDMENTDT_1", typeof(string));
            dt.Columns.Add("AMENDMENT_2", typeof(string));
            dt.Columns.Add("AMENDMENTDT_2", typeof(string));
            dt.Columns.Add("AMENDMENT_3", typeof(string));
            dt.Columns.Add("AMENDMENTDT_3", typeof(string));
            dt.Columns.Add("AMENDMENT_4", typeof(string));
            dt.Columns.Add("AMENDMENTDT_4", typeof(string));
            dt.Columns.Add("BK_NO", typeof(string));
            dt.Columns.Add("SET_NO", typeof(string));
            dt.Columns.Add("VISITS_DATES", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE", typeof(string));
            dt.Columns.Add("IE_STAMP_IMAGE1", typeof(string));
            dt.Columns.Add("LAB_TST_RECT_DT", typeof(string));
            dt.Columns.Add("PASSED_INST_NO", typeof(string));
            dt.Columns.Add("CONSIGNEE_DTL", typeof(string));
            dt.Columns.Add("BPO_DTL", typeof(string));
            dt.Columns.Add("GOV_BILL_AUTH", typeof(string));
            dt.Columns.Add("PUR_DTL", typeof(string));
            dt.Columns.Add("PUR_AUT_DTL", typeof(string));
            dt.Columns.Add("OFF_INST_NO_DTL", typeof(string));
            dt.Columns.Add("UNIT_DTL", typeof(string));
            dt.Columns.Add("DISPATCH_PACKING_NO", typeof(string));
            dt.Columns.Add("INVOICE_NO", typeof(string));
            dt.Columns.Add("NAME_OF_IE", typeof(string));
            dt.Columns.Add("MAN_TYPE", typeof(string));
            dt.Columns.Add("CONSIGNEE_DESG", typeof(string));
            dt.Columns.Add("DATETIME", typeof(string));
            dt.Columns.Add("CONSGN_CALL_STATUS", typeof(string));
            dt.Columns.Add("pRegion", typeof(string));
            dt.TableName = "Command";
            while (readerB_cr_r.Read())
            {
                DataRow dr_cr_r = dt.NewRow();// dsReports.Tables["Command"].Rows.Add();
                dr_cr_r["CASE_NO"] = Convert.ToString(readerB_cr_r["CASE_NO"]);
                dr_cr_r["Call_SNO"] = Convert.ToString(readerB_cr_r["Call_SNO"]);
                dr_cr_r["Call_Recv_dt"] = Convert.ToString(readerB_cr_r["Call_Recv_dt"]);
                dr_cr_r["Region_Code"] = Convert.ToString(readerB_cr_r["Region_Code"]);
                dr_cr_r["RLY_CD"] = Convert.ToString(readerB_cr_r["RLY_CD"]);
                dr_cr_r["Call_Install_No"] = Convert.ToString(readerB_cr_r["Call_Install_No"]);
                dr_cr_r["IE_Sname"] = Convert.ToString(readerB_cr_r["IE_Sname"]);
                dr_cr_r["Vend_Name"] = Convert.ToString(readerB_cr_r["Vend_Name"]);
                dr_cr_r["Vend_Add1"] = Convert.ToString(readerB_cr_r["Vend_Add1"]);
                dr_cr_r["Vend_Add2"] = Convert.ToString(readerB_cr_r["Vend_Add2"]);
                dr_cr_r["Vend_City"] = Convert.ToString(readerB_cr_r["Vend_City"]);
                dr_cr_r["MFG_Name"] = Convert.ToString(readerB_cr_r["MFG_Name"]);
                dr_cr_r["MFG_Add1"] = Convert.ToString(readerB_cr_r["MFG_Add1"]);
                dr_cr_r["MFG_Add2"] = Convert.ToString(readerB_cr_r["MFG_Add2"]);
                dr_cr_r["MFG_City"] = Convert.ToString(readerB_cr_r["MFG_City"]);
                dr_cr_r["MFG_PLACE"] = Convert.ToString(readerB_cr_r["MFG_PLACE"]);
                dr_cr_r["PO_NO"] = Convert.ToString(readerB_cr_r["PO_NO"]);
                dr_cr_r["PO_DT"] = Convert.ToString(readerB_cr_r["PO_DT"]);
                dr_cr_r["CONSIGNEE_DESIG"] = Convert.ToString(readerB_cr_r["CONSIGNEE_DESIG"]);
                dr_cr_r["CONSIGNEE_CD"] = Convert.ToString(readerB_cr_r["CONSIGNEE_CD"]);
                dr_cr_r["CONSIGNEE_CITYNAME"] = Convert.ToString(readerB_cr_r["CONSIGNEE_CITYNAME"]);
                dr_cr_r["CONSIGNEE_DEPT"] = Convert.ToString(readerB_cr_r["CONSIGNEE_DEPT"]);
                dr_cr_r["CONSIGNEE_FIRM"] = Convert.ToString(readerB_cr_r["CONSIGNEE_FIRM"]);
                dr_cr_r["PUR_DESIG"] = Convert.ToString(readerB_cr_r["PUR_DESIG"]);
                dr_cr_r["PUR_CD"] = Convert.ToString(readerB_cr_r["PUR_CD"]);
                dr_cr_r["PUR_DEPT"] = Convert.ToString(readerB_cr_r["PUR_DEPT"]);
                dr_cr_r["PUR_FIRM"] = Convert.ToString(readerB_cr_r["PUR_FIRM"]);
                dr_cr_r["PUR_City"] = Convert.ToString(readerB_cr_r["PUR_City"]);
                dr_cr_r["ITEM_SRNO_PO"] = Convert.ToString(readerB_cr_r["ITEM_SRNO_PO"]);
                dr_cr_r["ITEM_DESC_PO"] = Convert.ToString(readerB_cr_r["ITEM_DESC_PO"]);
                dr_cr_r["UOM_S_DESC"] = Convert.ToString(readerB_cr_r["UOM_S_DESC"]);
                dr_cr_r["QTY_ORDERED"] = Convert.ToString(readerB_cr_r["QTY_ORDERED"]);
                dr_cr_r["CUM_QTY_PREV_OFFERED"] = Convert.ToString(readerB_cr_r["CUM_QTY_PREV_OFFERED"]);
                dr_cr_r["CUM_QTY_PREV_PASSED"] = Convert.ToString(readerB_cr_r["CUM_QTY_PREV_PASSED"]);
                dr_cr_r["QTY_TO_INSP"] = Convert.ToString(readerB_cr_r["QTY_TO_INSP"]);
                dr_cr_r["QTY_PASSED"] = Convert.ToString(readerB_cr_r["QTY_PASSED"]);
                dr_cr_r["QTY_REJECTED"] = Convert.ToString(readerB_cr_r["QTY_REJECTED"]);
                dr_cr_r["QTY_DUE"] = Convert.ToString(readerB_cr_r["QTY_DUE"]);
                dr_cr_r["HOLOGRAM"] = Convert.ToString(readerB_cr_r["HOLOGRAM"]);
                dr_cr_r["NUM_VISITS"] = Convert.ToString(readerB_cr_r["NUM_VISITS"]);
                dr_cr_r["VISIT_DATES"] = Convert.ToString(readerB_cr_r["VISIT_DATES"]);
                dr_cr_r["BPO_NAME"] = Convert.ToString(readerB_cr_r["BPO_NAME"]);
                dr_cr_r["BPO_ORGN"] = Convert.ToString(readerB_cr_r["BPO_ORGN"]);
                dr_cr_r["City"] = Convert.ToString(readerB_cr_r["City"]);
                dr_cr_r["HOLOGRAMORG"] = Convert.ToString(readerB_cr_r["HOLOGRAMORG"]);
                dr_cr_r["REMARK"] = Convert.ToString(readerB_cr_r["REMARK"]);
                dr_cr_r["DT_INSP_DESIRE"] = Convert.ToString(readerB_cr_r["DT_INSP_DESIRE"]);
                dr_cr_r["ITEM_REMARK"] = Convert.ToString(readerB_cr_r["ITEM_REMARK"]);
                dr_cr_r["AMENDMENT_1"] = Convert.ToString(readerB_cr_r["AMENDMENT_1"]);
                dr_cr_r["AMENDMENTDT_1"] = Convert.ToString(readerB_cr_r["AMENDMENTDT_1"]);
                dr_cr_r["AMENDMENT_2"] = Convert.ToString(readerB_cr_r["AMENDMENT_2"]);
                dr_cr_r["AMENDMENTDT_2"] = Convert.ToString(readerB_cr_r["AMENDMENTDT_2"]);
                dr_cr_r["AMENDMENT_3"] = Convert.ToString(readerB_cr_r["AMENDMENT_3"]);
                dr_cr_r["AMENDMENTDT_3"] = Convert.ToString(readerB_cr_r["AMENDMENTDT_3"]);
                dr_cr_r["AMENDMENT_4"] = Convert.ToString(readerB_cr_r["AMENDMENT_4"]);
                dr_cr_r["AMENDMENTDT_4"] = Convert.ToString(readerB_cr_r["AMENDMENTDT_4"]);
                dr_cr_r["BK_NO"] = Convert.ToString(readerB_cr_r["BK_NO"]);
                dr_cr_r["SET_NO"] = Convert.ToString(readerB_cr_r["SET_NO"]);
                dr_cr_r["VISITS_DATES"] = Convert.ToString(readerB_cr_r["VISITS_DATES"]);
                dr_cr_r["IE_STAMP_IMAGE"] = Convert.ToString(readerB_cr_r["IE_STAMP_IMAGE"]);
                dr_cr_r["IE_STAMP_IMAGE1"] = Convert.ToString(readerB_cr_r["IE_STAMP_IMAGE1"]);
                dr_cr_r["LAB_TST_RECT_DT"] = Convert.ToString(readerB_cr_r["LAB_TST_RECT_DT"]);
                dr_cr_r["PASSED_INST_NO"] = Convert.ToString(readerB_cr_r["PASSED_INST_NO"]);
                dr_cr_r["CONSIGNEE_DTL"] = Convert.ToString(readerB_cr_r["CONSIGNEE_DTL"]);
                dr_cr_r["BPO_DTL"] = Convert.ToString(readerB_cr_r["BPO_DTL"]);
                dr_cr_r["GOV_BILL_AUTH"] = Convert.ToString(readerB_cr_r["GOV_BILL_AUTH"]);
                dr_cr_r["PUR_DTL"] = Convert.ToString(readerB_cr_r["PUR_DTL"]);
                dr_cr_r["PUR_AUT_DTL"] = Convert.ToString(readerB_cr_r["PUR_AUT_DTL"]);
                dr_cr_r["OFF_INST_NO_DTL"] = Convert.ToString(readerB_cr_r["OFF_INST_NO_DTL"]);
                dr_cr_r["UNIT_DTL"] = Convert.ToString(readerB_cr_r["UNIT_DTL"]);
                dr_cr_r["DISPATCH_PACKING_NO"] = Convert.ToString(readerB_cr_r["DISPATCH_PACKING_NO"]);
                dr_cr_r["INVOICE_NO"] = Convert.ToString(readerB_cr_r["INVOICE_NO"]);
                dr_cr_r["NAME_OF_IE"] = Convert.ToString(readerB_cr_r["NAME_OF_IE"]);
                dr_cr_r["MAN_TYPE"] = Convert.ToString(readerB_cr_r["MAN_TYPE"]);
                dr_cr_r["CONSIGNEE_DESG"] = Convert.ToString(readerB_cr_r["CONSIGNEE_DESG"]);
                dr_cr_r["DATETIME"] = Convert.ToString(readerB_cr_r["DATETIME"]);
                dr_cr_r["CONSGN_CALL_STATUS"] = Convert.ToString(readerB_cr_r["CONSGN_CALL_STATUS"]);
                dr_cr_r["pRegion"] = pregion_cr_r;
                dt.Rows.Add(dr_cr_r);
            }
            dsReports.Tables.Add(dt);
            return dsReports;

        }
    }
}