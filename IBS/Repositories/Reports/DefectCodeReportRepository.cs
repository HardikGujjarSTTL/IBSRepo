using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class DefectCodeReportRepository : IDefectCodeReportRepository
    {
        private readonly ModelContext context;

        public DefectCodeReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public DefectCodeReport GetDefectCodeWiseData(DateTime FromDate, DateTime ToDate,string Region)
        {
            DefectCodeReport model = new();
            List<DefectCodeList> lstDefectCodeList = new();
            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_From_date", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_To_date", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("Defect_Code_Wise_Report", parameter,1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<DefectCodeList> listcong = dt.AsEnumerable().Select(row => new DefectCodeList
                {
                    Code = Convert.ToString(row["DEFECT_DESC"]),
                    Upheld = Convert.ToString(row["UPHELD"]),
                    Sorting = Convert.ToString(row["SORTING"]),
                    Rectification = Convert.ToString(row["RECTIFICATION"]),
                    PriceReduction = Convert.ToString(row["PRICE_REDUCTION"]),
                }).ToList();
                model.lstDefectCodeList = listcong;
            }

            return model;
        }
    }
}
