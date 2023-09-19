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

namespace IBS.Repositories
{
    public class ConsigneeCompPeriodRepository : IConsigneeCompPeriodRepository
    {
        private readonly ModelContext context;

        public ConsigneeCompPeriodRepository(ModelContext context)
        {
            this.context = context;
        }

        public ConsigneeCompPeriodReport GetCompPeriodData(string FromDate, string ToDate, string Allregion, string regionorth, string regionsouth, string regioneast, string regionwest, string jiallregion,
            string jinorth, string jisourth, string jieast, string jiwest, string compallregion, string compyes, string compno, string cancelled, string underconsider, string allaction, string particilaraction, string actiondrp,
            string actioncodedrp, string actionjidrp)
        {
            ConsigneeCompPeriodReport model = new();
            List<ConsigneeComplaintsReportModel> lstConsigneeComplaints = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.FromDate = FromDate; model.ToDate = ToDate; model.Allregion = Allregion; model.regionorth = regionorth; model.regionsouth = regionsouth; model.regioneast = regioneast; model.regionwest = regionwest;
            model.jiallregion = jiallregion; model.jinorth = jinorth; model.jisourth = jisourth; model.jieast = jieast; model.jiwest = jiwest; model.compallregion = compallregion;
            model.compyes = compyes; model.compno = compno; model.cancelled = cancelled; model.underconsider = underconsider; model.allaction = allaction; model.particilaraction = particilaraction; model.actiondrp = actiondrp;
            model.actioncodedrp = actioncodedrp; model.actionjidrp = actionjidrp;

            OracleParameter[] parameter = new OracleParameter[6];
            parameter[0] = new OracleParameter("p_Fromdate", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_Todate", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_lstAction", OracleDbType.Varchar2, actiondrp, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_lstClassification", OracleDbType.Varchar2, actionjidrp, ParameterDirection.Input);
            parameter[4] = new OracleParameter("p_lstDefectCd", OracleDbType.Varchar2, actioncodedrp, ParameterDirection.Input);
            parameter[5] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);
            ds = DataAccessDB.GetDataSet("compliants_Period_Report", parameter, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<ConsigneeComplaintsReportModel> listcong = dt.AsEnumerable().Select(row => new ConsigneeComplaintsReportModel
                {
                    IN_REGION = Convert.ToString(row["IN_REGION"]),
                    COMPLAINT_ID = Convert.ToString(row["COMPLAINT_ID"]),
                    JI_SNO = Convert.ToString(row["JI_SNO"]),
                    VENDOR = Convert.ToString(row["VENDOR"]),
                    PO_NO = Convert.ToString(row["PO_NO"]),
                    PO_DATE = Convert.ToString(row["PO_DATE"]),
                    BK_SET = Convert.ToString(row["BK_SET"]),
                    IC_DATE = Convert.ToString(row["IC_DATE"]),
                    ITEM_DESC = Convert.ToString(row["ITEM_DESC"]),
                    CONSIGNEE = Convert.ToString(row["CONSIGNEE"]),
                    IE_NAME = Convert.ToString(row["IE_NAME"]),
                    QTY_OFF = Convert.ToString(row["QTY_OFF"]),
                    QTY_REJECTED = Convert.ToString(row["QTY_REJECTED"]),
                    REJECTION_VALUE = Convert.ToString(row["REJECTION_VALUE"]),
                    DEPT = Convert.ToString(row["DEPT"]),
                    COMPLAINT_DATE = Convert.ToString(row["COMPLAINT_DATE"]),
                    REJECTIONMEMOPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.RejectionMemo), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),// "/REJECTION_MEMO/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    REJECTION_REASON = Convert.ToString(row["REJECTION_REASON"]),
                    NO_JI_RES = Convert.ToString(row["NO_JI_RES"]),
                    JI_DATE = Convert.ToString(row["JI_DATE"]),
                    COMPLAINTSCASESPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ComplaintCase), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_CASES/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    STATUS = Convert.ToString(row["STATUS"]),
                    COMPLAINTSREPORTPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.COMPLAINTSREPORT), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_REPORT/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                    DEFECT_DESC = Convert.ToString(row["DEFECT_DESC"]),
                    JI_STATUS_DESC = Convert.ToString(row["JI_STATUS_DESC"]),
                    CONCLUSION_DATE = Convert.ToString(row["CONCLUSION_DATE"]),
                    CO_NAME = Convert.ToString(row["CO_NAME"]),
                    JI_IE_NAME = Convert.ToString(row["JI_IE_NAME"]),
                    ROOT_CAUSE_ANALYSIS = Convert.ToString(row["ROOT_CAUSE_ANALYSIS"]),
                    CHK_STATUS = Convert.ToString(row["CHK_STATUS"]),

                    TECH_REF = Convert.ToString(row["TECH_REF"]),
                    ACTION_PROPOSED = Convert.ToString(row["ACTION_PROPOSED"]),
                    ANY_OTHER = Convert.ToString(row["ANY_OTHER"]),
                    CAPA_STATUS = Convert.ToString(row["CAPA_STATUS"]),
                    DANDAR_STATUS = Convert.ToString(row["DANDAR_STATUS"]),

                    CASE_NO = Convert.ToString(row["CASE_NO"]),
                    BK_NO = Convert.ToString(row["BK_NO"]),
                    SET_NO = Convert.ToString(row["SET_NO"]),
                }).ToList();
                model.lstConsigneeComplaints = listcong;
            }

            return model;
        }
    }
}
