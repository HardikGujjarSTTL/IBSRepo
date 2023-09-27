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

namespace IBS.Repositories.Reports
{
    public class IEICPhotoEnclosedReportRepository : IIEICPhotoEnclosedReportRepository
    {
        private readonly ModelContext context;

        public IEICPhotoEnclosedReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<IEICPhotoEnclosedModelReport> GetDataList(DTParameters dtParameters,string Region)
        {
            DTResult<IEICPhotoEnclosedModelReport> dTResult = new() { draw = 0 };
            IQueryable<IEICPhotoEnclosedModelReport>? query = null;
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
                        orderCriteria = "CaseNo";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "CaseNo";
                    orderAscendingDirection = true;
                }

                string CaseNo = "", CallRecDT = "", CallSno = null, BKNO = null, SETNO = null;

                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
                {
                    CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecDT"]))
                {
                    CallRecDT = Convert.ToString(dtParameters.AdditionalValues["CallRecDT"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
                {
                    CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BKNO"]))
                {
                    BKNO = Convert.ToString(dtParameters.AdditionalValues["BKNO"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SETNO"]))
                {
                    SETNO = Convert.ToString(dtParameters.AdditionalValues["SETNO"]);
                }

                IEICPhotoEnclosedModelReport model = new IEICPhotoEnclosedModelReport();
                DataTable dt = new DataTable();

                DataSet ds;
                DateTime? parsedCallRecDT = null;
                if (CallRecDT != null && CallRecDT != "")
                {
                    parsedCallRecDT = DateTime.ParseExact(CallRecDT, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                try
                {
                    OracleParameter[] par = new OracleParameter[7];
                    par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_caseNO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_recdt", OracleDbType.Varchar2, parsedCallRecDT, ParameterDirection.Input);
                    par[3] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_bkno", OracleDbType.Varchar2, BKNO, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_setno", OracleDbType.Varchar2, SETNO, ParameterDirection.Input);
                    par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
                    ds = DataAccessDB.GetDataSet("GetIEICPhotoReport", par, 1);
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<IEICPhotoEnclosedModelReport> list = dt.AsEnumerable().Select(row => new IEICPhotoEnclosedModelReport
                    {
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        CallRecDT = Convert.ToString(row["CALL_DT"]),
                        CallSno = Convert.ToString(row["CALL_SNO"]),
                        BKNO = Convert.ToString(row["BK_NO"]),
                        SETNO = Convert.ToString(row["SET_NO"]),
                        FILE_1 = Convert.ToString(row["FILE_1"]),
                        FILE_2 = Convert.ToString(row["FILE_2"]),
                        FILE_3 = Convert.ToString(row["FILE_3"]),
                        FILE_4 = Convert.ToString(row["FILE_4"]),
                        FILE_5 = Convert.ToString(row["FILE_5"]),
                        FILE_6 = Convert.ToString(row["FILE_6"]),
                        FILE_7 = Convert.ToString(row["FILE_7"]),
                        FILE_8 = Convert.ToString(row["FILE_8"]),
                        FILE_9 = Convert.ToString(row["FILE_9"]),
                        FILE_10 = Convert.ToString(row["FILE_10"]),
                    }).ToList();

                    query = list.AsQueryable();

                    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

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
