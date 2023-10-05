using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports.OtherReports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;

namespace IBS.Repositories.Reports.OtherReports
{
    public class OtherReportsRepository : IOtherReportsRepository
    {
        private readonly ModelContext context;

        public OtherReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public ControllingOfficerIEModel GetControllingOfficerWiseIE(string Region)
        {
            ControllingOfficerIEModel model = new();
            List<ControllingOfficerModel> lstControllingOfficer = new();
            List<ControllingOfficerModel> lstCluster = new();
            List<IEModel> lstLocalIE = new();
            List<IEModel> lstOutstationIE = new();
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);
            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[2] = new OracleParameter("P_TBL1_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[3] = new OracleParameter("P_TBL2_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[4] = new OracleParameter("P_TBL3_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CONTROLLING_OFFICER_WISE_IE", parameter, 4);
            string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            lstControllingOfficer = JsonConvert.DeserializeObject<List<ControllingOfficerModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstControllingOfficer = lstControllingOfficer;

            string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
            lstCluster = JsonConvert.DeserializeObject<List<ControllingOfficerModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstCluster = lstCluster;

            string serializeddt3 = JsonConvert.SerializeObject(ds.Tables[2], Formatting.Indented);
            lstLocalIE = JsonConvert.DeserializeObject<List<IEModel>>(serializeddt3, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstLocalIE = lstLocalIE;

            string serializeddt4 = JsonConvert.SerializeObject(ds.Tables[3], Formatting.Indented);
            lstOutstationIE = JsonConvert.DeserializeObject<List<IEModel>>(serializeddt4, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.lstOutstationIE = lstOutstationIE;
            return model;
        }

        public DTResult<CoIeWiseCallsListModel> GetCoIeWiseCalls(DTParameters dtParameters)//string CO, string Status, string IE, bool IsAllIE, bool IsCallDate)
        {
            DTResult<CoIeWiseCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<CoIeWiseCallsListModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "IE_NAME";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "IE_NAME";
                orderAscendingDirection = true;
            }

            string CO = null, Status = null, IE = null;
            bool IsAllIE = false, IsCallDate = false;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CO"]))
                CO = dtParameters.AdditionalValues["CO"];
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Status"]))
                Status = dtParameters.AdditionalValues["Status"];
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IE"]))
                IE = dtParameters.AdditionalValues["IE"];
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IsAllIE"]))
                IsAllIE = Convert.ToBoolean(dtParameters.AdditionalValues["IsAllIE"]);
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IsCallDate"]))
                IsCallDate = Convert.ToBoolean(dtParameters.AdditionalValues["IsCallDate"]);

            OracleParameter[] parameter = new OracleParameter[6];
            parameter[0] = new OracleParameter("P_CO", OracleDbType.Varchar2, CO, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_STATUS", OracleDbType.Varchar2, Status, ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_IE", OracleDbType.Varchar2, IE, ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_ISALLIE", OracleDbType.Int16, IsAllIE == true ? 1 : 0, ParameterDirection.Input);
            parameter[4] = new OracleParameter("P_ISCALLDATE", OracleDbType.Int16, IsCallDate == true ? 1 : 0, ParameterDirection.Input);
            parameter[5] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CO_IE_WISE_CALLS", parameter, 1);
            ////string serializedDt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            ////model = JsonConvert.DeserializeObject<List<CoIeWiseCallsListModel>>(serializedDt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            DataTable dt = ds.Tables[0];
            List<CoIeWiseCallsListModel> list = dt.AsEnumerable().Select(row => new CoIeWiseCallsListModel
            {
                VENDOR = row["VENDOR"].ToString(),
                MANUFACTURER = row["MANUFACTURER"].ToString(),
                VEND_CD = Convert.ToInt32(row["VEND_CD"]),
                MFG_CD = Convert.ToInt32(row["MFG_CD"]),
                CONSIGNEE = row["CONSIGNEE"].ToString(),
                ITEM_DESC_PO = row["ITEM_DESC_PO"].ToString(),
                EXT_DELV_DT = row["EXT_DELV_DT"].ToString(),
                CALL_MARK_DT = row["CALL_MARK_DT"].ToString(),
                INSP_DESIRE_DT = row["INSP_DESIRE_DT"].ToString(),
                CALL_RECV_DT = row["CALL_RECV_DT"].ToString(),
                IE_NAME = row["IE_NAME"].ToString(),
                IE_PHONE_NO = row["IE_PHONE_NO"].ToString(),
                PO_NO = row["PO_NO"].ToString(),
                PO_DATE = row["PO_DATE"].ToString(),
                PO_YR = row["PO_YR"].ToString(),
                PO_SOURCE = row["PO_SOURCE"].ToString(),
                SOURCE = row["SOURCE"].ToString(),
                RLY_CD = row["RLY_CD"].ToString(),
                CASE_NO = row["CASE_NO"].ToString(),
                USER_ID = row["USER_ID"].ToString(),
                DATETIME = row["DATETIME"].ToString(),
                REMARKS = row["REMARKS"].ToString(),
                NEW_VENDOR = row["NEW_VENDOR"].ToString(),
                CALL_STATUS = row["CALL_STATUS"].ToString(),
                CALL_STATUS_FULL = row["CALL_STATUS_FULL"].ToString(),
                COLOUR = row["COLOUR"].ToString(),
                MFG_PERS = row["MFG_PERS"].ToString(),
                MFG_PHONE = row["MFG_PHONE"].ToString(),
                CALL_SNO = row["CALL_SNO"].ToString(),
                IC_PHOTO = row["IC_PHOTO"].ToString(),
                IC_PHOTO_A1 = row["IC_PHOTO_A1"].ToString(),
                IC_PHOTO_A2 = row["IC_PHOTO_A2"].ToString(),
                COUNT = Convert.ToInt32(row["COUNT"])
            }).ToList();


            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].PO_SOURCE == "C")
                {
                    using ModelContext context = new(DbContextHelper.GetDbContextOptions());
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        bool wasOpen = command.Connection.State == ConnectionState.Open;
                        if (!wasOpen) command.Connection.Open();
                        try
                        {
                            command.CommandText = "Select IMMS_RLY_CD from T91_RAILWAYS WHERE RLY_CD='" + list[i].RLY_CD + "' ";
                            list[i].SS = Convert.ToString(command.ExecuteScalar());
                        }
                        finally
                        {
                            if (!wasOpen) command.Connection.Close();
                        }
                    }
                }

