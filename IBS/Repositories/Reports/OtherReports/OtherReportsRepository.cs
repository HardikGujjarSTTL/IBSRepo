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
using System.Security.Cryptography;

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

            model.month = month; model.year = year; model.AllCM = AllCM; model.FromDate = Convert.ToDateTime(FromDate); model.ToDate = Convert.ToDateTime(ToDate); model.forCM = forCM; model.Outstanding = Outstanding; model.formonth = formonth; model.forperiod = forperiod;

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("yyyy/MM/dd").Replace("/", "");
                formattedToDate = parsedToDate.ToString("yyyy/MM/dd").Replace("/", "");
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

        public ContractReportModel GetContractDetails(string FromDate, string ToDate, string Region, string clientname)
        {
            ContractReportModel model = new();
            List<Contrcats> lstContrcats = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

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

        public VendorClusterReportModel GetVendorClusterReport(string departreport, string Region)
        {
            VendorClusterReportModel model = new();
            List<VendorClusterList> lstVendorClusterList = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[3];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_department", OracleDbType.Varchar2, departreport, ParameterDirection.Input);
            par[2] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorClusterAllReport", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorClusterList> listcong = dt.AsEnumerable().Select(row => new VendorClusterList
                {
                    Department = Convert.ToString(row["DEPARTMENT"]),
                    Cluster_name = Convert.ToString(row["CLUSTER_NAME"]),
                    geographical_partition = Convert.ToString(row["geographical_partition"]),
                    Vend_cd = Convert.ToString(row["VEND_CD"]),
                    vendor = Convert.ToString(row["VEND_NAME"]),
                    vend_add1 = Convert.ToString(row["VEND_ADD1"]),
                    city = Convert.ToString(row["CITY"]),
                    Ie_name = Convert.ToString(row["IE_NAME"]),

                }).ToList();
                model.lstVendorClusterList = listcong;
            }

            return model;
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

        public VendorPerformanceReportModel GetVendorperformanceReport(string FromDate, string ToDate, string formonth, string forperiod, string month, string year, string vendcd, string Region)
        {
            VendorPerformanceReportModel model = new();
            List<VendorPerformance> lstVendorPerformance = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("yyyy/MM/dd");
                formattedToDate = parsedToDate.ToString("yyyy/MM/dd");
            }

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_lstvendor", OracleDbType.Varchar2, vendcd, ParameterDirection.Input);
            par[2] = new OracleParameter("p_rdbForMonth", OracleDbType.Varchar2, formonth, ParameterDirection.Input);
            par[3] = new OracleParameter("p_monthyear", OracleDbType.Varchar2, year + month, ParameterDirection.Input);
            par[4] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
            par[5] = new OracleParameter("p_todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
            par[6] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorPerformanceReports", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorPerformance> listcong = dt.AsEnumerable().Select(row => new VendorPerformance
                {
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    PO_NO = Convert.ToString(row["PO_NO"]),
                    PO_DATE = Convert.ToString(row["PO_DATE"]),
                    CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                    QTY_TO_INSP = Convert.ToString(row["QTY_TO_INSP"]),
                    UOM = Convert.ToString(row["UOM"]),
                    QTY_PASSED = Convert.ToString(row["QTY_PASSED"]),
                    QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                    REASON_REJECT = Convert.ToString(row["REASON_REJECT"]),
                    CALL_DATE = Convert.ToString(row["CALL_DATE"]),
                    FIRST_INSP_DATE = Convert.ToString(row["FIRST_INSP_DATE"]),
                    LAST_INSP_DATE = Convert.ToString(row["LAST_INSP_DATE"]),
                    CALL_STATUS_DESC = Convert.ToString(row["CALL_STATUS_DESC"]),
                    IC_NO = Convert.ToString(row["IC_NO"]),
                    IC_DATE = Convert.ToString(row["IC_DATE"]),

                }).ToList();
                model.lstVendorPerformance = listcong;
            }

            return model;
        }

        public VendorFeedbackReportModel GetVendorFeedbackReport(string Region)
        {
            VendorFeedbackReportModel model = new();
            List<VendorFeedbackReport> lstVendorFeedbackReport = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetVendorfeedbackReport", par, 1);


            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<VendorFeedbackReport> listcong = dt.AsEnumerable().Select(row => new VendorFeedbackReport
                {
                    Vendor = Convert.ToString(row["VENDOR"]),
                    Region = Convert.ToString(row["REGION"]),
                    FIELD_1 = Convert.IsDBNull(row["FIELD_1"]) ? 0 : Convert.ToInt32(row["FIELD_1"]),
                    FIELD_2 = Convert.IsDBNull(row["FIELD_2"]) ? 0 : Convert.ToInt32(row["FIELD_2"]),
                    FIELD_3 = Convert.IsDBNull(row["FIELD_3"]) ? 0 : Convert.ToInt32(row["FIELD_3"]),
                    FIELD_4 = Convert.IsDBNull(row["FIELD_4"]) ? 0 : Convert.ToInt32(row["FIELD_4"]),
                    FIELD_5 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_5"]),
                    FIELD_6 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_6"]),
                    FIELD_7 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_7"]),
                    FIELD_8 = Convert.IsDBNull(row["FIELD_5"]) ? 0 : Convert.ToInt32(row["FIELD_8"]),
                    FIELD_9 = Convert.IsDBNull(row["FIELD_9"]) ? string.Empty : Convert.ToString(row["FIELD_9"]),
                    FIELD_10 = Convert.IsDBNull(row["FIELD_10"]) ? string.Empty : Convert.ToString(row["FIELD_10"]),
                    MTHYR_PERIOD = Convert.IsDBNull(row["MTHYR_PERIOD"]) ? 0 : Convert.ToInt32(row["MTHYR_PERIOD"]),
                }).ToList();

                model.lstVendorFeedbackReport = listcong;
            }

            return model;
        }


        public PeriodWiseChecksheetReportModel Getperiodwisechecksheetdetails(string FromDate, string ToDate, string Region)
        {
            PeriodWiseChecksheetReportModel model = new();
            List<PeriodWiseChecksheet> lstPeriodWiseChecksheet = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("dd/MM/yyyy");
                formattedToDate = parsedToDate.ToString("dd/MM/yyyy");
            }

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetPeriodWiseChecksheetReports", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<PeriodWiseChecksheet> listcong = dt.AsEnumerable().Select(row => new PeriodWiseChecksheet
                {
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    IE = Convert.ToString(row["IE"]),
                    CO_NAME = Convert.ToString(row["CO_NAME"]),
                    CREATION_REV_DT = Convert.ToString(row["CREATION_REV_DT"]),
                }).ToList();

                model.lstPeriodWiseChecksheet = listcong;
            }

            return model;
        }


        public PeriodWiseTechnicalRefReportModel Getperiodwisetechrefdetails(string FromDate, string ToDate, string Region)
        {
            PeriodWiseTechnicalRefReportModel model = new();
            List<PeriodWiseTechnicalRef> lstPeriodWiseTechnicalRef = new();

            DataSet ds = null;
            DataTable dt = new DataTable();
            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("dd/MM/yyyy");
                formattedToDate = parsedToDate.ToString("dd/MM/yyyy");
            }
            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
            par[3] = new OracleParameter("p_ResultSet", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetPeriodWiseTechRefReport", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<PeriodWiseTechnicalRef> listcong = dt.AsEnumerable().Select(row => new PeriodWiseTechnicalRef
                {
                    cm_name = Convert.ToString(row["cm_name"]),
                    ie_name = Convert.ToString(row["ie_name"]),
                    item_des = Convert.ToString(row["item_des"]),
                    spec_drg = Convert.ToString(row["spec_drg"]),
                    letter_no = Convert.ToString(row["letter_no"]),
                    tech_date = Convert.ToString(row["tech_date"]),
                    ref_made = Convert.ToString(row["ref_made"]),
                    tech_content = Convert.ToString(row["tech_content"]),
                }).ToList();

                model.lstPeriodWiseTechnicalRef = listcong;
            }

            return model;
        }

        public DailyIECMWorkPlanReportModel GetDailyWorkData(string FromDate, string ToDate, string lstIE, string lstCM, string AllIEs, string ParticularIEs, string AllCM, string ParticularCMs, string ReportType, string IEWise, string CMWise, string Region, string SortedIE, string visitdate)
        {
            DailyIECMWorkPlanReportModel model = new();
            List<DailyIECMWorkPlanReporttbl1> lstDailyIECMWorkPlanReporttbl1 = new();
            List<DailyIECMWorkPlanReporttbl2> lstDailyIECMWorkPlanReporttbl2 = new();
            List<DailyIECMWorkPlanReporttbl3> lstDailyIECMWorkPlanReporttbl3 = new();
            List<DailyIEWorklanExcepReport> lstDailyIEWorklanExcepReport = new();

            DataSet ds = null;
            DataSet ds2 = null;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();


            string formattedFromDate = "";
            string formattedToDate = "";

            if (FromDate != null && ToDate != null && Convert.ToDateTime(FromDate) != DateTime.MinValue && Convert.ToDateTime(ToDate) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime parsedToDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("yyyyMMdd");
                formattedToDate = parsedToDate.ToString("yyyyMMdd");
            }

            if (ReportType == "U")
            {

                OracleParameter[] par = new OracleParameter[11];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("rdbIEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                par[2] = new OracleParameter("rdbCOWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                par[3] = new OracleParameter("FrmDt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[4] = new OracleParameter("ToDt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[5] = new OracleParameter("lstIE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                par[6] = new OracleParameter("lstCO", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                par[7] = new OracleParameter("rdopartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                par[8] = new OracleParameter("rdopartCM", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                par[9] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
                par[10] = new OracleParameter("p_result1", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetDailyIEWorkPlanReport", par, 2);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<DailyIECMWorkPlanReporttbl1> listcong = dt.AsEnumerable().Select(row => new DailyIECMWorkPlanReporttbl1
                    {
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        VISIT_DATE = Convert.ToString(row["VISIT_DATE"]),
                        LOGIN_TIME = Convert.ToString(row["LOGIN_TIME"]),
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DATE = Convert.ToString(row["CALL_RECV_DATE"]),
                        DESIRE_DT = Convert.ToString(row["DESIRE_DT"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        CHK_COUNT = Convert.ToString(row["CHK_COUNT"]),
                        MFG_NAME = Convert.ToString(row["MFG_NAME"]),
                        MFG_PLACE = Convert.ToString(row["MFG_PLACE"]),
                        MFG_CITY = Convert.ToString(row["MFG_CITY"]),
                        ITEM_DESC_PO = Convert.ToString(row["ITEM_DESC_PO"]),
                        VALUE = Convert.ToString(row["VALUE"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                    }).ToList();
                    model.lstDailyIECMWorkPlanReporttbl1 = listcong;

                    dt1 = ds.Tables[1];
                    List<DailyIECMWorkPlanReporttbl2> listcong1 = dt1.AsEnumerable().Select(row => new DailyIECMWorkPlanReporttbl2
                    {
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        WORK_DATE = Convert.ToString(row["WORK_DATE"]),
                        LOGIN_TIME = Convert.ToString(row["LOGIN_TIME"]),
                        NI_WORK_PLAN_CD = Convert.ToString(row["NI_WORK_PLAN_CD"]),
                    }).ToList();
                    model.lstDailyIECMWorkPlanReporttbl2 = listcong1;
                }

                var dates = Enumerable.Range(0, (int)(Convert.ToDateTime(ToDate) - Convert.ToDateTime(FromDate)).TotalDays + 1)
                        .Select(offset => Convert.ToDateTime(FromDate).AddDays(offset).ToString("dd/MM/yyyy"));
                foreach (var date in dates)
                {
                    OracleParameter[] par2 = new OracleParameter[11];
                    par2[0] = new OracleParameter("p_regioncode", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                    par2[1] = new OracleParameter("p_IEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                    par2[2] = new OracleParameter("p_AllIE", OracleDbType.Varchar2, AllIEs, ParameterDirection.Input);
                    par2[3] = new OracleParameter("p_PartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                    par2[4] = new OracleParameter("p_COWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                    par2[5] = new OracleParameter("p_AllCM", OracleDbType.Varchar2, AllCM, ParameterDirection.Input);
                    par2[6] = new OracleParameter("p_PartCM", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                    par2[7] = new OracleParameter("p_lst_IE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                    par2[8] = new OracleParameter("p_lst_CO", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                    par2[9] = new OracleParameter("p_date", OracleDbType.Varchar2, date, ParameterDirection.Input);
                    par2[10] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                    ds2 = DataAccessDB.GetDataSet("GetIEWorkPlan", par2, 1);

                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        dt = ds2.Tables[0];
                        List<DailyIECMWorkPlanReporttbl3> listcong = dt.AsEnumerable().Select(row => new DailyIECMWorkPlanReporttbl3
                        {
                            IE_NAME = Convert.ToString(row["IE_NAME"]),
                            CO_NAME = Convert.ToString(row["CO_NAME"]),
                        }).ToList();

                        if (model.lstDailyIECMWorkPlanReporttbl3 == null)
                        {
                            model.lstDailyIECMWorkPlanReporttbl3 = new List<DailyIECMWorkPlanReporttbl3>();
                        }

                        foreach (var item in listcong)
                        {
                            model.lstDailyIECMWorkPlanReporttbl3.Add(item);
                        }
                    }
                }

            }

            if (ReportType == "E")
            {

                OracleParameter[] par = new OracleParameter[11];
                par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                par[1] = new OracleParameter("rdbIEWise", OracleDbType.Varchar2, IEWise, ParameterDirection.Input);
                par[2] = new OracleParameter("rdbCOWise", OracleDbType.Varchar2, CMWise, ParameterDirection.Input);
                par[3] = new OracleParameter("rdbPartIE", OracleDbType.Varchar2, ParticularIEs, ParameterDirection.Input);
                par[4] = new OracleParameter("rdbPartCo", OracleDbType.Varchar2, ParticularCMs, ParameterDirection.Input);
                par[5] = new OracleParameter("rdbIESort", OracleDbType.Varchar2, SortedIE, ParameterDirection.Input);
                par[6] = new OracleParameter("frmdt", OracleDbType.Varchar2, formattedFromDate, ParameterDirection.Input);
                par[7] = new OracleParameter("todt", OracleDbType.Varchar2, formattedToDate, ParameterDirection.Input);
                par[8] = new OracleParameter("lstIE", OracleDbType.Varchar2, lstIE, ParameterDirection.Input);
                par[9] = new OracleParameter("lstCo", OracleDbType.Varchar2, lstCM, ParameterDirection.Input);
                par[10] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

                ds = DataAccessDB.GetDataSet("GetDailyIEWisePlanEXCEPTIONReport", par, 1);


                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    List<DailyIEWorklanExcepReport> listcong = dt.AsEnumerable().Select(row => new DailyIEWorklanExcepReport
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DATE = Convert.ToString(row["CALL_RECV_DATE"]),
                        CALL_SNO = Convert.ToString(row["CALL_SNO"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        BK_NO = Convert.ToString(row["BK_NO"]),
                        SET_NO = Convert.ToString(row["SET_NO"]),
                        NO_OF_INSP = Convert.ToString(row["NO_OF_INSP"]),
                        IC_DATE = Convert.ToString(row["IC_DATE"]),
                        FIRST_INSP_DATE = Convert.ToString(row["FIRST_INSP_DATE"]),
                        LAST_INSP_DATE = Convert.ToString(row["LAST_INSP_DATE"]),
                        VISIT_DATE = Convert.ToString(row["VISIT_DATE"]),
                    }).ToList();
                    model.lstDailyIEWorklanExcepReport = listcong;
                }

            }

            return model;
        }

        public DTResult<IEICPhotoEnclosedModelReport> GetDataList(DTParameters dtParameters, string Region)
        {
            DTResult<IEICPhotoEnclosedModelReport> dTResult = new() { draw = 0 };
            IQueryable<IEICPhotoEnclosedModelReport>? query = null;
            try
            {
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                    if (orderCriteria == "")
                    {
                        orderCriteria = "CaseNo";
                    }
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
                }
                else
                {
                    orderCriteria = "CaseNo";
                    orderAscendingDirection = true;
                }

                string CaseNo = "", CallRecDT = "", CallSno = null, BKNO = null, SETNO = null;

                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CaseNo"]))
                {
                    CaseNo = Convert.ToString(dtParameters.AdditionalValues["CaseNo"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallRecDT"]))
                {
                    CallRecDT = Convert.ToString(dtParameters.AdditionalValues["CallRecDT"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["CallSno"]))
                {
                    CallSno = Convert.ToString(dtParameters.AdditionalValues["CallSno"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["BKNO"]))
                {
                    BKNO = Convert.ToString(dtParameters.AdditionalValues["BKNO"]);
                }
                if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["SETNO"]))
                {
                    SETNO = Convert.ToString(dtParameters.AdditionalValues["SETNO"]);
                }

                IEICPhotoEnclosedModelReport model = new IEICPhotoEnclosedModelReport();
                DataTable dt = new DataTable();

                DataSet ds;

                string formattedFromDate = null;

                if (CallRecDT != null && CallRecDT != "" && Convert.ToDateTime(CallRecDT) != DateTime.MinValue)
                {
                    DateTime parsedFromDate = DateTime.ParseExact(CallRecDT, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    formattedFromDate = parsedFromDate.ToString("dd/MM/yyyy");
                }
                try
                {
                    OracleParameter[] par = new OracleParameter[7];
                    par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
                    par[1] = new OracleParameter("p_caseNO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
                    par[2] = new OracleParameter("p_recdt", OracleDbType.Date, formattedFromDate, ParameterDirection.Input); // Corrected type to OracleDbType.Date
                    par[3] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
                    par[4] = new OracleParameter("p_bkno", OracleDbType.Varchar2, BKNO, ParameterDirection.Input);
                    par[5] = new OracleParameter("p_setno", OracleDbType.Varchar2, SETNO, ParameterDirection.Input);
                    par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
                    ds = DataAccessDB.GetDataSet("GetIEICPhotoReport", par, 1);
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    List<IEICPhotoEnclosedModelReport> list = dt.AsEnumerable().Select(row => new IEICPhotoEnclosedModelReport
                    {
                        CaseNo = Convert.ToString(row["CASE_NO"]),
                        CallRecDT = Convert.ToString(row["CALL_DT"]),
                        CallSno = Convert.ToString(row["CALL_SNO"]),
                        BKNO = Convert.ToString(row["BK_NO"]),
                        SETNO = Convert.ToString(row["SET_NO"]),
                        FILE_1 = Convert.ToString(row["FILE_1"]),
                        FILE_2 = Convert.ToString(row["FILE_2"]),
                        FILE_3 = Convert.ToString(row["FILE_3"]),
                        FILE_4 = Convert.ToString(row["FILE_4"]),
                        FILE_5 = Convert.ToString(row["FILE_5"]),
                        FILE_6 = Convert.ToString(row["FILE_6"]),
                        FILE_7 = Convert.ToString(row["FILE_7"]),
                        FILE_8 = Convert.ToString(row["FILE_8"]),
                        FILE_9 = Convert.ToString(row["FILE_9"]),
                        FILE_10 = Convert.ToString(row["FILE_10"]),
                    }).ToList();

                    query = list.AsQueryable();

                    dTResult.recordsTotal = ds.Tables[0].Rows.Count;

                    dTResult.recordsFiltered = query.Count();

                    dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

                    dTResult.draw = dtParameters.Draw;

                }
                else
                {
                    return dTResult;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return dTResult;
        }

        public IEICPhotoEnclosedModelReport GetDataListReport(string CaseNo, string CallRecDT, string CallSno, string BKNO, string SETNO, string Region)
        {
            IEICPhotoEnclosedModelReport model = new();
            List<listSubmittedPhotobyIE> lstlistSubmittedPhotobyIE = new();

            DataSet ds = null;
            DataTable dt = new DataTable();


            string formattedFromDate = null;

            if (CallRecDT != null && CallRecDT != "" && Convert.ToDateTime(CallRecDT) != DateTime.MinValue)
            {
                DateTime parsedFromDate = DateTime.ParseExact(CallRecDT, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                formattedFromDate = parsedFromDate.ToString("dd/MM/yyyy");
            }

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            par[1] = new OracleParameter("p_caseNO", OracleDbType.Varchar2, CaseNo, ParameterDirection.Input);
            par[2] = new OracleParameter("p_recdt", OracleDbType.Date, formattedFromDate, ParameterDirection.Input); // Corrected type to OracleDbType.Date
            par[3] = new OracleParameter("p_callsno", OracleDbType.Varchar2, CallSno, ParameterDirection.Input);
            par[4] = new OracleParameter("p_bkno", OracleDbType.Varchar2, BKNO, ParameterDirection.Input);
            par[5] = new OracleParameter("p_setno", OracleDbType.Varchar2, SETNO, ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("GetIEICPhotoReport", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<listSubmittedPhotobyIE> listcong = dt.AsEnumerable().Select(row => new listSubmittedPhotobyIE
                {
                    CaseNo = Convert.ToString(row["CASE_NO"]),
                    CallRecDT = Convert.ToString(row["CALL_DT"]),
                    CallSno = Convert.ToString(row["CALL_SNO"]),
                    BKNO = Convert.ToString(row["BK_NO"]),
                    SETNO = Convert.ToString(row["SET_NO"]),
                    FILE_1 = Convert.ToString(row["FILE_1"]),
                    FILE_2 = Convert.ToString(row["FILE_2"]),
                    FILE_3 = Convert.ToString(row["FILE_3"]),
                    FILE_4 = Convert.ToString(row["FILE_4"]),
                    FILE_5 = Convert.ToString(row["FILE_5"]),
                    FILE_6 = Convert.ToString(row["FILE_6"]),
                    FILE_7 = Convert.ToString(row["FILE_7"]),
                    FILE_8 = Convert.ToString(row["FILE_8"]),
                    FILE_9 = Convert.ToString(row["FILE_9"]),
                    FILE_10 = Convert.ToString(row["FILE_10"]),

                }).ToList();
                model.lstlistSubmittedPhotobyIE = listcong;
            }

            return model;
        }
    }
}
