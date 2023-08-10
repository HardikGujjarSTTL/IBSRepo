using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace IBS.Repositories
{
    public class NCRRegisterRepository : INCRRegisterRepository
    {
        public DTResult<NCRRegister> GetDataList(DTParameters dtParameters)
        {
            DTResult<NCRRegister> dTResult = new() { draw = 0 };
            IQueryable<NCRRegister>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "NCNO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "NCNO";
                orderAscendingDirection = true;
            }

            string NCNO = "", CASENO = "", todate="", fromdate="", IENAME="";

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["NCNO"]))
            {
                NCNO = Convert.ToString(dtParameters.AdditionalValues["NCNO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CASENO"]))
            {
                CASENO = Convert.ToString(dtParameters.AdditionalValues["CASENO"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["selectedValue"]))
            {
                IENAME = Convert.ToString(dtParameters.AdditionalValues["selectedValue"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["fromdate"]))
            {
                fromdate = Convert.ToString(dtParameters.AdditionalValues["fromdate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["todate"]))
            {
                todate = Convert.ToString(dtParameters.AdditionalValues["todate"]);
            }

            NCRRegister model = new NCRRegister();
            DataTable dt = new DataTable();
            List<NCRRegister> modelList = new List<NCRRegister>();
            string Region = "";

            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASENO, ParameterDirection.Input);
            par[1] = new OracleParameter("p_nc_no", OracleDbType.Varchar2, NCNO, ParameterDirection.Input);
            par[2] = new OracleParameter("p_lstIE", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
            par[3] = new OracleParameter("p_frmDt", OracleDbType.Date, fromdate, ParameterDirection.Input);
            par[4] = new OracleParameter("p_toDt", OracleDbType.Date, todate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetFilterNCR", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

                List<NCRRegister> list = dt.AsEnumerable().Select(row => new NCRRegister
                {
                    CaseNo = row.Field<string>("CASE_NO"),
                    BKNo = row.Field<string>("BK_NO"),
                    SetNo = row.Field<string>("SET_NO"),
                    NC_NO = row.Field<string>("NC_NO"),
                    CALL_SNO = row.Field<int>("CALL_SNO"),
                    IE_SNAME = row.Field<string>("IE_SNAME"),
                    CALL_RECV_DT = DateTime.TryParseExact(row.Field<string>("CALL_RECV_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime callRecvDate)
            ? callRecvDate
            : (DateTime?)null,
                    CONSIGNEE = row.Field<string>("CONSIGNEE"),
                }).ToList();

                query = list.AsQueryable();

                dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                if (!string.IsNullOrEmpty(searchBy))
                    query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                    || Convert.ToString(w.NC_NO).ToLower().Contains(searchBy.ToLower())
                    );

                dTResult.recordsFiltered = query.Count();

                dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                dTResult.draw = dtParameters.Draw;

            }
            else
            {
                return dTResult;
            }

            return dTResult;
        }
    }
}
