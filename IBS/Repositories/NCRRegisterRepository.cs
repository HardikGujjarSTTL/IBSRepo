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

            string NCNO = "", CASENO = "", todate=null, fromdate= null, IENAME="";

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

        public NCRRegister FindByIDActionA(string CASE_NO, string BK_NO, string SET_NO, string NCNO)
        {
            NCRRegister model = new NCRRegister();
            DataTable dt = new DataTable();

            if(NCNO != "" && NCNO != null)
            {
                OracleParameter[] par = new OracleParameter[2];
                par[0] = new OracleParameter("p_nc_no", OracleDbType.Varchar2, NCNO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GetForAction_M_NCR", par, 1);
                dt = ds.Tables[0];
            }
            else {
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_case_no", OracleDbType.Varchar2, CASE_NO, ParameterDirection.Input);
                par[1] = new OracleParameter("p_bk_no", OracleDbType.Varchar2, BK_NO, ParameterDirection.Input);
                par[2] = new OracleParameter("p_set_no", OracleDbType.Varchar2, SET_NO, ParameterDirection.Input);
                par[3] = new OracleParameter("RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                var ds = DataAccessDB.GetDataSet("GetForAction_A_NCR", par, 1);
                dt = ds.Tables[0];
            }

            if (dt != null)
            {

                DataRow firstRow = dt.Rows[0]; // Get the first row of the DataTable

                if (NCNO != "" && NCNO != null)
                {
                    model.QtyPassed = Convert.ToInt32(firstRow["QTY_PASSED"]);
                    model.Item = firstRow["ITEM_DESC_PO"].ToString();
                    if (!firstRow.IsNull("NC_DATE"))
                    {
                        model.NCRDate = Convert.ToDateTime(firstRow["NC_DATE"]);
                    }
                }
                model.CaseNo = firstRow["case_no"].ToString();
                model.PO_NO = firstRow["po_no"].ToString();
                model.BKNo = firstRow["bk_no"].ToString();
                model.SetNo = firstRow["set_no"].ToString();
                model.CONSIGNEE = firstRow["CONSIGNEE"].ToString();
                model.CONSIGNEE_CD = firstRow["CONSIGNEE_CD"].ToString();
                model.Vendor = firstRow["vendor"].ToString();
                model.CALL_SNO = Convert.ToInt32(firstRow["CALL_SNO"]);

                // Parse CALL_RECV_DT if it's not null
                if (!firstRow.IsNull("CALL_RECV_DT"))
                {
                    DateTime callRecvDate;
                    if (DateTime.TryParseExact(firstRow["CALL_RECV_DT"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out callRecvDate))
                    {
                        model.CALL_RECV_DT = callRecvDate;
                    }
                }

                model.IeCd = Convert.ToInt32(firstRow["IE_CD"]);
                model.IE_SNAME = firstRow["IE_NAME"].ToString();

                // Parse IC_DATE if it's not null
                if (!firstRow.IsNull("IC_DATE"))
                {
                    model.ICDate = Convert.ToDateTime(firstRow["IC_DATE"]);
                }
               
                model.IC_NO = firstRow["IC_NO"].ToString();

                // Parse PO_DT if it's not null
                if (!firstRow.IsNull("PO_DT"))
                {
                    model.PO_DT = Convert.ToDateTime(firstRow["PO_DT"]);
                }

            }
            return model;
        }
    }
}
