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
        public IActionResult GetPassedBills(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string token = CallAuthenticate();
                string apiUrl = "https://ireps.gov.in/immsapi/rites/getAllBills";
                object input = new
                {
                    fromDate = FromDate,
                    toDate = ToDate,
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
                if (response != null && response.data.Count>0)
                {
                    List<PassedBillsModel> data = response.data;
                    if (data.Count != null)
                    {
                        foreach (PassedBillsModel model in data)
                        {
                            if (model != null)
                            {
                                OracleParameter[] par1 = new OracleParameter[21];
                                par1[0] = new OracleParameter("P_BILL_NO", OracleDbType.Varchar2, model.BILL_NO, ParameterDirection.Input);
                                par1[1] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, model.IC_NO, ParameterDirection.Input);
                                par1[2] = new OracleParameter("P_IC_DT", OracleDbType.Varchar2, model.IC_DT, ParameterDirection.Input);
                                par1[3] = new OracleParameter("P_INVOICENO", OracleDbType.Varchar2, model.INVOICENO, ParameterDirection.Input);
                                par1[4] = new OracleParameter("P_CO6_NO", OracleDbType.Varchar2, model.CO6_NO, ParameterDirection.Input);
                                par1[5] = new OracleParameter("P_CO6_DATE", OracleDbType.Varchar2, model.CO6_DATE, ParameterDirection.Input);
                                par1[6] = new OracleParameter("P_CO6_STATUS", OracleDbType.Varchar2, model.CO6_STATUS, ParameterDirection.Input);
                                par1[7] = new OracleParameter("P_CO6_STATUS_DATE", OracleDbType.Varchar2, model.CO6_STATUS_DATE, ParameterDirection.Input);
                                par1[8] = new OracleParameter("P_PASSED_AMT", OracleDbType.Varchar2, model.PASSED_AMT, ParameterDirection.Input);
                                par1[9] = new OracleParameter("P_DEDUCTED_AMT", OracleDbType.Varchar2, model.DEDUCTED_AMT, ParameterDirection.Input);
                                par1[10] = new OracleParameter("P_NET_AMT", OracleDbType.Varchar2, model.NET_AMT, ParameterDirection.Input);
                                par1[11] = new OracleParameter("P_BOOKDATE", OracleDbType.Varchar2, model.BOOKDATE, ParameterDirection.Input);
                                par1[12] = new OracleParameter("P_RETURN_REASON", OracleDbType.Varchar2, model.RETURN_REASON, ParameterDirection.Input);
                                par1[13] = new OracleParameter("P_RETURN_DATE", OracleDbType.Varchar2, model.RETURN_DATE, ParameterDirection.Input);
                                par1[14] = new OracleParameter("P_CO7_NO", OracleDbType.Varchar2, model.CO7_NO, ParameterDirection.Input);
                                par1[15] = new OracleParameter("P_CO7_DATE", OracleDbType.Varchar2, model.CO7_DATE, ParameterDirection.Input);
                                par1[16] = new OracleParameter("P_PAYMENT_DT", OracleDbType.Varchar2, model.PAYMENT_DT, ParameterDirection.Input);
                                par1[17] = new OracleParameter("P_BILL_RESENT_COUNT", OracleDbType.Varchar2, model.BILL_RESENT_COUNT, ParameterDirection.Input);
                                par1[18] = new OracleParameter("P_IRFC_FUNDED", OracleDbType.Varchar2, model.IRFC_FUNDED, ParameterDirection.Input);
                                par1[19] = new OracleParameter("P_INVOICE_SUPP_DOCS", OracleDbType.Varchar2, model.INVOICE_SUPP_DOCS, ParameterDirection.Input);
                                par1[20] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                                var ds = DataAccessDB.GetDataSet("SP_INSERT_Get_Passed_Bills_FROMAPI", par1, 2);
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
        public IActionResult GetAllBills(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                string token = CallAuthenticate();
                string apiUrl = "https://ireps.gov.in/immsapi/rites/getPassedBills";
                object input = new
                {
                    fromDate = FromDate,
                    toDate = ToDate,
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
                    if (data.Count != null)
                    {
                        foreach (AllBillsModel model in data)
                        {
                            if (model != null)
                            {
                                //OracleParameter[] par1 = new OracleParameter[21];
                                //par1[0] = new OracleParameter("P_BILL_NO", OracleDbType.Varchar2, model.BILL_NO, ParameterDirection.Input);
                                //par1[1] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, model.IC_NO, ParameterDirection.Input);
                                //par1[2] = new OracleParameter("P_IC_DT", OracleDbType.Varchar2, model.IC_DT, ParameterDirection.Input);
                                //par1[3] = new OracleParameter("P_INVOICENO", OracleDbType.Varchar2, model.INVOICENO, ParameterDirection.Input);
                                //par1[4] = new OracleParameter("P_CO6_NO", OracleDbType.Varchar2, model.CO6_NO, ParameterDirection.Input);
                                //par1[5] = new OracleParameter("P_CO6_DATE", OracleDbType.Varchar2, model.CO6_DATE, ParameterDirection.Input);
                                //par1[6] = new OracleParameter("P_CO6_STATUS", OracleDbType.Varchar2, model.CO6_STATUS, ParameterDirection.Input);
                                //par1[7] = new OracleParameter("P_CO6_STATUS_DATE", OracleDbType.Varchar2, model.CO6_STATUS_DATE, ParameterDirection.Input);
                                //par1[8] = new OracleParameter("P_PASSED_AMT", OracleDbType.Varchar2, model.PASSED_AMT, ParameterDirection.Input);
                                //par1[9] = new OracleParameter("P_DEDUCTED_AMT", OracleDbType.Varchar2, model.DEDUCTED_AMT, ParameterDirection.Input);
                                //par1[10] = new OracleParameter("P_NET_AMT", OracleDbType.Varchar2, model.NET_AMT, ParameterDirection.Input);
                                //par1[11] = new OracleParameter("P_BOOKDATE", OracleDbType.Varchar2, model.BOOKDATE, ParameterDirection.Input);
                                //par1[12] = new OracleParameter("P_RETURN_REASON", OracleDbType.Varchar2, model.RETURN_REASON, ParameterDirection.Input);
                                //par1[13] = new OracleParameter("P_RETURN_DATE", OracleDbType.Varchar2, model.RETURN_DATE, ParameterDirection.Input);
                                //par1[14] = new OracleParameter("P_CO7_NO", OracleDbType.Varchar2, model.CO7_NO, ParameterDirection.Input);
                                //par1[15] = new OracleParameter("P_CO7_DATE", OracleDbType.Varchar2, model.CO7_DATE, ParameterDirection.Input);
                                //par1[16] = new OracleParameter("P_PAYMENT_DT", OracleDbType.Varchar2, model.PAYMENT_DT, ParameterDirection.Input);
                                //par1[17] = new OracleParameter("P_BILL_RESENT_COUNT", OracleDbType.Varchar2, model.BILL_RESENT_COUNT, ParameterDirection.Input);
                                //par1[18] = new OracleParameter("P_IRFC_FUNDED", OracleDbType.Varchar2, model.IRFC_FUNDED, ParameterDirection.Input);
                                //par1[19] = new OracleParameter("P_INVOICE_SUPP_DOCS", OracleDbType.Varchar2, model.INVOICE_SUPP_DOCS, ParameterDirection.Input);
                                //par1[20] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                                //var ds = DataAccessDB.GetDataSet("SP_INSERT_Get_AllBills_FROMAPI", par1, 2);
                                //// need to create new SP for SP_INSERT_Get_AllBills_FROMAPI
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
