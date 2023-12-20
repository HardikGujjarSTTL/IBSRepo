using DocumentFormat.OpenXml.Bibliography;
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
            var GetPODetails = (from p in context.T13PoMasters
                                join v in context.T05Vendors on p.VendCd equals v.VendCd
                                join c in context.T03Cities on v.VendCityCd equals c.CityCd
                                join r in context.T17CallRegisters on p.CaseNo equals r.CaseNo
                                where r.CaseNo == model.CaseNo &&
                                      r.CallRecvDt == model.CallDate &&
                                      r.CallSno == model.CallSno
                                select new OnlinePaymentGateway
                                {
                                    PO_NO = p.PoNo + "  &  " + p.PoDt.Value.ToString("dd/MM/yyyy"),
                                    PO_DT = p.PoDt,
                                    VEND_CD=v.VendCd,
                                    VEND_NAME = $"{v.VendName.Trim()}/{v.VendAdd1.Trim()}/{(c.Location != null ? c.Location + " / " : "")}{c.City}",
                                }).FirstOrDefault();

            if (GetPODetails == null)
            {
                model.AlertMsg = "Record not found for the given Case No. and Call Recieve Date & Call Sno. !!!";
            }
            else
            {
                model.PO_NO = GetPODetails.PO_NO;
                model.VEND_NAME = GetPODetails.VEND_NAME;
                model.VEND_CD = GetPODetails.VEND_CD;
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

            var OnlinePayment = new OnlinePayment
            {
                MerTxnRef = mer_ref,
                OrderInfo = model.VEND_CD.HasValue ? (short?)model.VEND_CD.Value : null,
                CaseNo = model.CaseNo.Trim(),
                CallRecvDt = model.CallDate,
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
    }
}