                if (Status == "M")
                {
                    using ModelContext context = new(DbContextHelper.GetDbContextOptions());
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        bool wasOpen = command.Connection.State == ConnectionState.Open;
                        if (!wasOpen) command.Connection.Open();
                        try
                        {
                            command.CommandText = "Select decode(nvl(count(*),0),0,'No','Yes') VISIT from T47_IE_WORK_PLAN where CASE_NO='" + list[i].CASE_NO.ToString() + "' and CALL_RECV_DT=to_date('" + list[i].CALL_RECV_DT + "','dd/mm/yyyy') and CALL_SNO='" + list[i].CALL_SNO + "' AND VISIT_DT=to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')";
                            list[i].VISIT = Convert.ToString(command.ExecuteScalar());
                        }
                        finally
                        {
                            if (!wasOpen) command.Connection.Close();
                        }
                    }
                }

                if (list[i].CALL_STATUS == "U")
                {
                    using ModelContext context = new(DbContextHelper.GetDbContextOptions());
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        bool wasOpen = command.Connection.State == ConnectionState.Open;
                        if (!wasOpen) command.Connection.Open();
                        try
                        {
                            command.CommandText = "SELECT DECODE(STATUS,'S','SAMPLE RECEIVED ON: '||TO_CHAR(SAMPLE_RECV_DT,'DD/MM/YYYY')||', TOTAL TESTING CHARGES ARE: Rs.'||DECODE(TESTING_CHARGES,0,'_',TESTING_CHARGES)||', LIKELY TEST REPORT RELEASE DATE IS: '||NVL(TO_CHAR(LIKELY_DT_REPORT,'DD/MM/YYYY'),'_'),'C','Lab Report under Compilation','U','Lab Report Uploaded on: '||to_char(LAB_REP_UPLOADED_DT,'dd/mm/yyyy-HH24:MI:SS'),'O','Others- '||REMARKS) FROM T109_LAB_SAMPLE_INFO where CASE_NO='" + list[i].CASE_NO + "' and CALL_RECV_DT=to_date('" + list[i].CALL_RECV_DT + "','dd/mm/yyyy') and CALL_SNO='" + list[i].CALL_SNO + "' ";
                            list[i].Lab_Status = Convert.ToString(command.ExecuteScalar());
                        }
                        finally
                        {
                            if (!wasOpen) command.Connection.Close();
                        }
                    }
                }
            }
            query = list.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                query = query.Where(x => x.IE_NAME.ToLower().Contains(searchBy.ToLower()));
            }
            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }

        public CoIeWiseCallsModel GetCoIeWiseCallsReport(string Case_No, string Call_Recv_Date, string Call_SNo)
        {
            CoIeWiseCallsModel model = new();
            List<CoIeWiseCallsList1Model> coIeWiseCallsList1s = new();
            List<CoIeWiseCallsList2Model> coIeWiseCallsList2s = new();

            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("P_CASE_NO", OracleDbType.Varchar2, Case_No, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_CALL_RECV_DT", OracleDbType.Varchar2, Call_Recv_Date, ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_CALL_SNO", OracleDbType.Varchar2, Call_SNo, ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_RESUT_CURSOR1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[4] = new OracleParameter("P_RESUT_CURSOR2", OracleDbType.RefCursor, ParameterDirection.Output);
            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CO_IE_WISE_CALL_REPORT", parameter, 2);

            //coIeWiseCallsList1s =
            string serializedDt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
            coIeWiseCallsList1s = JsonConvert.DeserializeObject<List<CoIeWiseCallsList1Model>>(serializedDt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.coIeWiseCallsList1 = coIeWiseCallsList1s;

            string serializedDt1 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
            coIeWiseCallsList2s = JsonConvert.DeserializeObject<List<CoIeWiseCallsList2Model>>(serializedDt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            model.coIeWiseCallsList2 = coIeWiseCallsList2s;
            return model;
        }

        public NCRReport GetNCRIECOWiseData(string month, string year, string FromDate, string ToDate, string AllCM, string forCM, string All, string Outstanding, string formonth, string forperiod, string Region, string iecmname, string reporttype)
        {
            NCRReport model = new();
            List<AllNCRCMIE> lstAllNCRCMIE = new();
            List<IECMWise> lstIECMWise = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.month = month; model.year = year; model.AllCM = AllCM; model.FromDate = FromDate; model.ToDate = ToDate; model.forCM = forCM; model.Outstanding = Outstanding; model.formonth = formonth; model.forperiod = forperiod;

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

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
                par[5] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year + month, ParameterDirection.Input);
                par[6] = new OracleParameter("p_rdomonth", OracleDbType.Varchar2, formonth, ParameterDirection.Input);
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

        public IEWiseTrainingReportModel GetIEWiseTrainingDetails(string IENAME, string TrainingArea, string Mechanical, string Electrical, string Civil, string Regular, string Deputaion, string Particularie, string ParticularArea, string Region)
        {
            IEWiseTrainingReportModel model = new();
            List<IEWISETRAINING> lstIEWISETRAINING = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[11];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("rdbMech", OracleDbType.Varchar2, Mechanical, ParameterDirection.Input);
            par[2] = new OracleParameter("rdbElec", OracleDbType.Varchar2, Electrical, ParameterDirection.Input);
            par[3] = new OracleParameter("rdbCiv", OracleDbType.Varchar2, Civil, ParameterDirection.Input);
            par[4] = new OracleParameter("rdbPIE", OracleDbType.Varchar2, Particularie, ParameterDirection.Input);
            par[5] = new OracleParameter("rdbRegular", OracleDbType.Varchar2, Regular, ParameterDirection.Input);
            par[6] = new OracleParameter("rdbDepu", OracleDbType.Varchar2, Deputaion, ParameterDirection.Input);
            par[7] = new OracleParameter("rdbPArea", OracleDbType.Varchar2, ParticularArea, ParameterDirection.Input);
            par[8] = new OracleParameter("lstIE", OracleDbType.Varchar2, IENAME, ParameterDirection.Input);
            par[9] = new OracleParameter("lstarea", OracleDbType.Varchar2, TrainingArea, ParameterDirection.Input);
            par[10] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetIEWiseTrainingReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<IEWISETRAINING> listcong = dt.AsEnumerable().Select(row => new IEWISETRAINING
                {
                    NAME = Convert.ToString(row["NAME"]),
                    EMP_NO = Convert.ToString(row["EMP_NO"]),
                    CATEGORY = Convert.ToString(row["CATEGORY"]),
                    QUAL = Convert.ToString(row["QUAL"]),
                    T_TYPE = Convert.ToString(row["T_TYPE"]),
                    T_FEILD = Convert.ToString(row["T_FEILD"]),
                    COURSE_NAME = Convert.ToString(row["COURSE_NAME"]),
                    COURSE_INSTITUTE = Convert.ToString(row["COURSE_INSTITUTE"]),
                    C_DUR_FR = Convert.ToString(row["C_DUR_FR"]),
                    C_DUR_TO = Convert.ToString(row["C_DUR_TO"]),
                    CERTIFICATE = Convert.ToString(row["CERTIFICATE"]),
                    FEES = Convert.ToString(row["FEES"]),
                    VALIDITY = Convert.ToString(row["VALIDITY"]),
                }).ToList();
                model.lstIEWISETRAINING = listcong;
            }

            return model;
        }
    }
}
