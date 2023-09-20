using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
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
            parameter[0] = new OracleParameter("p_From_date", OracleDbType.Varchar2, FromDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_To_date", OracleDbType.Varchar2, ToDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_Region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("Defect_Code_Wise_Report", parameter,1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<DefectCodeList> listcong = dt.AsEnumerable().Select(row => new DefectCodeList
                {
                    Code = Convert.ToString(row["DEFECT_DESC"]),
                    Upheld = Convert.ToDecimal(row["UPHELD"]),
                    Sorting = Convert.ToDecimal(row["SORTING"]),
                    Rectification = Convert.ToDecimal(row["RECTIFICATION"]),
                    PriceReduction = Convert.ToDecimal(row["PRICE_REDUCTION"]),
                }).ToList();
                foreach (var item in listcong)
                {
                    item.Total = (decimal)item.Upheld + (decimal)item.Sorting + (decimal)item.Rectification + (decimal)item.PriceReduction;
                }
                model.lstDefectCodeList = listcong;
            }

            model.FromDate = FromDate;
            model.ToDate = ToDate;

            return model;
        }
    }
}
