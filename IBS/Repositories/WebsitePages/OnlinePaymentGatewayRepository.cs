using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.WebsitePages;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace IBS.Repositories.WebsitePages
{
    public class OnlinePaymentGatewayRepository : IOnlinePaymentGatewayRepository
    {
        private readonly ModelContext context;
        #region Vendor feedback
        public OnlinePaymentGatewayRepository(ModelContext context)
        {
            this.context = context;
        }
        #endregion

        public OnlinePaymentGateway VerifyByCaseNo(OnlinePaymentGateway model)
        {
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_caseno", OracleDbType.Varchar2, model.CaseNo, ParameterDirection.Input);
            par[1] = new OracleParameter("p_calldate", OracleDbType.Varchar2,model.CallDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_callsno", OracleDbType.Varchar2, model.CallSno, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetPODetailsForPaymentGateway", par, 1);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model.PO_NO = ds.Tables[0].Rows[0]["PO_NO"]?.ToString();
                model.VEND_NAME = ds.Tables[0].Rows[0]["VEND_NAME"]?.ToString();
                model.VEND_CD = ds.Tables[0].Rows[0]["VEND_CD"] as int?; 
            }
            else
            {
                model.AlertMsg = "Record not found for the given Case No. and Call Receive Date & Call Sno. !!!";
            }

            return model;
        }

        public OnlinePaymentGateway PaymentIntergreationSave(OnlinePaymentGateway model)
        {
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_DATE", OracleDbType.Varchar2, DateTime.Now.ToString("ddMMyy"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GenerateMERID", par, 1);

            string merNo = ds.Tables[0].Rows[0]["MERNO"].ToString();

            string mer_ref = DateTime.Now.ToString("ddMMyy") + model.CaseNo.Substring(0, 1) + model.ChargesType + merNo.PadLeft(5,'0');
            model.MER_TXN_REF = mer_ref;
            var OnlinePayment = new OnlinePayment
            {
                MerTxnRef = mer_ref,
                OrderInfo = model.VEND_CD.HasValue ? (short?)model.VEND_CD.Value : null,
                CaseNo = model.CaseNo.Trim(),
                CallRecvDt = Convert.ToDateTime(model.CallDate),
                CallSno = model.CallSno.HasValue ? (short?)model.CallSno.Value : null,
                VendCd = model.VEND_CD.HasValue ? (short?)model.VEND_CD.Value : null,
                Amount = model.Charges,
                ChargesType = model.ChargesType,
                Datetime = DateTime.Now
            };

            context.OnlinePayments.Add(OnlinePayment);
            context.SaveChanges();

            return model;
        }

        public OnlinePaymentGateway PaymentResponseUpdate(OnlinePaymentGateway model)
        {
            var onlinePayment = context.OnlinePayments.FirstOrDefault(p => p.MerTxnRef == model.MER_TXN_REF.Trim());

            if (onlinePayment != null)
            {
                onlinePayment.TransactionNo = model.BankTXNID;
                onlinePayment.RrnNo = null;
                onlinePayment.Status = model.PaymentStatus;
                onlinePayment.CustEmail = model.Email;
                onlinePayment.CustMobile = model.Mobile;
                onlinePayment.MerId = model.MerID;
                onlinePayment.MerTxnDate = Convert.ToDateTime(model.merchTxnDate);
                onlinePayment.MerTxnId = model.MERTXNID;
                onlinePayment.AtomTxnId = model.AtomTXNID;
                onlinePayment.CustAccNo = model.custAccNo;
                onlinePayment.TxnCompleteDate = Convert.ToDateTime(model.TranDate);
                onlinePayment.BankTxnId = model.BankTXNID;
                onlinePayment.BankName = model.BankName;
                onlinePayment.SubChannel = model.SubChannel;
                onlinePayment.Description = model.Description;
                onlinePayment.StatusCd = model.StatusCode;

                context.SaveChanges();
            }
            return model;
        }
    }
}
