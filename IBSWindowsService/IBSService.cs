using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using IBSWindowsService.Models;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Reflection;

namespace IBSWindowsService
{
    public partial class IBSService : ServiceBase
    {
        System.Timers.Timer webserviceTime_VeryHighPriority_Interval = new System.Timers.Timer();

        string connectionString = ConfigurationManager.ConnectionStrings["SysNotificationsEntities"].ConnectionString;
        int VeryHighPriority_Interval = ConfigurationManager.AppSettings["VeryHighPriority_Interval"] == null ? 120 : Convert.ToInt32(ConfigurationManager.AppSettings["VeryHighPriority_Interval"].ToString());

        public IBSService()
        {

            string token = CallAuthenticate();
            GetPOList(token);
            //GetPOMAList(token);
            //GetBillsStatus(token);

            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            webserviceTime_VeryHighPriority_Interval.Enabled = true;
            webserviceTime_VeryHighPriority_Interval.Interval = (1000 * VeryHighPriority_Interval);
            //webserviceTime_VeryHighPriority_Interval.Elapsed += new System.Timers.ElapsedEventHandler(GetPOList);
            webserviceTime_VeryHighPriority_Interval.Start();

        }
        protected override void OnStop()
        {

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
            //client.Headers["User-Agent"] = "PostmanRuntime/7.29.0";
            //client.Headers["Accept"] = "*/*";
            //client.Headers["Accept-Encoding"] = "gzip, deflate, br";

            string inputJson = JsonConvert.SerializeObject(input);
            client.Encoding = Encoding.UTF8;
            //client.Headers["Authorization"] = ConfigurationManager.AppSettings.Get("Authorization");
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
                AuthenticateModel model = JsonConvert.DeserializeObject<AuthenticateModel>(json);
                return model.token;
            }
            return "";
        }

