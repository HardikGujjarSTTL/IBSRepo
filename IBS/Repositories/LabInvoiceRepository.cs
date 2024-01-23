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

        public LabInvoiceReportModel GetLabInvoice(string FromDate, string ToDate, string Region)
        {

           var query = (from T55 in context.T55LabInvoices
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
                     });

            List<labInvoicelst> lstHighOutstanding = query.AsEnumerable().Select(row => new labInvoicelst
            {
                InvoiceNo = row.InvoiceNo,
                BillNO = row.BillNO,
            }).ToList();

            foreach (var item in lstHighOutstanding)
            {
                string[] invoiceParts = item.InvoiceNo.Split('/');
                string[] billParts = item.BillNO.Split('-');
                item.InvoiceBillNo = $"{invoiceParts[0]}{billParts[1]}";
            }

            LabInvoiceReportModel model1 = new LabInvoiceReportModel
            {
                lstlabInvoicelst = lstHighOutstanding,
            };

            return model1;
        }
    }
}

