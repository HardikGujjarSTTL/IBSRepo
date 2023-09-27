using IBS.DataAccess;
using IBS.Helper;
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
    public class OngoingContractsReportRepository : IOngoingContractsReportRepository
    {
        private readonly ModelContext context;

        public OngoingContractsReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public OngoingContrcatsReportModel Getongoingcontractdetails(string StatusOffer, string Region, string StatusOffertxt, string Regiontxt, string rdoregionwise)
        {
            OngoingContrcatsReportModel model = new();
            List<OngoingContrcats> lstOngoingContrcats = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_rdoregion", OracleDbType.Varchar2, rdoregionwise, ParameterDirection.Input);
            par[1] = new OracleParameter("p_bporegion", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[2] = new OracleParameter("p_statusoffer", OracleDbType.Varchar2, StatusOffer, ParameterDirection.Input);
            par[3] = new OracleParameter("p_todaydate", OracleDbType.Varchar2, DateTime.Now.ToString("yyyyMMdd"), ParameterDirection.Input);
            par[4] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetOngoingContractsReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<OngoingContrcats> listcong = dt.AsEnumerable().Select(row => new OngoingContrcats
                {
                    CONTRACT_ID = Convert.ToString(row["CONTRACT_ID"]),
                    CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                    CONTRACT_NO = Convert.ToString(row["CONTRACT_NO"]),
                    OFFER_DTE = Convert.ToString(row["OFFER_DTE"]),
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
                model.lstOngoingContrcats = listcong;
            }

            return model;
        }
    }
}
