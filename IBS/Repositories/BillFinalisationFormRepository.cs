using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class BillFinalisationFormRepository : IBillFinalisationFormRepository
    {
        private readonly ModelContext context;

        public BillFinalisationFormRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<BillFinalisationFormModel> GetBillFinalisationList(DTParameters dtParameters)
        {
            DTResult<BillFinalisationFormModel> dTResult = new() { draw = 0 };
            IQueryable<BillFinalisationFormModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "BillNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "BillNo";
                orderAscendingDirection = true;
            }
            //query = from v22 in context.V22Bills
            //        where v22.BillDt >= new DateTime(2020, 11, 1) && v22.SentToSap == null && v22.BillFinalised == null && v22.RegionCode == Region
            //        && v22.BillDt >= FromDate && v22.BillDt <= ToDate
            //        && (Sector != "A" ? v22.BpoType == Sector : true)
            //        orderby v22.BillNo
            //        select new BillFinalisationFormModel
            //        {
            //            BillNo = v22.BillNo,
            //            BillDt = v22.BillDt,
            //            InspFee = v22.InspFee,
            //            Cgst = v22.Cgst,
            //            Sgst = v22.Sgst,
            //            Igst = v22.Igst,
            //            BillAmount = v22.BillAmount,
            //            InvoiceNo = v22.InvoiceNo,
            //            BPO = v22.BpoName + "/" + v22.BpoRly + "/" + v22.BpoCity,
            //            Consignee = v22.Consignee + "/" + v22.ConsigneeCity,
            //            RecipientGstinNo = v22.RecipientGstinNo
            //        };

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            DateTime? FromDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["FromDate"]) : null;
            DateTime? ToDate = !string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]) ? Convert.ToDateTime(dtParameters.AdditionalValues["ToDate"]) : null;
            string Sector = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Sector"]) ? Convert.ToString(dtParameters.AdditionalValues["Sector"]) : "";

            DataTable dt = new DataTable();
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_FromDate", OracleDbType.Varchar2, FromDate.Value.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[1] = new OracleParameter("p_ToDate", OracleDbType.Varchar2, ToDate.Value.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[2] = new OracleParameter("p_Sector", OracleDbType.Varchar2, Sector, ParameterDirection.Input);
            par[3] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[4] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[5] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[6] = new OracleParameter("p_RESULT", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetBillFinalisationForm", par, 2);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<BillFinalisationFormModel> list = dt.AsEnumerable().Select(row => new BillFinalisationFormModel
                {
                    BillNo = Convert.ToString(row["BillNo"]),
                    BillDt = Convert.ToDateTime(row["BillDt"]),
                    InspFee = Convert.ToDecimal(row["InspFee"]),
                    Cgst = Convert.ToDecimal(row["CGST"]),
                    Sgst = Convert.ToDecimal(row["SGST"]),
                    Igst = Convert.ToDecimal(row["IGST"]),
                    BillAmount = Convert.ToDecimal(row["BillAmount"]),
                    InvoiceNo = Convert.ToString(row["InvoiceNo"]),
                    BPO = Convert.ToString(row["BPO"]),
                    Consignee = Convert.ToString(row["Consignee"]),
                    RecipientGstinNo = Convert.ToString(row["RecipientGstinNo"]),
                }).ToList();

                query = list.AsQueryable();

                int recordsTotal = 0;
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["total_records"]);
                }
                dTResult.recordsTotal = recordsTotal;
                dTResult.recordsFiltered = recordsTotal;
                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Select(p => p).ToList();
                dTResult.draw = dtParameters.Draw;
            }
            //dTResult.recordsTotal = query.Count();

            //dTResult.recordsFiltered = query.Count();

            //if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            //dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            //dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public void UpdateBillFinalisationStatus(string[] BillNos)
        {
            if (BillNos.Length > 0)
            {
                foreach (var id in BillNos)
                {
                    T22Bill bill = context.T22Bills.Find(id);
                    if (bill != null)
                    {
                        bill.BillDt = DateTime.Now.Date;
                        bill.BillFinalised = "Y";
                        context.SaveChanges();
                    }
                }
            }
        }
    }

}