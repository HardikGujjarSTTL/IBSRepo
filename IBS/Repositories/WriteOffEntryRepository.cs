using Humanizer;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Numerics;
using Microsoft.AspNetCore.Http;
using IBS.Models.Reports;
using IBS.Interfaces.Reports;

namespace IBS.Repositories
{
    public class WriteOffEntryRepository : IWriteOffEntryRepository
    {
        private readonly ModelContext context;

        public WriteOffEntryRepository(ModelContext context)
        {
            this.context = context;
        }
        public DTResult<WriteOffEntryModel> GetWriteOfEntryList(DTParameters dtParameters)
        {
            DTResult<WriteOffEntryModel> dTResult = new() { draw = 0 };
            IQueryable<WriteOffEntryModel>? query = null;
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "Bill_No";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "Bill_No";
                    orderAscendingDirection = true;
                }

                string FrmDt = null, ToDt = null, BPOName = null;

                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FrmDt"]))
                {
                    FrmDt = Convert.ToString(dtParameters.AdditionalValues["FrmDt"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDt"]))
                {
                    ToDt = Convert.ToString(dtParameters.AdditionalValues["ToDt"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BPOName"]))
                {
                    BPOName = Convert.ToString(dtParameters.AdditionalValues["BPOName"]);
                }

                DataTable dt = new DataTable();

                DataSet ds;
               
                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_frm_dt", OracleDbType.Varchar2, FrmDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_to_dt", OracleDbType.Varchar2, ToDt, ParameterDirection.Input);
                par[2] = new OracleParameter("p_bpo_CD", OracleDbType.Varchar2, BPOName, ParameterDirection.Input);
                par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                ds = DataAccessDB.GetDataSet("GetWriteOffEntryDetails", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<WriteOffEntryModel> list = dt.AsEnumerable().Select(row => new WriteOffEntryModel
                    {
                        Bill_No = Convert.ToString(row["BILL_NO"]),
                        BillDt = Convert.ToDateTime(row["BILL_DT"]),
                        BillAmt = Convert.ToDecimal(row["BILL_AMOUNT"]),
                        BillAmtRec = Convert.ToDecimal(row["AMOUNT_RECEIVED"]),
                        BillAmtClr = Convert.ToDecimal(row["BILL_AMT_CLEARED"]),
                        WRITE_OFF_AMT = (row["WRITE_OFF_AMT"] != DBNull.Value) ? Convert.ToDecimal(row["WRITE_OFF_AMT"]) : 0,
                }).ToList();

                    query = list.AsQueryable();

                    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                    if (!string.IsNullOrEmpty(searchBy))
                        query = query.Where(w => Convert.ToString(w.Bill_No).ToLower().Contains(searchBy.ToLower())
                        || Convert.ToString(w.BillDt).ToLower().Contains(searchBy.ToLower())
                        );

                    dTResult.recordsFiltered = query.Count();

                    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                    dTResult.draw = dtParameters.Draw;

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
