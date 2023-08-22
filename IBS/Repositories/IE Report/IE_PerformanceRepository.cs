using IBS.Helper;
using IBS.Interfaces.IE_Reports;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Drawing;

namespace IBS.Repositories.IE_Report
{
    public class IE_PerformanceRepository : IIE_PerfomanceRepository
    {
        public DTResult<IE_PerformanceModel> Get_IE_Performance(DTParameters dtParameters, IEPerformanceFilter model)
        {
            DTResult<IE_PerformanceModel> dTResult = new () { draw = 0 };            
            IQueryable<IE_PerformanceModel>? query = null;


            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;


            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IE_NAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "IE_NAME";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", REGION = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(model.Region))
            {
                REGION = model.Region;
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                model.IE_CD = null;
            }else if (!string.IsNullOrEmpty(model.IE_CD))
            {
                model.IE_CD = model.IE_CD.Trim();
            }

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();
            REGION = REGION.ToString() == "" ? string.Empty : REGION.ToString();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[3] = new OracleParameter("P_IECD", OracleDbType.Varchar2, model.IE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("SP_GET_IE_PERFORMANCE_REPORT", par, 1);
            DataTable dt = ds.Tables[0];

            List<IE_PerformanceModel> list = dt.AsEnumerable().Select(row => new IE_PerformanceModel
            {
                IE_NAME = row["IE_NAME"].ToString(),
                DEPT = row["DEPT"].ToString(),
                C3 = row["C3"].ToString(),
                C7 = row["C7"].ToString(),
                CM7 = row["CM7"].ToString(),
                C10 = row["C10"].ToString(),
                C0 = row["C0"].ToString(),
                INSP_FEE = row["INSP_FEE"].ToString(),
                MATERIAL_VALUE = row["MATERIAL_VALUE"].ToString(),
                CALLS = row["CALLS"].ToString(),
                CALL_CANCEL = row["CALL_CANCEL"].ToString(),
                REJECTIONS = row["REJECTIONS"].ToString()
            }).ToList();

            query = list.AsQueryable();

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public IEPerformanceSummary Get_IE_Performance_Summary(IEFromToDate obj, IEPerformanceFilter model)
        {
            IEPerformanceSummary result = new IEPerformanceSummary();


            string FromDate = "", ToDate = "";

            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            //{
            //    FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            //}
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            //{
            //    ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            //}            
            if(obj.FromDt != null)
            {
                FromDate = Convert.ToDateTime(obj.FromDt).ToString("dd/MM/yyyy");
            }
            if (obj.ToDt != null)
            {
                ToDate = Convert.ToDateTime(obj.ToDt).ToString("dd/MM/yyyy");
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                model.IE_CD = null;
            }
            else if (!string.IsNullOrEmpty(model.IE_CD))
            {
                model.IE_CD = model.IE_CD.Trim();
            }

            FromDate = FromDate.ToString() == "" ? string.Empty : FromDate.ToString();
            ToDate = ToDate.ToString() == "" ? string.Empty : ToDate.ToString();            

            OracleParameter[] par = new OracleParameter[9];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[3] = new OracleParameter("P_IECD", OracleDbType.Varchar2, model.IE_CD, ParameterDirection.Input);
            par[4] = new OracleParameter("P_REJECTION_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[5] = new OracleParameter("P_NOOFICS_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[6] = new OracleParameter("P_CALLS_WITHIN_7_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[7] = new OracleParameter("P_CALLS_BEYOND_7_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            par[8] = new OracleParameter("P_SUMMARY_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output); 

            var ds = DataAccessDB.GetDataSet("SP_GET_IE_PERFORMANCE_SUMMARY", par, 5);
            DataTable dt = ds.Tables[4];
            result.Rejection = Convert.ToString(ds.Tables[0].Rows[0][0]);
            result.NoOfIcs = Convert.ToString(ds.Tables[1].Rows[0][0]);
            result.CallsWithin = Convert.ToString(ds.Tables[2].Rows[0][0]);
            result.CallsBeyond = Convert.ToString(ds.Tables[3].Rows[0][0]);
            result.IEPerSumFooter = dt.AsEnumerable().Select(row => new IEPerformanceSummaryFooter
            {
                RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"]),
                IC_COUNT = Convert.ToInt32(row["IC_COUNT"]),
                MATERIAL_VALUE = Convert.ToDecimal(row["MATERIAL_VALUE"]),
            }).ToList();
            return result;

        }
    }
}
