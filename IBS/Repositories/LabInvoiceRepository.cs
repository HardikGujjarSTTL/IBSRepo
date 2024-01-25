using DocumentFormat.OpenXml.InkML;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using iText.Commons.Actions.Contexts;
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

        public labInvoicelst GetLabInvoice(string FromDate, string ToDate, string Region)
        {
            labInvoicelst model = new();
           
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
                         InvoiceDt = T55.InvoiceDt,
                         CaseNo = T55.CaseNo,
                         irn_no = T55.IrnNo,
                         ack_no = T55.AckNo,
                         ack_dt = T55.AckDt,
                         recipient_gstin_no = T55.RecipientGstinNo,
                     }).Take(5);

            List<labInvoicelst> lstLabInvoice = query.AsEnumerable().Select(row => new labInvoicelst
            {
                InvoiceNo = row.InvoiceNo,
                BillNO = row.BillNO,
                CaseNo = row.CaseNo,
                irn_no = row.irn_no,
                ack_no = row.ack_no,
                ack_dt = row.ack_dt,
                recipient_gstin_no = row.recipient_gstin_no,
                InvoiceBillNo = row.InvoiceNo.Split('/')[0] + row.BillNO.Split('-')[1],
                Region_code = Region == "N" ? "NORTHERN REGION(INSPECTION)" :
                  Region == "S" ? "SOUTERN REGION(INSPECTION)" :
                  Region == "E" ? "EASTERN REGION(INSPECTION)" :
                  Region == "W" ? "WESTERN REGION(INSPECTION)" : Region
            }).ToList();

            model.lstlabInvoicelst = lstLabInvoice;

            return model;
        }

        public labInvoicelst UpdatePDFDetails(labInvoicelst model, string PDFNamee, string RelativePath)
        {
            var invoiceToUpdate = context.T55LabInvoices.FirstOrDefault(i => i.InvoiceNo == model.InvoiceNo);

            if (invoiceToUpdate != null)
            {
                invoiceToUpdate.DigBillGenDt = DateTime.Now.Date;
                invoiceToUpdate.Relativepath = RelativePath;
                invoiceToUpdate.Fileid = PDFNamee;
                context.SaveChanges(); 
            }

            return model;
        }

        public List<LabItemsDetail> GetBillItems(string InvoiceNo)
        {
            List<LabItemsDetail> list = new List<LabItemsDetail>();
            list = (from vbi in context.T86LabInvoiceDetails
                    where vbi.InvoiceNo == InvoiceNo
                    select new LabItemsDetail
                    {
                        INVOICE_NO = vbi.InvoiceNo,
                        ITEM_DESC = vbi.ItemDesc,
                        IGST = vbi.Igst,
                        QTY = vbi.Qty,
                        TESTING_CHARGES = vbi.TestingCharges,
                    }).ToList();

            return list;
        }
    }
}

