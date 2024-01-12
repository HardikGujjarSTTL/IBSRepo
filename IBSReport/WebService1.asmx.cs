using CrystalDecisions.CrystalReports.Engine;
using IBSReports.ReportClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace IBSReports
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)] // Optional for JSON response
        public string GetReportData(string CaseNO, string Call_Recv_Dt, string CallSNo, string Consignee_CD, string Region, string BkNo, string SetNo)
        {
            ReportDocument rd = new ReportDocument();
            DataSet dsCustom = new DataSet();

            rd = IC_RPT.IC_Report(CaseNO, Call_Recv_Dt, CallSNo, Consignee_CD, Region, BkNo, SetNo, out dsCustom);

            System.IO.Stream st = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            CopyStream(st, s);
            string pdf = Convert.ToBase64String(s.ToArray(), 0, s.ToArray().Length);

            //return pdf;
            return new JavaScriptSerializer().Serialize(pdf);
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