        public void GetPOList(string token)
        {
            string apiUrl = "https://ireps.gov.in/immsapi/purchase/getPOList";

            object input = new
            {
                poDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"),
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

            POResponse response = JsonConvert.DeserializeObject<POResponse>(json);
            if (response != null && response.data != null)
            {
                List<POModel> data = response.data;
                if (data != null)
                {
                    foreach (POModel model in data)
                    {
                        if (model != null)
                        {
                            // INSERT Data Start
                            OracleParameter[] par = new OracleParameter[3];
                            par[0] = new OracleParameter("P_RLY", OracleDbType.Varchar2, model.RLY, ParameterDirection.Input);
                            par[1] = new OracleParameter("P_POKEY", OracleDbType.Varchar2, model.POKEY, ParameterDirection.Input);
                            par[2] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                            var ds = GetDataSet("SP_INSERT_POHDR_FROMAPI", par, 1);
                            // INSERT Data End

                            string apiUrl1 = "https://ireps.gov.in/immsapi/purchase/getPODetails";
                            object input1 = new
                            {
                                rly = model.RLY,
                                poKey = model.POKEY
                            };

                            SyncWebClient client1 = new SyncWebClient();
                            client1.Headers["Method"] = "POST";

                            client1.Headers["Content-type"] = "application/json";
                            string inputJson1 = JsonConvert.SerializeObject(input1);
                            client1.Encoding = Encoding.UTF8;
                            client1.Headers["Authorization"] = "Bearer " + token;
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

                            string json1 = client1.UploadString(apiUrl1, inputJson1);
                            PoDetailResponse response1 = JsonConvert.DeserializeObject<PoDetailResponse>(json1);

                            if (response1 != null && response1.data != null && response1.data.PoDtl != null)
                            {
                                List<PoDetail> pODetailModels = response1.data.PoDtl;
                                PoHdr pOHdr = response1.data.PoHdr;
                                if (pODetailModels != null)
                                {
                                    foreach (var item in pODetailModels)
                                    {
                                        // PoDetail Save logic
                                        // INSERT Data Start
                                        OracleParameter[] par1 = new OracleParameter[32];
                                        par1[0] = new OracleParameter("P_RLY", OracleDbType.Varchar2, item.RLY, ParameterDirection.Input);
                                        par1[1] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, item.CASE_NO, ParameterDirection.Input);
                                        par1[2] = new OracleParameter("P_PL_NO", OracleDbType.Varchar2, item.PL_NO, ParameterDirection.Input);
                                        par1[3] = new OracleParameter("P_ITEM_SRNO", OracleDbType.Varchar2, item.ITEM_SRNO, ParameterDirection.Input);
                                        par1[4] = new OracleParameter("P_ITEM_DESC", OracleDbType.Varchar2, item.ITEM_DESC, ParameterDirection.Input);
                                        par1[5] = new OracleParameter("P_CONSIGNEE_CD", OracleDbType.Varchar2, item.CONSIGNEE_CD, ParameterDirection.Input);
                                        par1[6] = new OracleParameter("P_IMMS_CONSIGNEE_CD", OracleDbType.Varchar2, item.IMMS_CONSIGNEE_CD, ParameterDirection.Input);
                                        par1[7] = new OracleParameter("P_IMMS_CONSIGNEE_NAME", OracleDbType.Varchar2, item.IMMS_CONSIGNEE_NAME, ParameterDirection.Input);
                                        par1[8] = new OracleParameter("P_CONSIGNEE_DETAIL", OracleDbType.Varchar2, item.CONSIGNEE_DETAIL, ParameterDirection.Input);
                                        par1[9] = new OracleParameter("P_QTY", OracleDbType.Varchar2, item.QTY, ParameterDirection.Input);
                                        par1[10] = new OracleParameter("P_QTY_CANCELLED", OracleDbType.Varchar2, item.QTY_CANCELLED, ParameterDirection.Input);
                                        par1[11] = new OracleParameter("P_RATE", OracleDbType.Varchar2, item.RATE, ParameterDirection.Input);
                                        par1[12] = new OracleParameter("P_UOM_CD", OracleDbType.Varchar2, item.UOM_CD, ParameterDirection.Input);
                                        par1[13] = new OracleParameter("P_UOM", OracleDbType.Varchar2, item.UOM, ParameterDirection.Input);
                                        par1[14] = new OracleParameter("P_BASIC_VALUE", OracleDbType.Varchar2, item.BASIC_VALUE, ParameterDirection.Input);
                                        par1[15] = new OracleParameter("P_SALES_TAX_PER", OracleDbType.Varchar2, item.SALES_TAX_PER, ParameterDirection.Input);
                                        par1[16] = new OracleParameter("P_SALES_TAX", OracleDbType.Varchar2, item.SALES_TAX, ParameterDirection.Input);
                                        par1[17] = new OracleParameter("P_EXCISE_TYPE", OracleDbType.Varchar2, item.EXCISE_TYPE, ParameterDirection.Input);
                                        par1[18] = new OracleParameter("P_EXCISE_PER", OracleDbType.Varchar2, item.EXCISE_PER, ParameterDirection.Input);
                                        par1[19] = new OracleParameter("P_DISCOUNT_TYPE", OracleDbType.Varchar2, item.DISCOUNT_TYPE, ParameterDirection.Input);
                                        par1[20] = new OracleParameter("P_DISCOUNT_PER", OracleDbType.Varchar2, item.DISCOUNT_PER, ParameterDirection.Input);
                                        par1[21] = new OracleParameter("P_DISCOUNT", OracleDbType.Varchar2, item.DISCOUNT, ParameterDirection.Input);
                                        par1[22] = new OracleParameter("P_OT_CHARGE_TYPE", OracleDbType.Varchar2, item.OT_CHARGE_TYPE, ParameterDirection.Input);
                                        par1[23] = new OracleParameter("P_OT_CHARGE_PER", OracleDbType.Varchar2, item.OT_CHARGE_PER, ParameterDirection.Input);
                                        par1[24] = new OracleParameter("P_OTHER_CHARGES", OracleDbType.Varchar2, item.OTHER_CHARGES, ParameterDirection.Input);
                                        par1[25] = new OracleParameter("P_VALUE", OracleDbType.Varchar2, item.VALUE, ParameterDirection.Input);
                                        par1[26] = new OracleParameter("P_DELV_DT", OracleDbType.Date, item.DELV_DT != null ? (object)Convert.ToDateTime(item.DELV_DT) : DBNull.Value, ParameterDirection.Input);
                                        par1[27] = new OracleParameter("P_EXT_DELV_DT", OracleDbType.Date, item.EXT_DELV_DT != null ? (object)Convert.ToDateTime(item.EXT_DELV_DT) : DBNull.Value, ParameterDirection.Input);
                                        par1[28] = new OracleParameter("P_USER_ID", OracleDbType.Varchar2, item.USER_ID, ParameterDirection.Input);
                                        par1[29] = new OracleParameter("P_DATETIME", OracleDbType.Date, item.DATETIME != null ? (object)Convert.ToDateTime(item.DATETIME) : DBNull.Value, ParameterDirection.Input);
                                        par1[30] = new OracleParameter("P_ALLOCATION", OracleDbType.Varchar2, item.ALLOCATION, ParameterDirection.Input);
                                        par1[31] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                                        var ds1 = GetDataSet("SP_INSERT_POHDRDETAIL_FROMAPI", par1, 1);

                                        // INSERT Data End
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public void GetPOMAList(string token)
        {
            string apiUrl = "https://ireps.gov.in/immsapi/purchase/getPoMaList";

            object input = new
            {
                poMaDate = DateTime.Now.ToString("yyyy-MM-dd"),
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
            POMAResponse response = JsonConvert.DeserializeObject<POMAResponse>(json);
            if (response != null && response.data != null)
            {
                List<POMAModel> data = response.data;
                if (data != null)
                {
                    foreach (POMAModel model in data)
                    {
                        if (model != null)
                        {
                            string apiUrl1 = "https://ireps.gov.in/immsapi/purchase/getPoMaDetails";

                            object input1 = new
                            {
                                rly = model.RLY,
                                maKey = model.MAKEY
                            };

                            SyncWebClient client1 = new SyncWebClient();
                            client1.Headers["Method"] = "POST";

                            client1.Headers["Content-type"] = "application/json";
                            string inputJson1 = JsonConvert.SerializeObject(input1);
                            client1.Encoding = Encoding.UTF8;
                            client1.Headers["Authorization"] = "Bearer " + token;
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

                            string json1 = client1.UploadString(apiUrl1, inputJson1);
                            MMP_POMA_Response response1 = JsonConvert.DeserializeObject<MMP_POMA_Response>(json1);

                            if (response1 != null && response1.data != null && response1.data.MMP_POMA_DTL != null)
                            {
                                List<MMP_POMA_DTL> pODetailModels = response1.data.MMP_POMA_DTL;
                                MMP_POMA_HDR pOHdr = response1.data.MMP_POMA_HDR;

                                if (pOHdr != null)
                                {
                                    // MA Po Save logic
                                    // INSERT Data Start
                                    OracleParameter[] par = new OracleParameter[36];
                                    par[0] = new OracleParameter("P_RLY", OracleDbType.Varchar2, pOHdr.RLY, ParameterDirection.Input);
                                    par[1] = new OracleParameter("P_MAKEY", OracleDbType.Varchar2, pOHdr.MAKEY, ParameterDirection.Input);
                                    par[2] = new OracleParameter("P_MAKEY_DATE", OracleDbType.Date, !string.IsNullOrEmpty(pOHdr.MAKEY_DATE) ? (object)Convert.ToDateTime(pOHdr.MAKEY_DATE) : DBNull.Value, ParameterDirection.Input);
                                    par[3] = new OracleParameter("P_POKEY", OracleDbType.Varchar2, pOHdr.POKEY, ParameterDirection.Input);
                                    par[4] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, pOHdr.PO_NO, ParameterDirection.Input);
                                    par[5] = new OracleParameter("P_MA_NO", OracleDbType.Varchar2, pOHdr.MA_NO, ParameterDirection.Input);
                                    par[6] = new OracleParameter("P_MA_DATE", OracleDbType.Date, !string.IsNullOrEmpty(pOHdr.MA_DATE) ? (object)Convert.ToDateTime(pOHdr.MA_DATE) : DBNull.Value, ParameterDirection.Input);
                                    par[7] = new OracleParameter("P_MA_TYPE", OracleDbType.Varchar2, pOHdr.MA_TYPE, ParameterDirection.Input);
                                    par[8] = new OracleParameter("P_VCODE", OracleDbType.Varchar2, pOHdr.VCODE, ParameterDirection.Input);
                                    par[9] = new OracleParameter("P_SUBJECT", OracleDbType.Varchar2, pOHdr.SUBJECT, ParameterDirection.Input);
                                    par[10] = new OracleParameter("P_REF_NO", OracleDbType.Varchar2, pOHdr.REF_NO, ParameterDirection.Input);
                                    par[11] = new OracleParameter("P_REF_DATE", OracleDbType.Date, !string.IsNullOrEmpty(pOHdr.REF_DATE) ? (object)Convert.ToDateTime(pOHdr.REF_DATE) : DBNull.Value, ParameterDirection.Input);
                                    par[12] = new OracleParameter("P_REMARKS", OracleDbType.Varchar2, pOHdr.REMARKS, ParameterDirection.Input);
                                    par[13] = new OracleParameter("P_MA_SIGN_OFF", OracleDbType.Varchar2, pOHdr.MA_SIGN_OFF, ParameterDirection.Input);
                                    par[14] = new OracleParameter("P_REQUEST_ID", OracleDbType.Varchar2, pOHdr.REQUEST_ID, ParameterDirection.Input);
                                    par[15] = new OracleParameter("P_AUTH_SEQ", OracleDbType.Varchar2, pOHdr.AUTH_SEQ, ParameterDirection.Input);
                                    par[16] = new OracleParameter("P_AUTH_SEQ_FIN", OracleDbType.Varchar2, pOHdr.AUTH_SEQ_FIN, ParameterDirection.Input);
                                    par[17] = new OracleParameter("P_CURUSER", OracleDbType.Varchar2, pOHdr.CURUSER, ParameterDirection.Input);
                                    par[18] = new OracleParameter("P_CURUSER_IND", OracleDbType.Varchar2, pOHdr.CURUSER_IND, ParameterDirection.Input);
                                    par[19] = new OracleParameter("P_SIGN_ID", OracleDbType.Varchar2, pOHdr.SIGN_ID, ParameterDirection.Input);
                                    par[20] = new OracleParameter("P_REQ_ID", OracleDbType.Varchar2, pOHdr.REQ_ID, ParameterDirection.Input);
                                    par[21] = new OracleParameter("P_FIN_STATUS", OracleDbType.Varchar2, pOHdr.FIN_STATUS, ParameterDirection.Input);
                                    par[22] = new OracleParameter("P_REC_IND", OracleDbType.Varchar2, pOHdr.REC_IND, ParameterDirection.Input);
                                    par[23] = new OracleParameter("P_FLAG", OracleDbType.Varchar2, pOHdr.FLAG, ParameterDirection.Input);
                                    par[24] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, pOHdr.STATUS, ParameterDirection.Input);
                                    par[25] = new OracleParameter("P_PUR_DIV", OracleDbType.Varchar2, pOHdr.PUR_DIV, ParameterDirection.Input);
                                    par[26] = new OracleParameter("P_PUR_SEC", OracleDbType.Varchar2, pOHdr.PUR_SEC, ParameterDirection.Input);
                                    par[27] = new OracleParameter("P_OLD_PO_VALUE", OracleDbType.Varchar2, pOHdr.OLD_PO_VALUE, ParameterDirection.Input);
                                    par[28] = new OracleParameter("P_NEW_PO_VALUE", OracleDbType.Varchar2, pOHdr.NEW_PO_VALUE, ParameterDirection.Input);
                                    par[29] = new OracleParameter("P_PO_MA_SRNO", OracleDbType.Varchar2, pOHdr.PO_MA_SRNO, ParameterDirection.Input);
                                    par[30] = new OracleParameter("P_PUBLISH_FLAG", OracleDbType.Varchar2, pOHdr.PUBLISH_FLAG, ParameterDirection.Input);
                                    par[31] = new OracleParameter("P_SENT4VET", OracleDbType.Varchar2, pOHdr.SENT4VET, ParameterDirection.Input);
                                    par[32] = new OracleParameter("P_VET_DATE", OracleDbType.Date, !string.IsNullOrEmpty(pOHdr.VET_DATE) ? (object)Convert.ToDateTime(pOHdr.VET_DATE) : DBNull.Value, ParameterDirection.Input);
                                    par[33] = new OracleParameter("P_VET_BY", OracleDbType.Varchar2, pOHdr.VET_BY, ParameterDirection.Input);
                                    par[34] = new OracleParameter("P_REQ_FLAG", OracleDbType.Varchar2, pOHdr.REQ_FLAG, ParameterDirection.Input);
                                    par[35] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

                                    var ds = GetDataSet("SP_INSERT_MMP_POMA_HDR_FROMAPI", par, 1);
                                    // INSERT Data End
                                }
                                if (pODetailModels != null)
                                {
                                    // PoDetail Save logic
                                    // INSERT Data Start

                                    foreach (var item in pODetailModels)
                                    {
                                        OracleParameter[] par1 = new OracleParameter[24];
                                        par1[0] = new OracleParameter("P_RLY", OracleDbType.Varchar2, item.RLY, ParameterDirection.Input);
                                        par1[1] = new OracleParameter("P_MAKEY", OracleDbType.Varchar2, item.MAKEY, ParameterDirection.Input);
                                        par1[2] = new OracleParameter("P_SLNO", OracleDbType.Varchar2, item.SLNO, ParameterDirection.Input);
                                        par1[3] = new OracleParameter("P_MA_FLD", OracleDbType.Varchar2, item.MA_FLD, ParameterDirection.Input);
                                        par1[4] = new OracleParameter("P_MA_FLD_DESCR", OracleDbType.Varchar2, item.MA_FLD_DESCR, ParameterDirection.Input);
                                        par1[5] = new OracleParameter("P_OLD_VALUE", OracleDbType.Varchar2, item.OLD_VALUE, ParameterDirection.Input);
                                        par1[6] = new OracleParameter("P_NEW_VALUE", OracleDbType.Varchar2, item.NEW_VALUE, ParameterDirection.Input);
                                        par1[7] = new OracleParameter("P_NEW_VALUE_IND", OracleDbType.Varchar2, item.NEW_VALUE_IND, ParameterDirection.Input);
                                        par1[8] = new OracleParameter("P_NEW_VALUE_FLAG", OracleDbType.Varchar2, item.NEW_VALUE_FLAG, ParameterDirection.Input);
                                        par1[9] = new OracleParameter("P_PL_NO", OracleDbType.Varchar2, item.PL_NO, ParameterDirection.Input);
                                        par1[10] = new OracleParameter("P_PO_SR", OracleDbType.Varchar2, item.PO_SR, ParameterDirection.Input);
                                        par1[11] = new OracleParameter("P_EXP_SR", OracleDbType.Varchar2, item.EXP_SR, ParameterDirection.Input);
                                        par1[12] = new OracleParameter("P_EXP_CODE", OracleDbType.Varchar2, item.EXP_CODE, ParameterDirection.Input);
                                        par1[13] = new OracleParameter("P_COND_SLNO", OracleDbType.Varchar2, item.COND_SLNO, ParameterDirection.Input);
                                        par1[14] = new OracleParameter("P_COND_NO", OracleDbType.Varchar2, item.COND_NO, ParameterDirection.Input);
                                        par1[15] = new OracleParameter("P_COND_CODE", OracleDbType.Varchar2, item.COND_CODE, ParameterDirection.Input);
                                        par1[16] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, item.STATUS, ParameterDirection.Input);
                                        par1[17] = new OracleParameter("P_MA_SR_NO", OracleDbType.Varchar2, item.MA_SR_NO, ParameterDirection.Input);
                                        par1[18] = new OracleParameter("P_ORIG_DP", OracleDbType.Date, !string.IsNullOrEmpty(item.ORIG_DP) ? (object)Convert.ToDateTime(item.ORIG_DP) : DBNull.Value, ParameterDirection.Input);
                                        par1[19] = new OracleParameter("P_PAYMENT_YEAR", OracleDbType.Varchar2, item.PAYMENT_YEAR, ParameterDirection.Input);
                                        par1[20] = new OracleParameter("P_NEW_POSR_DATA", OracleDbType.Varchar2, item.NEW_POSR_DATA, ParameterDirection.Input);
                                        par1[21] = new OracleParameter("P_REF_PONO", OracleDbType.Varchar2, item.REF_PONO, ParameterDirection.Input);
                                        par1[22] = new OracleParameter("P_CONSIGNEE_RLY", OracleDbType.Varchar2, item.CONSIGNEE_RLY, ParameterDirection.Input);
                                        par1[23] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

                                        var ds1 = GetDataSet("SP_INSERT_MMP_POMA_HDR_DETAIL_FROMAPI", par1, 1);

                                    }
                                    // INSERT Data End
                                }
                            }
                        }
                    }
                }
            }
        }

        public void GetBillsStatus(string token)
        {
            string apiUrl = "https://trial.ireps.gov.in/immsapi/rites/getBillsStatus";

            object input = new
            {
                reqDate = DateTime.Now.ToString("yyyy-MM-dd"),
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
            BillsStatusResponse response = JsonConvert.DeserializeObject<BillsStatusResponse>(json);
            if (response != null)
            {
                if (response.data != null)
                {
                    List<BillsStatusModel> model2 = response.data;

                    if (model2 != null)
                    {
                        foreach (BillsStatusModel model in model2)
                        {
                            // BillsStatus Save logic
                            OracleParameter[] par = new OracleParameter[21];
                            par[0] = new OracleParameter("P_BILL_NO", OracleDbType.Varchar2, model.BILL_NO, ParameterDirection.Input);
                            par[1] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, model.IC_NO, ParameterDirection.Input);
                            par[2] = new OracleParameter("P_IC_DT", OracleDbType.Date, !string.IsNullOrEmpty(model.IC_DT) ? (object)Convert.ToDateTime(model.IC_DT) : DBNull.Value , ParameterDirection.Input);
                            par[3] = new OracleParameter("P_INVOICENO", OracleDbType.Varchar2, model.INVOICENO, ParameterDirection.Input);
                            par[4] = new OracleParameter("P_CO6_NO", OracleDbType.Varchar2, model.CO6_NO, ParameterDirection.Input);
                            par[5] = new OracleParameter("P_CO6_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_DATE) ? (object)Convert.ToDateTime(model.CO6_DATE) : DBNull.Value , ParameterDirection.Input);
                            par[6] = new OracleParameter("P_CO6_STATUS", OracleDbType.Varchar2, model.CO6_STATUS, ParameterDirection.Input);
                            par[7] = new OracleParameter("P_CO6_STATUS_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO6_STATUS_DATE) ? (object)Convert.ToDateTime(model.CO6_STATUS_DATE) : DBNull.Value , ParameterDirection.Input);
                            par[8] = new OracleParameter("P_PASSED_AMT", OracleDbType.Varchar2, model.PASSED_AMT, ParameterDirection.Input);
                            par[9] = new OracleParameter("P_DEDUCTED_AMT", OracleDbType.Varchar2, model.DEDUCTED_AMT, ParameterDirection.Input);
                            par[10] = new OracleParameter("P_NET_AMT", OracleDbType.Varchar2, model.NET_AMT, ParameterDirection.Input);
                            par[11] = new OracleParameter("P_BOOKDATE", OracleDbType.Date, !string.IsNullOrEmpty(model.BOOKDATE) ? (object)Convert.ToDateTime(model.BOOKDATE) : DBNull.Value, ParameterDirection.Input);
                            par[12] = new OracleParameter("P_RETURN_REASON", OracleDbType.Varchar2, model.RETURN_REASON, ParameterDirection.Input);
                            par[13] = new OracleParameter("P_RETURN_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.RETURN_DATE) ? (object)Convert.ToDateTime(model.RETURN_DATE) : DBNull.Value, ParameterDirection.Input);
                            par[14] = new OracleParameter("P_CO7_NO", OracleDbType.Varchar2, model.CO7_NO, ParameterDirection.Input);
                            par[15] = new OracleParameter("P_CO7_DATE", OracleDbType.Date, !string.IsNullOrEmpty(model.CO7_DATE) ? (object)Convert.ToDateTime(model.CO7_DATE) : DBNull.Value, ParameterDirection.Input);
                            par[16] = new OracleParameter("P_PAYMENT_DT", OracleDbType.Varchar2, model.PAYMENT_DT, ParameterDirection.Input);
                            par[17] = new OracleParameter("P_BILL_RESENT_COUNT", OracleDbType.Varchar2, model.BILL_RESENT_COUNT, ParameterDirection.Input);
                            par[18] = new OracleParameter("P_IRFC_FUNDED", OracleDbType.Varchar2, model.IRFC_FUNDED, ParameterDirection.Input);
                            par[19] = new OracleParameter("P_INVOICE_SUPP_DOCS", OracleDbType.Varchar2, model.INVOICE_SUPP_DOCS, ParameterDirection.Input);
                            par[20] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);
                            var ds1 = GetDataSet("SP_INSERT_RITES_BILL_DTLS_FROMAPI", par, 1);
                        }
                    }
                }
            }
        }

        public static DataSet GetDataSet(string procedurename, OracleParameter[] par, int Tablecount)
        {
            DataSet ds = new DataSet();
            string connectionString = ConfigurationManager.ConnectionStrings["SysNotificationsEntities"].ConnectionString;
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = procedurename;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (par != null && par.Length > 0)
                        {
                            foreach (var item in par)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        string[] tableNames = new string[Tablecount];
                        for (int i = 0; i < Tablecount; i++)
                        {
                            DataTable dt = new DataTable();
                            dt.TableName = "Table" + i.ToString();
                            tableNames[i] = dt.TableName;
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
            return ds;
        }

    }
}
