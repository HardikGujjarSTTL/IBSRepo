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
                            command.CommandText = "SELECT DECODE(STATUS,'S','SAMPLE RECEIVED ON: '||TO_CHAR(SAMPLE_RECV_DT,'DD/MM/YYYY')||', TOTAL TESTING CHARGES ARE: Rs.'||DECODE(TESTING_CHARGES,0,'_',TESTING_CHARGES)||', LIKELY TEST REPORT RELEASE DATE IS: '||NVL(TO_CHAR(LIKELY_DT_REPORT,'DD/MM/YYYY'),'_'),'C','Lab Report under Compilation','U','Lab Report Uploaded on: '||to_char(LAB_REP_UPLOADED_DT,'dd/mm/yyyy-HH24:MI:SS'),'O','Others- '||REMARKS) FROM T109_LAB_SAMPLE_INFO where CASE_NO='" + list[i].CASE_NO + "' and CALL_RECV_DT=to_date('" + list[i].CALL_RECV_DT + "','dd/mm/yyyy') and CALL_SNO='" + list[i].CALL_SNO+"' ";
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

            dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
    }
}
