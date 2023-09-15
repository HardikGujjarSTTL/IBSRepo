using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class ConsigneeComplaintsReportRepository : IConsigneeComplaintsReportRepository
    {
        public List<ConsigneeComplaintsReportModel> Get_Consignee_Complaints(string FromDate, string ToDate, UserSessionModel model)
        {
            //DTResult<ConsigneeComplaintsReportModel> dTResult = new() { draw = 0 };
            //IQueryable<ConsigneeComplaintsReportModel>? query = null;

            //var searchBy = dtParameters.Search?.Value;
            //var orderCriteria = string.Empty;
            //var orderAscendingDirection = true;

            //if (dtParameters.Order != null && dtParameters.Order.Length > 0)
            //{
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
            //    if (string.IsNullOrEmpty(orderCriteria)) orderCriteria = "JI_SNO";
            //    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            //}
            //else
            //{
            //    orderCriteria = "JI_SNO";
            //    orderAscendingDirection = true;
            //}

            //string FromDate = null, ToDate = null;
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            //{
            //    FromDate = dtParameters.AdditionalValues["FromDate"];
            //}
            //if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            //{
            //    ToDate = dtParameters.AdditionalValues["ToDate"];
            //}

            OracleParameter[] par = new OracleParameter[4];
            par[0] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[1] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[2] = new OracleParameter("P_IECD", OracleDbType.Varchar2, model.IeCd, ParameterDirection.Input);
            par[3] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_CONSIGNEE_COMPLAINTS", par, 1);
            DataTable dt = ds.Tables[0];

            List<ConsigneeComplaintsReportModel> list = dt.AsEnumerable().Select(row => new ConsigneeComplaintsReportModel
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
                REJECTIONMEMOPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),// "/REJECTION_MEMO/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                REJECTION_REASON = Convert.ToString(row["REJECTION_REASON"]),
                NO_JI_RES = Convert.ToString(row["NO_JI_RES"]),
                JI_DATE = Convert.ToString(row["JI_DATE"]),
                COMPLAINTSCASESPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_CASES/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
                STATUS = Convert.ToString(row["STATUS"]),
                COMPLAINTSREPORTPATH = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Enums.GetEnumDescription(Enums.FolderPath.ConsigneeComplaints), Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"])),//"/COMPLAINTS_REPORT/" + Convert.ToString(row["CASE_NO"]) + "-" + Convert.ToString(row["BK_NO"]) + "-" + Convert.ToString(row["SET_NO"]),
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

            return list;
            //query = list.AsQueryable();

            //dTResult.recordsTotal = ds.Tables[0].Rows.Count;
            //dTResult.recordsFiltered = ds.Tables[0].Rows.Count;
            //if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            //dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            //dTResult.draw = dtParameters.Draw;
            //return dTResult;
        }

        public DTResult<ConsigneeComplaintsReportModel> Get_Consignee_Complaints(DTParameters dtParameters, UserSessionModel model)
        {
            throw new NotImplementedException();
        }
    }
}
