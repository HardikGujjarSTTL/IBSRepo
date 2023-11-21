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
using System.Dynamic;

namespace IBSAPI.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ModelContext context;
        public InspectionRepository(ModelContext context)
        {
            this.context = context;
        }

        #region IE Methods
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

        public List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, DateTime CurrDate)
        {
            List<PendingInspectionModel> pendingList = new();
            var CurrentDate = CurrDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATE", OracleDbType.Varchar2, CurrentDate, ParameterDirection.Input);
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

        public List<DateWiseRecentInspectionModel> GetDateWiseRecentInspection(int IeCd, DateTime FromDate, DateTime ToDate)
        {
            List<DateWiseRecentInspectionModel> recentInspDetail = new List<DateWiseRecentInspectionModel>();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GETDATEWISERECENTINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                recentInspDetail = JsonConvert.DeserializeObject<List<DateWiseRecentInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return recentInspDetail;
        }

        public List<CompleteInspectionModel> GetCompleteInspection(int IeCd)
        {
            List<CompleteInspectionModel> completeInspList = new List<CompleteInspectionModel>();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GETCOMPLETEDINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                completeInspList = JsonConvert.DeserializeObject<List<CompleteInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return completeInspList;
        }
        #endregion

        #region Vendor Methods
        public List<VendorPedingInspectionModel> Get_Vendor_PendingInspection(int Vend_Cd, DateTime FromDate, DateTime ToDate)
        {
            List<VendorPedingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[2] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_VENDOR_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<VendorPedingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }
        #endregion

        #region CM Methods
        public List<RecentInspectionModel> Get_CM_RecentInspection(int CO_CD, DateTime CurrDate)
        {             
            List<RecentInspectionModel> recentInspList = new();
            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("P_VISITDATE", OracleDbType.Varchar2, CurrDate.ToString("dd/MM/yyyy"), ParameterDirection.Input);
            par[1] = new OracleParameter("P_CO_CD", OracleDbType.Int32, CO_CD, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CM_RECENTINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                recentInspList = JsonConvert.DeserializeObject<List<RecentInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return recentInspList;
        }
        #endregion

        #region Client Methods
        public List<PendingInspectionModel> Get_Client_PendingInspection(string Rly_CD, string Rly_NonType, DateTime FromDate, DateTime ToDate)
        {
            List<PendingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");

            OracleParameter[] par = new OracleParameter[5];
            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Rly_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONTYPE", OracleDbType.Varchar2, Rly_NonType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CLIENT_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }

        public List<PendingInspectionModel> Get_Client_Region_Wise_PendingInspection(string Rly_CD, string Rly_NonType,string Region, DateTime FromDate, DateTime ToDate)
        {
            List<PendingInspectionModel> pendingInspList = new();
            var FrmDT = FromDate.ToString("dd/MM/yyyy");
            var ToDT = ToDate.ToString("dd/MM/yyyy");
            Region =  string.IsNullOrEmpty(Region) ? null : Region;
            OracleParameter[] par = new OracleParameter[6];
            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Rly_CD, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONTYPE", OracleDbType.Varchar2, Rly_NonType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FrmDT, ParameterDirection.Input);
            par[3] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDT, ParameterDirection.Input);
            par[4] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[5] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GET_CLIENT_REGION_WISE_PENDINGINSPECTION_API", par, 1);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                pendingInspList = JsonConvert.DeserializeObject<List<PendingInspectionModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }).ToList();
            }
            return pendingInspList;
        }
        #endregion
    }
}
