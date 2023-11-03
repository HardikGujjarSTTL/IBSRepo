using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Numerics;
using Oracle.ManagedDataAccess.Client;
using IBSAPI.Helper;
using Newtonsoft.Json;
using System.Data;

namespace IBSAPI.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ModelContext context;
        public InspectionRepository(ModelContext context)
        {
            this.context = context;
        }
        public List<TodayInspectionModel> GetToDayInspection(int IeCd)
        {
            List<TodayInspectionModel> todayList = new();
            var currDate = DateTime.Now.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODAYDATE", OracleDbType.Varchar2, currDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GETTODAYINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                todayList = JsonConvert.DeserializeObject<List<TodayInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return todayList;
        }

        public List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd)
        {
            var tomoDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
            List<TomorrowInspectionModel> tomorrowList = new();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TOMORROWDATE", OracleDbType.Varchar2, tomoDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GETTOMORROWINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                tomorrowList = JsonConvert.DeserializeObject<List<TomorrowInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return tomorrowList;
        }

        public List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, string CurrDate)
        {
            List<PendingInspectionModel> pendingList = new();
            CurrDate = Convert.ToDateTime(CurrDate).ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, CurrDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GETPENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingList;            
        }
        public CaseDetailIEModel GetCaseDetailForIE(string Case_No, DateTime CallRecvDt, int CallSNo, int IeCd)
        {
            CaseDetailIEModel caseDetailIEModel = new CaseDetailIEModel();

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("p_IeCd", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("p_Case_No", OracleDbType.Varchar2, Case_No, ParameterDirection.Input);
            par[2] = new OracleParameter("p_CALL_RECV_DT", OracleDbType.Date, CallRecvDt, ParameterDirection.Input);
            par[3] = new OracleParameter("p_CALL_SNO", OracleDbType.Int32, CallSNo, ParameterDirection.Input);
            par[4] = new OracleParameter("p_Result", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetCaseDetailForIE_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                caseDetailIEModel = JsonConvert.DeserializeObject<List<CaseDetailIEModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).FirstOrDefault();
            }
            return caseDetailIEModel;
        }
    }
}
