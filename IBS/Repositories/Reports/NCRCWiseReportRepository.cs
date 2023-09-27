using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace IBS.Repositories.Reports
{
    public class NCRCWiseReportRepository : INCRCWiseReportRepository
    {
        private readonly ModelContext context;

        public NCRCWiseReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod,string Region, string iecmname, string reporttype)
        {
            NCRReport model = new();
            List<AllNCRCMIE> lstAllNCRCMIE = new();
            List<IECMWise> lstIECMWise = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.month = month; model.year = year; model.AllCM = AllCM; model.FromDate = FromDate; model.ToDate = ToDate; model.forCM = forCM; model.Outstanding = Outstanding; model.formonth = formonth; model.forperiod = forperiod;

            string formattedFromDate="";
            string formattedToDate="";

            if (FromDate != null && ToDate != null)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                 formattedFromDate = parsedFromDate.ToString("yyyy/MM/dd");
                 formattedToDate = parsedToDate.ToString("yyyy/MM/dd");
            }

            if (forCM == "true")
            {
                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("p_reptype", OracleDbType.Varchar2, reporttype, ParameterDirection.Input);
                par[2] = new OracleParameter("p_out", OracleDbType.Varchar2, Outstanding, ParameterDirection.Input);
                par[3] = new OracleParameter("p_fromdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[4] = new OracleParameter("p_todate", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[5] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year+month, ParameterDirection.Input);
                par[6] = new OracleParameter("p_rdomonth", OracleDbType.Varchar2,formonth, ParameterDirection.Input);
                par[7] = new OracleParameter("p_lstCO", OracleDbType.Varchar2, iecmname, ParameterDirection.Input);
                par[8] = new OracleParameter("p_lstIE", OracleDbType.Varchar2, iecmname, ParameterDirection.Input);
                par[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetCMandIEWiseReport", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<IECMWise> listcong = dt.AsEnumerable().Select(row => new IECMWise
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        NC_NO = Convert.ToString(row["NC_NO"]),
                        ITEM = Convert.ToString(row["ITEM"]),
                        VENDOR = Convert.ToString(row["VENDOR"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        NC = Convert.ToString(row["NC"]),
                        NC_CD_SNO = Convert.ToString(row["NC_CD_SNO"]),
                        IE_ACTION1 = Convert.ToString(row["IE_ACTION1"]),
                        CO_FINAL_REMARKS1 = Convert.ToString(row["NC"]),
                    }).ToList();

                    model.lstIECMWise = listcong;
                }
            }
            else if (AllCM == "true")
            {
                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("p_reptype", OracleDbType.Varchar2, reporttype, ParameterDirection.Input);
                par[2] = new OracleParameter("p_fromdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[3] = new OracleParameter("p_todate", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[4] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year + month, ParameterDirection.Input);
                par[5] = new OracleParameter("p_rdomonth", OracleDbType.Varchar2, formonth, ParameterDirection.Input);
                par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetAllCMandIEWiseReport", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<AllNCRCMIE> listcong = dt.AsEnumerable().Select(row => new AllNCRCMIE
                    {
                        IECMName = Convert.ToString(row["BOTHNAME"]),
                        Total_NO_Call = Convert.ToDecimal(row["TOTAL_NO_CALLS"]),
                        Total_NC = Convert.ToDecimal(row["TOTAL_NC"]),
                        Total_Minor = Convert.ToDecimal(row["TOTAL_MINOR"]),
                        Total_Major = Convert.ToDecimal(row["TOTAL_MAJOR"]),
                        Total_Critical = Convert.ToDecimal(row["TOTAL_CRITICAL"]),
                        NO_NC = Convert.ToDecimal(row["NO_NC"]),
                    }).ToList();
                    foreach (var item in listcong)
                    {
                        item.Total = (decimal)item.Total_Minor + (decimal)item.Total_Major + (decimal)item.Total_Critical;
                    }
                    model.lstAllNCRCMIE = listcong;
                }
            }

            return model;
        }
    }
}
