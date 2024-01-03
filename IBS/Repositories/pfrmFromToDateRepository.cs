using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class pfrmFromToDateRepository : IpfrmFromToDateRepository
    {
        private readonly ModelContext context;

        public pfrmFromToDateRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<ICIsuued> GetDataList(DTParameters dtParameters)
        {
            DTResult<ICIsuued> dTResult = new() { draw = 0 };
            IQueryable<ICIsuued>? query = null;
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "BK_NO";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "BK_NO";
                    orderAscendingDirection = true;
                }

                string ToDate = null, FromDate = null;


                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
                {
                    FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
                {
                    ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
                }

                DataTable dt = new DataTable();

                DataSet ds;

                OracleParameter[] par = new OracleParameter[6];
                par[0] = new OracleParameter("p_start_date", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
                par[1] = new OracleParameter("p_end_date", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
                par[2] = new OracleParameter("p_page_start", OracleDbType.Int32, dtParameters.Start + 1, ParameterDirection.Input);
                par[3] = new OracleParameter("p_page_end", OracleDbType.Int32, (dtParameters.Start + dtParameters.Length), ParameterDirection.Input);
                par[4] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
                par[5] = new OracleParameter("p_result_records", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("CMWiseICDetail", par, 2);


                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<ICIsuued> list = dt.AsEnumerable().Select(row => new ICIsuued
                    {
                        CASE_NO = row.Field<string>("CASE_NO"),
                        BK_NO = row.Field<string>("BK_NO"),
                        SET_NO = row.Field<string>("SET_NO"),
                        IE_NAME = row.Field<string>("IE_NAME"),
                        CO_NAME = row.Field<string>("CO_NAME"),
                        IC_ISSUED_DT = DateTime.ParseExact(row["IC_ISSUED_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
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
                    return dTResult;

                }
                else
                {
                    return dTResult;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return dTResult;
        }
    }
}
