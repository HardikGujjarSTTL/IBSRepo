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

        public DTResult<ConsigneeComplaints> GetComplaintReportDetails(DTParameters dtParameters, string Region)
        {

            DTResult<ConsigneeComplaints> dTResult = new() { draw = 0 };
            IQueryable<ConsigneeComplaints>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "JI_SNO";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                orderCriteria = "JI_SNO";
                orderAscendingDirection = true;
            }

            string JI_SNO = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["JISNO"]))
            {
                JI_SNO = Convert.ToString(dtParameters.AdditionalValues["JISNO"]);
            }
            
            OracleParameter[] par = new OracleParameter[2];
            par[0] = new OracleParameter("p_JI_SNO", OracleDbType.Varchar2, JI_SNO.ToString() == "" ? DBNull.Value : JI_SNO.ToString(), ParameterDirection.Input);
            par[1] = new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            var ds = DataAccessDB.GetDataSet("GetComplaintDetailsReport", par, 1);

            DataTable dt = ds.Tables[0];

            List<ConsigneeComplaints> list = dt.AsEnumerable().Select(row => new ConsigneeComplaints
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

            query = list.AsQueryable();
            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.JiSno).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }
}
