using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;

namespace IBS.Repositories
{
    public class JITopsheetReportRepository : IJITopsheetReportRepository
    {
        private readonly ModelContext context;

        public JITopsheetReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public ConsigneeComplaints GetComplaintReportDetails(string JISNO, string Region)
        {
            ConsigneeComplaints model = new();
            List<ConsigneeReport> lstconsignee = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.JiSno = JISNO;

            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_JI_SNO", OracleDbType.Varchar2, JISNO.ToString() == "" ? DBNull.Value : JISNO.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

             ds = DataAccessDB.GetDataSet("GetComplaintDetailsReport", par, 1);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                List<ConsigneeReport> listcong = dt.AsEnumerable().Select(row => new ConsigneeReport
                {
                    JiSno = row.Field<string>("JI_SNO"),
                    ComplaintDate = DateTime.TryParseExact(row.Field<string>("COMPLAINT_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue)
                    ? dateValue
                    : (DateTime?)null,
                    Consignee = row.Field<string>("CONSIGNEE"),
                    PO_NO = row.Field<string>("PO"),
                    ie_name = row.Field<string>("IE_NAME"),
                    InspRegion = row.Field<string>("INSP_REGION_NAME"),
                    IC_NO = row.Field<string>("IC"),
                    BK_NO = row.Field<string>("BK_NO") + "/" + row.Field<string>("SET_NO"),
                    VEND_NAME = row.Field<string>("VENDOR"),
                    ItemDesc = row.Field<string>("ITEM_DESC"),
                    QtyOffered = (decimal?)row.Field<double>("QTY_OFFERED"),
                    QtyRejected = (decimal?)row.Field<double>("QTY_REJECTED"),
                    RejectionReason = row.Field<string>("REJECTION_REASON"),
                    Remarks = row.Field<string>("IE_JI_REMARKS"),
                    JIDate = DateTime.TryParseExact(row.Field<string>("JI_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValues)
                    ? dateValues
                    : (DateTime?)null,
                    NoJiOther = row.Field<string>("ACTION"),
                    JiStatusDesc = row.Field<string>("JI_STATUS"),
                    CoName = row.Field<string>("CO_NAME"),
                }).ToList();
                model.lstconsignee = listcong;
            }

            return model;
        }

    }
}
