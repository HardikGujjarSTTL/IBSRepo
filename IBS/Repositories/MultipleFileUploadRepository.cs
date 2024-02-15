using IBS.Models;
using IBS.DataAccess;
using IBS.Interfaces;
using Oracle.ManagedDataAccess.Client;
using IBS.Helper;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Cryptography;

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

            string CaseNo = null, PoNo = null, BillNO = null, CallSno = null,SetNo = null,BKNO= null, IC_NO = null, IC_DT_From = null, IC_DT_To = null;
            DateTime? CallRecvDt = null, BillDT = null;

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
            {
                CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillNO"]))
            {
                BillNO = Convert.ToString(dtParameters.AdditionalValues["BillNO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToDateTime(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BillDT"]))
            {
                BillDT = Convert.ToDateTime(dtParameters.AdditionalValues["BillDT"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IC_DT_From"]))
            {
                IC_DT_From = Convert.ToString(dtParameters.AdditionalValues["IC_DT_From"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IC_DT_To"]))
            {
                IC_DT_To = Convert.ToString(dtParameters.AdditionalValues["IC_DT_To"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IC_NO"]))
            {
                IC_NO = Convert.ToString(dtParameters.AdditionalValues["IC_NO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BKNO"]))
            {
                BKNO = Convert.ToString(dtParameters.AdditionalValues["BKNO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNo"]))
            {
                SetNo = Convert.ToString(dtParameters.AdditionalValues["SetNo"]);
            }

            OracleParameter[] par = new OracleParameter[15];
            par[0] = new OracleParameter("P_bill_no", OracleDbType.Varchar2, BillNO, ParameterDirection.Input);
            par[1] = new OracleParameter("P_BILL_DT", OracleDbType.Date, BillDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[3] = new OracleParameter("P_call_recv_dt", OracleDbType.Date, CallRecvDt, ParameterDirection.Input);
            par[4] = new OracleParameter("P_call_sno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
            par[5] = new OracleParameter("P_PO_NO", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[6] = new OracleParameter("P_IC_NO", OracleDbType.Varchar2, IC_NO, ParameterDirection.Input);
            par[7] = new OracleParameter("P_IC_DT_frm", OracleDbType.Varchar2, IC_DT_From, ParameterDirection.Input);
            par[8] = new OracleParameter("P_IC_DT_to", OracleDbType.Varchar2, IC_DT_To, ParameterDirection.Input);
            par[9] = new OracleParameter("P_BK_NO", OracleDbType.Varchar2, BKNO, ParameterDirection.Input);
            par[10] = new OracleParameter("P_SET_NO", OracleDbType.Varchar2, SetNo, ParameterDirection.Input);
            par[11] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
            par[12] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
            par[13] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[14] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

            List<MultipleFileUploadModel> list = new();
            var ds = DataAccessDB.GetDataSet("GET_MultiDocList", par, 2);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    list = dt.AsEnumerable().Select(row => new MultipleFileUploadModel
                    {
                        CaseNo = row["CASE_NO"].ToString(),
                        BKNO = row["bk_no"].ToString(),
                        SetNo = row["set_no"].ToString(),
                        PoNo = row["PO_NO"].ToString(),
                        CallSno = row["call_sno"].ToString(),
                        BillNO = row["BILL_NO"].ToString(),
                        FileName = row["file_name"].ToString(),
                        IC_NO = row["ic_no"].ToString(),
                        CallRecvDt = row["call_recv_dt"] != DBNull.Value ? Convert.ToDateTime(row["call_recv_dt"]) : null,
                        BillDT = row["bill_dt"] != DBNull.Value ? Convert.ToDateTime(row["bill_dt"]) : null,
                        IC_DT = row["ic_dt"] != DBNull.Value ? Convert.ToDateTime(row["ic_dt"]) : null,
                        CreatedDate = row["createddate"] != DBNull.Value ? Convert.ToDateTime(row["createddate"]) : null
                    }).ToList();
                }
            }

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

            return dTResult;
        }
    }
}
