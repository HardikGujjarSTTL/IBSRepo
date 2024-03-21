using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using IBS.Helper;
using System.Data;

namespace IBS.Controllers.CallAPI
{
    public class BillsAPIController : Controller
    {
        public IActionResult index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetPassedBills(string FromDate, string ToDate)
        {
            try
            {
                string token = CallAuthenticate();
                string apiUrl = "https://ireps.gov.in/immsapi/rites/getPassedBills";
                object input = new
                {
                    fromDate = Convert.ToDateTime(FromDate),
                    toDate = Convert.ToDateTime(ToDate),
                };

                SyncWebClient client = new SyncWebClient();
                client.Headers["Method"] = "POST";

                client.Headers["Content-type"] = "application/json";
                string inputJson = JsonConvert.SerializeObject(input);
                client.Encoding = Encoding.UTF8;
                client.Headers["Authorization"] = "Bearer " + token;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate (
                        object s,
                        X509Certificate certificate,
                        X509Chain chain,
                        SslPolicyErrors sslPolicyErrors
                    )
                    {
                        return true;
                    };

                string json = client.UploadString(apiUrl, inputJson);

                PassedBillsResponse response = JsonConvert.DeserializeObject<PassedBillsResponse>(json);
                if (response != null && response.data.Count > 0)
                {
                    List<PassedBillsModel> data = response.data;
                    if (data.Count > 0)
                    {
                        foreach (PassedBillsModel model in data)
                        {
                            if (model != null)
                            {
                                OracleParameter[] par1 = new OracleParameter[21];
                                par1[0] = new OracleParameter("P_BILL_NO", OracleDbType.Varchar2, model.BILL_NO, ParameterDirection.Input);
                                par1[1] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, model.IC_NO, ParameterDirection.Input);
                                par1[2] = new OracleParameter("P_IC_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.IC_DT) ? (object)Convert.ToDateTime(model.IC_DT) : DBNull.Value, ParameterDirection.Input);
                                par1[3] = new OracleParameter("P_INVOICENO", OracleDbType.Varchar2, model.INVOICENO, ParameterDirection.Input);
                                par1[4] = new OracleParameter("P_CO6_NO", OracleDbType.Varchar2, model.CO6_NO, ParameterDirection.Input);
                                par1[5] = new OracleParameter("P_CO6_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_DATE) ? (object)Convert.ToDateTime(model.CO6_DATE) : DBNull.Value, ParameterDirection.Input);
                                par1[6] = new OracleParameter("P_CO6_STATUS", OracleDbType.Varchar2, model.CO6_STATUS, ParameterDirection.Input);
                                par1[7] = new OracleParameter("P_CO6_STATUS_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_STATUS_DATE) ? (object)Convert.ToDateTime(model.CO6_STATUS_DATE) : DBNull.Value, ParameterDirection.Input);
                                par1[8] = new OracleParameter("P_PASSED_AMT", OracleDbType.Varchar2, model.PASSED_AMT, ParameterDirection.Input);
                                par1[9] = new OracleParameter("P_DEDUCTED_AMT", OracleDbType.Varchar2, model.DEDUCTED_AMT, ParameterDirection.Input);
                                par1[10] = new OracleParameter("P_NET_AMT", OracleDbType.Varchar2, model.NET_AMT, ParameterDirection.Input);
                                par1[11] = new OracleParameter("P_BOOKDATE", OracleDbType.Date, !string.IsNullOrEmpty(model.BOOKDATE) ? (object)Convert.ToDateTime(model.BOOKDATE) : DBNull.Value, ParameterDirection.Input);
                                par1[12] = new OracleParameter("P_RETURN_REASON", OracleDbType.Varchar2, model.RETURN_REASON, ParameterDirection.Input);
                                par1[13] = new OracleParameter("P_RETURN_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.RETURN_DATE) ? (object)Convert.ToDateTime(model.RETURN_DATE) : DBNull.Value, ParameterDirection.Input);
                                par1[14] = new OracleParameter("P_CO7_NO", OracleDbType.Varchar2, model.CO7_NO, ParameterDirection.Input);
                                par1[15] = new OracleParameter("P_CO7_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO7_DATE) ? (object)Convert.ToDateTime(model.CO7_DATE) : DBNull.Value, ParameterDirection.Input);
                                par1[16] = new OracleParameter("P_PAYMENT_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.PAYMENT_DT) ? (object)Convert.ToDateTime(model.PAYMENT_DT) : DBNull.Value, ParameterDirection.Input);
                                par1[17] = new OracleParameter("P_BILL_RESENT_COUNT", OracleDbType.Varchar2, model.BILL_RESENT_COUNT, ParameterDirection.Input);
                                par1[18] = new OracleParameter("P_IRFC_FUNDED", OracleDbType.Varchar2, model.IRFC_FUNDED, ParameterDirection.Input);
                                par1[19] = new OracleParameter("P_INVOICE_SUPP_DOCS", OracleDbType.Varchar2, model.INVOICE_SUPP_DOCS, ParameterDirection.Input);
                                par1[20] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                                var ds = DataAccessDB.GetDataSet("SP_UPDATE_Get_Passed_Bills_FROMAPI", par1, 2);
                            }
                        }
                    }
                    return Json(new { status = true, responseText = "Successful." });
                }
                else
                {
                    return Json(new { status = true, responseText = "Data not found." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillsAPI", "GetPassedBills", 1, "");
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }
        [HttpGet]
        public IActionResult GetAllBills(string FromDate, string ToDate)
        {
            try
            {
                string token = CallAuthenticate();
                string apiUrl = "https://ireps.gov.in/immsapi/rites/getAllBills";
                object input = new
                {
                    fromDate = Convert.ToDateTime(FromDate),
                    toDate = Convert.ToDateTime(ToDate),
                };

                SyncWebClient client = new SyncWebClient();
                client.Headers["Method"] = "POST";

                client.Headers["Content-type"] = "application/json";
                string inputJson = JsonConvert.SerializeObject(input);
                client.Encoding = Encoding.UTF8;
                client.Headers["Authorization"] = "Bearer " + token;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate (
                        object s,
                        X509Certificate certificate,
                        X509Chain chain,
                        SslPolicyErrors sslPolicyErrors
                    )
                    {
                        return true;
                    };

                string json = client.UploadString(apiUrl, inputJson);

                AllBillsResponse response = JsonConvert.DeserializeObject<AllBillsResponse>(json);
                if (response != null && response.data.Count > 0)
                {
                    List<AllBillsModel> data = response.data;
                    if (data.Count > 0)
                    {
                        foreach (AllBillsModel model in data)
                        {
                            if (model != null)
                            {
                                OracleParameter[] parameters = new OracleParameter[95];
                                parameters[0] = new OracleParameter("P_BILL_NO", OracleDbType.Varchar2, model.BILL_NO, ParameterDirection.Input);
                                parameters[1] = new OracleParameter("P_RLY_CODE", OracleDbType.Varchar2, model.RLY_CODE, ParameterDirection.Input);
                                parameters[2] = new OracleParameter("P_INVOICEDATE", OracleDbType.Date, !string.IsNullOrEmpty(model.INVOICEDATE) ? (object)Convert.ToDateTime(model.INVOICEDATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[3] = new OracleParameter("P_BPO_CD", OracleDbType.Varchar2, model.BPO_CD, ParameterDirection.Input);
                                parameters[4] = new OracleParameter("P_BPO_TYPE", OracleDbType.Varchar2, model.BPO_TYPE, ParameterDirection.Input);
                                parameters[5] = new OracleParameter("P_BPO_RLY", OracleDbType.Varchar2, model.BPO_RLY, ParameterDirection.Input);
                                parameters[6] = new OracleParameter("P_BPO_NAME", OracleDbType.Varchar2, model.BPO_NAME, ParameterDirection.Input);
                                parameters[7] = new OracleParameter("P_BPO_ORGN", OracleDbType.Varchar2, model.BPO_ORGN, ParameterDirection.Input);
                                parameters[8] = new OracleParameter("P_BPO_ADD", OracleDbType.Varchar2, model.BPO_ADD, ParameterDirection.Input);
                                parameters[9] = new OracleParameter("P_BPO_CITY", OracleDbType.Varchar2, model.BPO_CITY, ParameterDirection.Input);
                                parameters[10] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, model.CASE_NO, ParameterDirection.Input);
                                parameters[11] = new OracleParameter("P_REGION_CODE", OracleDbType.Varchar2, model.REGION_CODE, ParameterDirection.Input);
                                parameters[12] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, model.PO_NO, ParameterDirection.Input);
                                parameters[13] = new OracleParameter("P_PO_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.PO_DT) ? (object)Convert.ToDateTime(model.PO_DT) : DBNull.Value, ParameterDirection.Input);
                                parameters[14] = new OracleParameter("P_VEND_CD", OracleDbType.Varchar2, model.VEND_CD, ParameterDirection.Input);
                                parameters[15] = new OracleParameter("P_VEND_NAME", OracleDbType.Varchar2, model.VEND_NAME, ParameterDirection.Input);
                                parameters[16] = new OracleParameter("P_VEND_ADD1", OracleDbType.Varchar2, model.VEND_ADD1, ParameterDirection.Input);
                                parameters[17] = new OracleParameter("P_VEND_ADD2", OracleDbType.Varchar2, model.VEND_ADD2, ParameterDirection.Input);
                                parameters[18] = new OracleParameter("P_VENDOR_CITY", OracleDbType.Varchar2, model.VENDOR_CITY, ParameterDirection.Input);
                                parameters[19] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Varchar2, model.CONSIGNEE_CD, ParameterDirection.Input);
                                parameters[20] = new OracleParameter("P_CONSIGNEE", OracleDbType.Varchar2, model.CONSIGNEE, ParameterDirection.Input);
                                parameters[21] = new OracleParameter("P_CONSIGNEE_ADD1", OracleDbType.Varchar2, model.CONSIGNEE_ADD1, ParameterDirection.Input);
                                parameters[22] = new OracleParameter("P_CONSIGNEE_ADD2", OracleDbType.Varchar2, model.CONSIGNEE_ADD2, ParameterDirection.Input);
                                parameters[23] = new OracleParameter("P_CONSIGNEE_CITY", OracleDbType.Varchar2, model.CONSIGNEE_CITY, ParameterDirection.Input);
                                parameters[24] = new OracleParameter("P_IE_CD", OracleDbType.Varchar2, model.IE_CD, ParameterDirection.Input);
                                parameters[25] = new OracleParameter("P_IE_CO_CD", OracleDbType.Varchar2, model.IE_CO_CD, ParameterDirection.Input);
                                parameters[26] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, model.IC_NO, ParameterDirection.Input);
                                parameters[27] = new OracleParameter("P_IC_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.IC_DT) ? (object)Convert.ToDateTime(model.IC_DT) : DBNull.Value, ParameterDirection.Input);
                                parameters[28] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, model.BK_NO, ParameterDirection.Input);
                                parameters[29] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, model.SET_NO, ParameterDirection.Input);
                                parameters[30] = new OracleParameter("P_CALL_INSTALMENT_NO", OracleDbType.Varchar2, model.CALL_INSTALMENT_NO, ParameterDirection.Input);
                                parameters[31] = new OracleParameter("P_MATERIAL_VALUE", OracleDbType.Varchar2, model.MATERIAL_VALUE, ParameterDirection.Input);
                                parameters[32] = new OracleParameter("P_VISITS", OracleDbType.Varchar2, model.VISITS, ParameterDirection.Input);
                                parameters[33] = new OracleParameter("P_GSTTAXABLEAMT", OracleDbType.Varchar2, model.GSTTAXABLEAMT, ParameterDirection.Input);
                                parameters[34] = new OracleParameter("P_CGSTAMT", OracleDbType.Varchar2, model.CGSTAMT, ParameterDirection.Input);
                                parameters[35] = new OracleParameter("P_SGSTAMT", OracleDbType.Varchar2, model.SGSTAMT, ParameterDirection.Input);
                                parameters[36] = new OracleParameter("P_IGSTAMT", OracleDbType.Varchar2, model.IGSTAMT, ParameterDirection.Input);
                                parameters[37] = new OracleParameter("P_AMOUNT", OracleDbType.Varchar2, model.AMOUNT, ParameterDirection.Input);
                                parameters[38] = new OracleParameter("P_INVOICENO", OracleDbType.Varchar2, model.INVOICENO, ParameterDirection.Input);
                                parameters[39] = new OracleParameter("P_RLYGSTIN", OracleDbType.Varchar2, model.RLYGSTIN, ParameterDirection.Input);
                                parameters[40] = new OracleParameter("P_PARTYGSTIN", OracleDbType.Varchar2, model.PARTYGSTIN, ParameterDirection.Input);
                                parameters[41] = new OracleParameter("P_PARTYCODE", OracleDbType.Varchar2, model.PARTYCODE, ParameterDirection.Input);
                                parameters[42] = new OracleParameter("P_PARTYNAME", OracleDbType.Varchar2, model.PARTYNAME, ParameterDirection.Input);
                                parameters[43] = new OracleParameter("P_ITEM_SRNO", OracleDbType.Varchar2, model.ITEM_SRNO, ParameterDirection.Input);
                                parameters[44] = new OracleParameter("P_ITEMDESC", OracleDbType.Varchar2, model.ITEMDESC, ParameterDirection.Input);
                                parameters[45] = new OracleParameter("P_QTY", OracleDbType.Varchar2, model.QTY, ParameterDirection.Input);
                                parameters[46] = new OracleParameter("P_RATE", OracleDbType.Varchar2, model.RATE, ParameterDirection.Input);
                                parameters[47] = new OracleParameter("P_UNITCODE", OracleDbType.Varchar2, model.UNITCODE, ParameterDirection.Input);
                                parameters[48] = new OracleParameter("P_UOM_FACTOR", OracleDbType.Varchar2, model.UOM_FACTOR, ParameterDirection.Input);
                                parameters[49] = new OracleParameter("P_BASIC_VALUE", OracleDbType.Varchar2, model.BASIC_VALUE, ParameterDirection.Input);
                                parameters[50] = new OracleParameter("P_VALUE", OracleDbType.Varchar2, model.VALUE, ParameterDirection.Input);
                                parameters[51] = new OracleParameter("P_PDFFILE", OracleDbType.Varchar2, model.PDFFILE, ParameterDirection.Input);
                                parameters[52] = new OracleParameter("P_BILLDESC", OracleDbType.Varchar2, model.BILLDESC, ParameterDirection.Input);
                                parameters[53] = new OracleParameter("P_PARTYSTATE", OracleDbType.Varchar2, model.PARTYSTATE, ParameterDirection.Input);
                                parameters[54] = new OracleParameter("P_REVERSECHARGE", OracleDbType.Varchar2, model.REVERSECHARGE, ParameterDirection.Input);
                                parameters[55] = new OracleParameter("P_ISGSTREGISTERED", OracleDbType.Varchar2, model.ISGSTREGISTERED, ParameterDirection.Input);
                                parameters[56] = new OracleParameter("P_GSTTDSDEDUCTION", OracleDbType.Varchar2, model.GSTTDSDEDUCTION, ParameterDirection.Input);
                                parameters[57] = new OracleParameter("P_COMPOSITETAXABLE", OracleDbType.Varchar2, model.COMPOSITETAXABLE, ParameterDirection.Input);
                                parameters[58] = new OracleParameter("P_HSNSAC", OracleDbType.Varchar2, model.HSNSAC, ParameterDirection.Input);
                                parameters[59] = new OracleParameter("P_HSNSACCODE", OracleDbType.Varchar2, model.HSNSACCODE, ParameterDirection.Input);
                                parameters[60] = new OracleParameter("P_ITCELIGIBLE", OracleDbType.Varchar2, model.ITCELIGIBLE, ParameterDirection.Input);
                                parameters[61] = new OracleParameter("P_SGSTRATE", OracleDbType.Varchar2, model.SGSTRATE, ParameterDirection.Input);
                                parameters[62] = new OracleParameter("P_CGSTRATE", OracleDbType.Varchar2, model.CGSTRATE, ParameterDirection.Input);
                                parameters[63] = new OracleParameter("P_UGSTRATE", OracleDbType.Varchar2, model.UGSTRATE, ParameterDirection.Input);
                                parameters[64] = new OracleParameter("P_UGSTAMT", OracleDbType.Varchar2, model.UGSTAMT, ParameterDirection.Input);
                                parameters[65] = new OracleParameter("P_IGSTRATE", OracleDbType.Varchar2, model.IGSTRATE, ParameterDirection.Input);
                                parameters[66] = new OracleParameter("P_STATESUPPLY", OracleDbType.Varchar2, model.STATESUPPLY, ParameterDirection.Input);
                                parameters[67] = new OracleParameter("P_RECV_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.RECV_DATE) ? (object)Convert.ToDateTime(model.RECV_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[68] = new OracleParameter("P_UPD_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.UPD_DATE) ? (object)Convert.ToDateTime(model.UPD_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[69] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, model.STATUS, ParameterDirection.Input);
                                parameters[70] = new OracleParameter("P_CO6_NO", OracleDbType.Varchar2, model.CO6_NO, ParameterDirection.Input);
                                parameters[71] = new OracleParameter("P_CO6_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_DATE) ? (object)Convert.ToDateTime(model.CO6_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[72] = new OracleParameter("P_CO6_STATUS", OracleDbType.Varchar2, model.CO6_STATUS, ParameterDirection.Input);
                                parameters[73] = new OracleParameter("P_CO6_STATUS_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_STATUS_DATE) ? (object)Convert.ToDateTime(model.CO6_STATUS_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[74] = new OracleParameter("P_PASSED_AMT", OracleDbType.Varchar2, model.PASSED_AMT, ParameterDirection.Input);
                                parameters[75] = new OracleParameter("P_DEDUCTED_AMT", OracleDbType.Varchar2, model.DEDUCTED_AMT, ParameterDirection.Input);
                                parameters[76] = new OracleParameter("P_NET_AMT", OracleDbType.Varchar2, model.NET_AMT, ParameterDirection.Input);
                                parameters[77] = new OracleParameter("P_BOOKDATE", OracleDbType.Date, !string.IsNullOrEmpty(model.BOOKDATE) ? (object)Convert.ToDateTime(model.BOOKDATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[78] = new OracleParameter("P_RETURN_REASON", OracleDbType.Varchar2, model.RETURN_REASON, ParameterDirection.Input);
                                parameters[79] = new OracleParameter("P_RETURN_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.RETURN_DATE) ? (object)Convert.ToDateTime(model.RETURN_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[80] = new OracleParameter("P_PDF_LINK", OracleDbType.Varchar2, model.PDF_LINK, ParameterDirection.Input);
                                parameters[81] = new OracleParameter("P_PAYMENT_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.PAYMENT_DT) ? (object)Convert.ToDateTime(model.PAYMENT_DT) : DBNull.Value, ParameterDirection.Input);
                                parameters[82] = new OracleParameter("P_CO7_NO", OracleDbType.Varchar2, model.CO7_NO, ParameterDirection.Input);
                                parameters[83] = new OracleParameter("P_CO7_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO7_DATE) ? (object)Convert.ToDateTime(model.CO7_DATE) : DBNull.Value, ParameterDirection.Input);
                                parameters[84] = new OracleParameter("P_BANK_ACCT_NO", OracleDbType.Varchar2, model.BANK_ACCT_NO, ParameterDirection.Input);
                                parameters[85] = new OracleParameter("P_IFSCCODE", OracleDbType.Varchar2, model.IFSCCODE, ParameterDirection.Input);
                                parameters[86] = new OracleParameter("P_BANK_NAME", OracleDbType.Varchar2, model.BANK_NAME, ParameterDirection.Input);
                                parameters[87] = new OracleParameter("P_BANKADDRESS", OracleDbType.Varchar2, model.BANKADDRESS, ParameterDirection.Input);
                                parameters[88] = new OracleParameter("P_IC_PDF", OracleDbType.Varchar2, model.IC_PDF, ParameterDirection.Input);
                                parameters[89] = new OracleParameter("P_INVOICE_PDF", OracleDbType.Varchar2, model.INVOICE_PDF, ParameterDirection.Input);
                                parameters[90] = new OracleParameter("P_AU", OracleDbType.Varchar2, model.AU, ParameterDirection.Input);
                                parameters[91] = new OracleParameter("P_BILL_RESENT_COUNT", OracleDbType.Varchar2, model.BILL_RESENT_COUNT, ParameterDirection.Input);
                                parameters[92] = new OracleParameter("P_IRFC_FUNDED", OracleDbType.Varchar2, model.IRFC_FUNDED, ParameterDirection.Input);
                                parameters[93] = new OracleParameter("P_INVOICE_SUPP_DOCS", OracleDbType.Varchar2, model.INVOICE_SUPP_DOCS, ParameterDirection.Input);
                                parameters[94] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                                var ds = DataAccessDB.GetDataSet("SP_INSERT_Get_AllBills_FROMAPI", parameters, 2);
                                
                            }
                        }
                    }
                    return Json(new { status = true, responseText = "Successful." });
                }
                else
                {
                    return Json(new { status = true, responseText = "Data not found." });
                }
            }
            catch (Exception ex)
            {
                Common.AddException(ex.ToString(), ex.Message.ToString(), "BillsAPI", "GetPassedBills", 1, "");
            }
            return Json(new { status = false, responseText = "Oops Somthing Went Wrong !!" });
        }

        private class SyncWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest w = base.GetWebRequest(uri);
                w.Timeout = 20 * 60 * 1000;
                return w;
            }
        }
        public string CallAuthenticate()
        {
            string apiUrl = "https://trial.ireps.gov.in/immsapi/authenticate";

            object input = new
            {
                username = "rites",
                password = "password"
            };

            SyncWebClient client = new SyncWebClient();
            client.Headers["Method"] = "POST";

            client.Headers["Content-type"] = "application/json";

            string inputJson = JsonConvert.SerializeObject(input);
            client.Encoding = Encoding.UTF8;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (
                    object s,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors
                )
                {
                    return true;
                };

            string json = client.UploadString(apiUrl, inputJson);
            if (json != null)
            {
                BillsAPIModel model = JsonConvert.DeserializeObject<BillsAPIModel>(json);
                return model.token;
            }

            return "";
        }
    }
}
