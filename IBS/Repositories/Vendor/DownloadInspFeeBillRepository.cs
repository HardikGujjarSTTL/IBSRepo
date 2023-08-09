using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Vendor;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories.Vendor
{
    public class DownloadInspFeeBillRepository : IDownloadInspFeeBillRepository
    {
        private readonly ModelContext context;

        public DownloadInspFeeBillRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<DownloadInspectionFeeBillModel> GetDataList(DTParameters dtParameters, string UserName)
        {

            DTResult<DownloadInspectionFeeBillModel> dTResult = new() { draw = 0 };
            IQueryable<DownloadInspectionFeeBillModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string CaseNo = "", BkNo = "", SetNo = "", CallRecvDt = "", PoNo = "", PoDt = "";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
            {
                CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BkNo"]))
            {
                BkNo = Convert.ToString(dtParameters.AdditionalValues["BkNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SetNo"]))
            {
                SetNo = Convert.ToString(dtParameters.AdditionalValues["SetNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecvDt"]))
            {
                CallRecvDt = Convert.ToString(dtParameters.AdditionalValues["CallRecvDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoNo"]))
            {
                PoNo = Convert.ToString(dtParameters.AdditionalValues["PoNo"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PoDt"]))
            {
                PoDt = Convert.ToString(dtParameters.AdditionalValues["PoDt"]);
            }

            CaseNo = CaseNo.ToString() == "" ? string.Empty : CaseNo.ToString();
            DateTime? _CallRecvDt = CallRecvDt == "" ? null : DateTime.ParseExact(CallRecvDt, "dd-MM-yyyy", null);
            PoNo = PoNo.ToString() == "" ? string.Empty : PoNo.ToString();
            DateTime? _PoDt = PoDt == "" ? null : DateTime.ParseExact(PoDt, "dd-MM-yyyy", null);
            BkNo = BkNo.ToString() == "" ? string.Empty : BkNo.ToString();
            SetNo = SetNo.ToString() == "" ? string.Empty : SetNo.ToString();

            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("p_vend_cd", OracleDbType.Varchar2, UserName, ParameterDirection.Input);
            par[1] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, BkNo, ParameterDirection.Input);
            par[3] = new OracleParameter("p_set_no", OracleDbType.Varchar2, SetNo, ParameterDirection.Input);
            par[4] = new OracleParameter("p_dt_of_receipt", OracleDbType.Date, _CallRecvDt, ParameterDirection.Input);
            par[5] = new OracleParameter("p_po_no", OracleDbType.Varchar2, PoNo, ParameterDirection.Input);
            par[6] = new OracleParameter("p_po_dt", OracleDbType.Date, _PoDt, ParameterDirection.Input);
            par[7] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_INSPECTION_BILL_DETAILS_DOWNL", par, 1);
            if(ds!=null)
            {
                DataTable dt = ds.Tables[0];
                List<DownloadInspectionFeeBillModel> list = dt.AsEnumerable().Select(row => new DownloadInspectionFeeBillModel
                {
                    CaseNo = row["CASE_NO"].ToString(),
                    CallRecvDt = Convert.ToDateTime(row["CALL_RECV_DT"]),
                    CallInstallNo = row["CALL_INSTALL_NO"].ToString(),
                    CallSNo = row["CALL_SNO"].ToString(),
                    CallStatus = row["CALL_STATUS"].ToString(),
                    PoNo = row["PO_NO"].ToString(),
                    PoDt = Convert.ToDateTime(row["PO_DT"]),
                    IeSName = row["IE_SNAME"].ToString(),
                    Vendor = row["VENDOR"].ToString(),
                    BillNo = row["BILL_NO"].ToString(),
                    BillDt = row["BILL_DATE"].ToString(),
                    BkNo = row["BK_NO"].ToString(),
                    SetNo = row["SET_NO"].ToString(),
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                dTResult.recordsFiltered = ds.Tables[0].Rows.Count;

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;
            }
            

            return dTResult;
        }

    }
}
