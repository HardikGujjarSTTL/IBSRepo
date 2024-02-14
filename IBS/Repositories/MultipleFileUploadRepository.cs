using IBS.Models;
using IBS.DataAccess;
using IBS.Interfaces;
using Oracle.ManagedDataAccess.Client;
using IBS.Helper;
using System.Data;

namespace IBS.Repositories
{
    public class MultipleFileUploadRepository : IMultipleFileUploadRepository
    {
        private readonly ModelContext context;

        public MultipleFileUploadRepository(ModelContext context)
        {
            this.context = context;
        }

        public int InsertPDFDetails(string FileName, string Bill_NO, int CreatedBy)
        {
            var res = 0;
            var data = (from item in context.T114MultipleBillFileUploads
                        where item.BillNo == Bill_NO
                        select item).FirstOrDefault();
            try
            {
                if (data == null)
                {
                    int maxID = context.T114MultipleBillFileUploads.Any() ? context.T114MultipleBillFileUploads.Max(x => x.Id) + 1 : 1;
                    T114MultipleBillFileUpload obj = new T114MultipleBillFileUpload();
                    obj.Id = maxID;
                    obj.BillNo = Bill_NO;
                    obj.FileName = FileName;
                    obj.Createdby = CreatedBy.ToString();
                    obj.Createddate = DateTime.Now.Date;
                    context.T114MultipleBillFileUploads.Add(obj);
                    context.SaveChanges();
                    res = 1;
                }
                else
                {
                    data.BillNo = Bill_NO;
                    data.FileName = FileName;
                    data.Updatedby = CreatedBy.ToString();
                    data.Updateddate = DateTime.Now.Date;
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

        public DTResult<MultipleFileUploadModel> GetDocList(DTParameters dtParameters)
        {

            DTResult<MultipleFileUploadModel> dTResult = new() { draw = 0 };
            IQueryable<MultipleFileUploadModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "CaseNo";
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]) ? Convert.ToString(dtParameters.AdditionalValues["CaseNo"]) : "";
            string PoNo = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]) ? Convert.ToString(dtParameters.AdditionalValues["PoNo"]) : "";
            string PoDt = !string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]) ? Convert.ToString(dtParameters.AdditionalValues["PoDt"]) : "";
            string CallSno = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]) ? Convert.ToString(dtParameters.AdditionalValues["CallSno"]) : "";
            string BillNO = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNO"]) ? Convert.ToString(dtParameters.AdditionalValues["BillNO"]) : "";
            string CallRecvDt = !string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]) ? Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]) : "";
            string BillDT = !string.IsNullOrEmpty(dtParameters.AdditionalValues["BillDT"]) ? Convert.ToString(dtParameters.AdditionalValues["BillDT"]) : "";

            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("P_bill_no", OracleDbType.Varchar2, BillNO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_BILL_DT", OracleDbType.Varchar2, BillDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[3] = new OracleParameter("P_call_recv_dt", OracleDbType.Varchar2, CallRecvDt, ParameterDirection.Input);
            par[4] = new OracleParameter("P_call_sno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
            par[5] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[6] = new OracleParameter("P_PO_DT", OracleDbType.Varchar2, PoDt, ParameterDirection.Input);
            par[7] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_MultiDocList", par, 1);
            DataTable dt = ds.Tables[0];

            List<MultipleFileUploadModel> list = dt.AsEnumerable().Select(row => new MultipleFileUploadModel
            {
                CaseNo = row["CASE_NO"].ToString(),
                PoNo = row["PO_NO"].ToString(),
                PoDt = row["PO_DT"].ToString(),
                CallRecvDt = Convert.ToDateTime(row["call_recv_dt"]),
                CallSno = row["call_sno"].ToString(),
                BillNO = row["BILL_NO"].ToString(),
                BillDT = Convert.ToDateTime(row["bill_dt"]),
                FileName = row["file_name"].ToString()
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                || Convert.ToString(w.BillNO).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
