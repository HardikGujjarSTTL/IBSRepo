using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Interfaces.Transaction;
using IBS.Models;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories.Reports
{
    public class IEAlterReportRepository : IIEAlterReportRepository
    {
        private readonly ModelContext context;

        public IEAlterReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public IEAlterMappingReportModel GetIEAlterMappingReport(string Region)
        {
            IEAlterMappingReportModel model = new();
            List<IEAlterMappingReport> lstIEAlterMappingReport = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetIEALTIEAllReport", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<IEAlterMappingReport> listcong = dt.AsEnumerable().Select(row => new IEAlterMappingReport
                {
                    IE_Name = Convert.ToString(row["IE_NAME"]),
                    Altie_Name = Convert.ToString(row["ALTIE_NAME"]),
                    Altie_two_name = Convert.ToString(row["ALTIE_two_NAME"]),
                    Altie_three_name = Convert.ToString(row["ALTIE_three_NAME"]),

                }).ToList();
                model.lstIEAlterMappingReport = listcong;
            }

            return model;
        }
    }
}
