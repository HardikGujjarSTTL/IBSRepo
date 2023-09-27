using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
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
    public class ContractsReportsRepository : IContractsReportsRepository
    {
        private readonly ModelContext context;

        public ContractsReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public ContractReportModel GetContractDetails(string FromDate, string ToDate, string Region, string clientname)
        {
            ContractReportModel model = new();
            List<Contrcats> lstContrcats = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("yyyyMMdd");
                formattedToDate = parsedToDate.ToString("yyyyMMdd");
            }

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("p_todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_bporegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[3] = new OracleParameter("p_clientname", OracleDbType.Varchar2, clientname, ParameterDirection.Input);
            par[4] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetContractsReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<Contrcats> listcong = dt.AsEnumerable().Select(row => new Contrcats
                {
                    CONTRACT_ID = Convert.ToString(row["CONTRACT_ID"]),
                    CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                    CONTRACT_NO = Convert.ToString(row["CONTRACT_NO"]),
                    PER_FROM = Convert.ToString(row["PER_FROM"]),
                    PER_TO = Convert.ToString(row["PER_TO"]),
                    CONTRACT_FEE_NUM = Convert.ToString(row["CONTRACT_FEE_NUM"]),
                    CO_NAME = Convert.ToString(row["CO_NAME"]),
                    CONTRACT_SPECIAL_CONDN = Convert.ToString(row["CONTRACT_SPECIAL_CONDN"]),
                    CONTRACT_PANALTY = Convert.ToString(row["CONTRACT_PANALTY"]),
                    CONT_INSP_FEE = Convert.ToString(row["CONT_INSP_FEE"]),
                    SCOPE_OF_WORK = Convert.ToString(row["SCOPE_OF_WORK"]),
                    REGION = Convert.ToString(row["REGION"]),
                }).ToList();
                model.lstContrcats = listcong;
            }

            return model;
        }
    }
}
