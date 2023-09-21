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

        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod,string Region, string controllingmanager, string reporttype, string iename)
        {
            NCRReport model = new();
            List<AllNCRCMIE> lstAllNCRCMIE = new();
            List<IECMWise> lstIECMWise = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.month = month; model.year = year; model.AllCM = AllCM; model.FromDate = FromDate; model.ToDate = ToDate; model.forCM = forCM; model.Outstanding = Outstanding; model.formonth = formonth; model.forperiod = forperiod;

            if (forCM == "true")
            {
                OracleParameter[] par = new OracleParameter[10];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("p_reptype", OracleDbType.Varchar2, reporttype, ParameterDirection.Input);
                par[2] = new OracleParameter("p_out", OracleDbType.Varchar2, Outstanding, ParameterDirection.Input);
                par[3] = new OracleParameter("p_fromdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
                par[4] = new OracleParameter("p_todate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
                par[5] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year+month, ParameterDirection.Input);
                par[6] = new OracleParameter("p_rdomonth", OracleDbType.Varchar2,formonth, ParameterDirection.Input);
                par[7] = new OracleParameter("p_lstCO", OracleDbType.Varchar2,controllingmanager, ParameterDirection.Input);
                par[8] = new OracleParameter("p_lstIE", OracleDbType.Varchar2, iename, ParameterDirection.Input);
                par[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetCMandIEWiseReport", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<IECMWise> listcong = dt.AsEnumerable().Select(row => new IECMWise
                    {
                        CASE_NO = row.Field<string>("CASE_NO"),
                        NCR_NO = row.Field<string>("NCR_NO"),
                        ITEM = row.Field<string>("ITEM"),
                        VENDOR = row.Field<string>("VENDOR"),
                        IE_NAME = row.Field<string>("IE_NAME"),
                        CO_NAME = row.Field<string>("CO_NAME"),
                        NC = row.Field<string>("NC"),
                        NC_CD_SNO = row.Field<string>("NC_CD_SNO"),
                        IE_ACTION1 = row.Field<string>("IE_ACTION1"),
                        IE_ACTION_DATE = row.Field<string>("IE_ACTION_DATE"),
                        CO_FINAL_REMARKS1 = row.Field<string>("CO_FINAL_REMARKS1"),
                        CO_REMARK_DATE = row.Field<string>("CO_REMARK_DATE"),
                    }).ToList();

                    model.lstIECMWise = listcong;
                }
            }
            else if (AllCM == "true")
            {
                OracleParameter[] par = new OracleParameter[7];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("p_reptype", OracleDbType.Varchar2, reporttype, ParameterDirection.Input);
                par[2] = new OracleParameter("p_fromdt", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
                par[3] = new OracleParameter("p_todate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
                par[4] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year + month, ParameterDirection.Input);
                par[5] = new OracleParameter("p_rdomonth", OracleDbType.Varchar2, formonth, ParameterDirection.Input);
                par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetAllCMandIEWiseReport", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<AllNCRCMIE> listcong = dt.AsEnumerable().Select(row => new AllNCRCMIE
                    {
                        IECMName = row.Field<string>("BothName"),
                        Total_NO_Call = row.Field<string>("TOTAL_NO_CALLS"),
                        Total_NC = row.Field<string>("TOTAL_NC"),
                        Total_Minor = row.Field<string>("TOTAL_MINOR"),
                        Total_Major = row.Field<string>("TOTAL_MAJOR"),
                        Total_Critical = row.Field<string>("TOTAL_CRITICAL"),
                        NO_NC = row.Field<string>("NC_NO"),
                    }).ToList();
                    model.lstAllNCRCMIE = listcong;
                }
            }

            return model;
        }
    }
}
