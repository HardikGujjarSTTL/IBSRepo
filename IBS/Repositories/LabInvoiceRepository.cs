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
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_FromDate", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ToDate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GetLabInvoices", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<labInvoicelst> lstLabInvoice = dt.AsEnumerable().Select(row => new labInvoicelst
                {
                    InvoiceNo = Convert.ToString(row["InvoiceNO"]),
                    BillNO = Convert.ToString(row["BillNO"]),
                    CaseNo = Convert.ToString(row["InvoiceDt"]),
                    irn_no = Convert.ToString(row["irn_no"]),
                    ack_no = Convert.ToString(row["ack_no"]),
                    ack_dt = Convert.ToDateTime(row["ack_dt"]),
                    qr_code = Convert.ToString(row["qr_code"]),
                    BPO_NAME = Convert.ToString(row["BPO_NAME"]),
                    bpo_add = Convert.ToString(row["bpo_add"]),
                    bpo_city = Convert.ToString(row["bpo_city"]),
                    recipient_gstin_no = Convert.ToString(row["recipient_gstin_no"]),
                    InvoiceBillNo = Convert.ToString(row["InvoiceNO"]).Split('/')[0] + Convert.ToString(row["BillNO"]).Split('-')[1],
                    Region_code = Region == "N" ? "NORTHERN REGION(INSPECTION)" :
                 Region == "S" ? "SOUTERN REGION(INSPECTION)" :
                 Region == "E" ? "EASTERN REGION(INSPECTION)" :
                 Region == "W" ? "WESTERN REGION(INSPECTION)" : Region,
                    RegionChar = Region
                }).ToList();
                model.lstlabInvoicelst = lstLabInvoice;
            }

            return model;
        }
        
        public labInvoicelst GetPDFLabInvoice(string FromDate, string ToDate, string Region)
        {
            labInvoicelst model = new();
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_FromDate", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ToDate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_PDFLabInvoices", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<labInvoicelst> lstLabInvoice = dt.AsEnumerable().Select(row => new labInvoicelst
                {
                    InvoiceNo = Convert.ToString(row["InvoiceNO"]),
                    BillNO = Convert.ToString(row["BillNO"]),
                    CaseNo = Convert.ToString(row["InvoiceDt"]),
                    irn_no = Convert.ToString(row["irn_no"]),
                    ack_no = Convert.ToString(row["ack_no"]),
                    ack_dt = Convert.ToDateTime(row["ack_dt"]),
                    qr_code = Convert.ToString(row["qr_code"]),
                    BPO_NAME = Convert.ToString(row["BPO_NAME"]),
                    bpo_add = Convert.ToString(row["bpo_add"]),
                    bpo_city = Convert.ToString(row["bpo_city"]),
                    recipient_gstin_no = Convert.ToString(row["recipient_gstin_no"]),
                    InvoiceBillNo = Convert.ToString(row["InvoiceNO"]).Split('/')[0] + Convert.ToString(row["BillNO"]).Split('-')[1],
                    Region_code = Region == "N" ? "NORTHERN REGION(INSPECTION)" :
                 Region == "S" ? "SOUTERN REGION(INSPECTION)" :
                 Region == "E" ? "EASTERN REGION(INSPECTION)" :
                 Region == "W" ? "WESTERN REGION(INSPECTION)" : Region,
                    RegionChar = Region
                }).ToList();
                model.lstlabInvoicelst = lstLabInvoice;
            }

            return model;
        }

        public int UpdatePDFDetails(string InvoiceNo, string PDFNamee, string RelativePath)
        {
            var res = 0;
            var data = (from item in context.T55LabInvoices
                        where item.InvoiceNo == InvoiceNo
                        select item).FirstOrDefault();
            try
            {
                if (data != null)
                {
                    data.Relativepath = RelativePath;
                    data.DigBillGenDt = DateTime.Now.Date;
                    data.Fileid = PDFNamee;
                    context.SaveChanges();
                    res = 1;
                }
            }
            catch (Exception)
            {
                res = 0;
            }
            return res;
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

