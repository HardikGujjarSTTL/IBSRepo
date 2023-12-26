using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class MonthlyReportsRepository : IMonthlyReportsRepository
    {
        public AllICStatusModel GetAllICStatus(DateTime FromDate, DateTime ToDate, string IECD, string Region)
        {
            AllICStatusModel model = new();
            List<AllICStatusListModel> lstICStatus = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, model.Display_FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, model.Display_ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IECD, ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[4] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            DataSet ds = DataAccessDB.GetDataSet("SP_IC_ALL_STATUS_INSERT", parameter);
            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstICStatus = JsonConvert.DeserializeObject<List<AllICStatusListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            model.lstAllICStatus = lstICStatus;
            return model;
        }

        public ReInspectionICsModel GetReInspectionICs(DateTime FromDate, DateTime ToDate, string Region)
        {
            ReInspectionICsModel model = new();
            List<ReInspectionICsListModel> lstReInspectionIC = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, model.Display_FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, model.Display_ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("P_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REINSPECTIONICS", parameter);
            DataTable dt = ds.Tables[0];
            lstReInspectionIC = dt.AsEnumerable().Select(row => new ReInspectionICsListModel
            {
                BPO = row["BPO"].ToString(),
                BPO_CD = row["BPO_CD"].ToString(),
                BILL_NO = row["BILL_NO"].ToString(),
                BILL_DT = row["BILL_DT"].ToString(),
                PO_NO = row["PO_NO"].ToString(),
                PO_DT = row["PO_DT"].ToString(),
                CASE_NO = row["CASE_NO"].ToString(),
                RLY_CD = row["RLY_CD"].ToString(),
                BK_NO = row["BK_NO"].ToString(),
                SET_NO = row["SET_NO"].ToString(),
                IE_SNAME = row["IE_SNAME"].ToString(),
                Vendor = row["Vendor"].ToString(),
                IC_NO = row["IC_NO"].ToString(),
                IC_DT = row["IC_DT"].ToString(),
                BILL_AMOUNT = row["BILL_AMOUNT"].ToString(),
                INSP_FEE = row["INSP_FEE"].ToString(),
                SERVICE_TAX = row["SERVICE_TAX"].ToString(),
                TAX = row["TAX"].ToString(),
            }).ToList();
            model.lstReInspectionIC = lstReInspectionIC;
            return model;
        }
    }
}
