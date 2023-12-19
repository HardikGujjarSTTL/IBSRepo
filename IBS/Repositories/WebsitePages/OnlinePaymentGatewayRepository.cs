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
                            VEND_NAME = $"{v.VendName.Trim()}/{v.VendAdd1.Trim()}/{(c.Location != null ? c.Location + " / " : "")}{c.City}",                         
                        }).FirstOrDefault();
            
            if(GetPODetails == null)
            {
                model.AlertMsg = "Record not found for the given Case No. and Call Recieve Date & Call Sno. !!!";
            }
            else
            {
                model.PO_NO = GetPODetails.PO_NO;
                model.VEND_NAME = GetPODetails.VEND_NAME;
            }
            
            return model;
        }

    }
}
