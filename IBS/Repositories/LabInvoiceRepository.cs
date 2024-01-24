using DocumentFormat.OpenXml.InkML;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class LabInvoiceRepository : ILabInvoiceRepository
    {
        private readonly ModelContext context;

        public LabInvoiceRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<labInvoicelst> GetLabInvoice(DTParameters dtParameters)
        {
            DTResult<labInvoicelst> dTResult = new() { draw = 0 };
            IQueryable<labInvoicelst>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = false;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "InvoiceBillNo";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "InvoiceBillNo";
                orderAscendingDirection = true;
            }
            string FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToString(dtParameters.AdditionalValues["FromDate"]) : null;
            string ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToString(dtParameters.AdditionalValues["ToDate"]) : null;
            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : null;

            query = (from T55 in context.T55LabInvoices
                     join T22 in context.T22Bills on T55.CaseNo equals T22.CaseNo
                     where T55.InvoiceDt >= DateTime.Parse(FromDate) && T55.InvoiceDt <= DateTime.Parse(ToDate)
                           && T55.RegionCode == Region
                           && T55.QrCode != null
                           && T55.IrnNo != null
                           && T55.AckNo != null
                           && T55.AckDt != null
                           && T55.DigBillGenDt == null
                           && T55.BillFinalised != null
                     select new labInvoicelst
                     {
                         InvoiceNo = T55.InvoiceNo,
                         BillNO = T22.BillNo,
                         InvoiceDt = T55.InvoiceDt,
                         CaseNo = T55.CaseNo,
                         irn_no = T55.IrnNo,
                         ack_no = T55.AckNo,
                         ack_dt = T55.AckDt,
                         recipient_gstin_no = T55.RecipientGstinNo,
                     });

            List<labInvoicelst> lstLabInvoice = query.AsEnumerable().Select(row => new labInvoicelst
            {
                InvoiceNo = row.invoice_no,
                BillNO = row.BillNO,
                InvoiceBillNo = row.InvoiceNo.Split('/')[0] + row.BillNO.Split('-')[1]
            }).ToList();

            query = lstLabInvoice.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.InvoiceBillNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsTotal = query.Count();
            dTResult.recordsFiltered = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}

