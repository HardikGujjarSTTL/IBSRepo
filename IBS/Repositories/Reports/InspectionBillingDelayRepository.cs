using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Interfaces.Reports;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;
using System.Linq;

namespace IBS.Repositories.Reports
{
    public class InspectionBillingDelayRepository : IInspectionBillingDelayRepository
    {
        private readonly ModelContext context;
        private readonly IConfiguration configuration;
        public InspectionBillingDelayRepository(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public DTResult<InspectionBillingDelayModel> Get_Inspection_Billing_Delay(DTParameters dtParameters, UserSessionModel model)
        {
            DTResult<InspectionBillingDelayModel> dTResult = new() { draw = 0 };
            IQueryable<InspectionBillingDelayModel> query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            string wYrMth = "";

            bool BillDt = false, IcDt = false, FInspDt = false, LFnspDt = false;
            int MonthWise = 0, DateWise = 0;
            string Month = "", Year = "", FromDate = "", ToDate = "", IECD = "0";
            MonthWise = Convert.ToBoolean(dtParameters.AdditionalValues["MonthWise"]) == true ? 1 : 0;
            DateWise = Convert.ToBoolean(dtParameters.AdditionalValues["DateWise"]) == true ? 1 : 0;
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Month"]))
            {
                Month = Convert.ToString(dtParameters.AdditionalValues["Month"]);
                Month = (Month.Length == 1) ? "0" + Month : Month;
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["Year"]))
            {
                Year = Convert.ToString(dtParameters.AdditionalValues["Year"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }

            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["IECD"]))
            {
                IECD = Convert.ToString(dtParameters.AdditionalValues["IECD"]);
            }

            IcDt = Convert.ToBoolean(dtParameters.AdditionalValues["IcDt"]);
            FInspDt = Convert.ToBoolean(dtParameters.AdditionalValues["FInspDt"]);
            LFnspDt = Convert.ToBoolean(dtParameters.AdditionalValues["LFnspDt"]);
            BillDt = Convert.ToBoolean(dtParameters.AdditionalValues["BillDate"]);

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    if (IcDt) { orderCriteria = "Ic_Dt"; }
                    else if (FInspDt) { orderCriteria = "First_Insp_Dt"; }
                    else if (LFnspDt) { orderCriteria = "Last_Insp_Dt"; }
                    else { orderCriteria = "Bill_Dt"; }
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                if (IcDt) { orderCriteria = "Ic_Dt"; }
                else if (FInspDt) { orderCriteria = "First_Insp_Dt"; }
                else if (LFnspDt) { orderCriteria = "Last_Insp_Dt"; }
                else { orderCriteria = "Bill_Dt"; }
                orderAscendingDirection = true;
            }

            if (model.UserName != "" && model.RoleId != 2)
            {
                IECD = "0";
            }
            else if (model.IeCd > 0)
            {
                IECD = model.IeCd.ToString();
            }
            //IECD = "885"; //model.Region

            string MonthYear = Year + Month;
            OracleParameter[] par = new OracleParameter[8];
            par[0] = new OracleParameter("P_MONTHWISE", OracleDbType.Int16, MonthWise, ParameterDirection.Input);
            par[1] = new OracleParameter("P_DATEWISE", OracleDbType.Int16, DateWise, ParameterDirection.Input);
            par[2] = new OracleParameter("P_MONTHYEAR", OracleDbType.Varchar2, MonthYear, ParameterDirection.Input);
            par[3] = new OracleParameter("P_FROMDATE", OracleDbType.Varchar2, FromDate, ParameterDirection.Input);
            par[4] = new OracleParameter("P_TODATE", OracleDbType.Varchar2, ToDate, ParameterDirection.Input);
            par[5] = new OracleParameter("P_IECD", OracleDbType.Varchar2, IECD, ParameterDirection.Input);
            par[6] = new OracleParameter("P_REGION", OracleDbType.Varchar2, model.Region, ParameterDirection.Input);
            par[7] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
            var ds = DataAccessDB.GetDataSet("SP_GET_INSPECTION_BILLING_DELAY", par, 1);
            DataTable dt = ds.Tables[0];
            List<InspectionBillingDelayModel> list = dt.AsEnumerable().Select(row => new InspectionBillingDelayModel
            {
                Ic_No = Convert.ToString(row["Ic_No"]),
                Ic_Dt = Convert.ToString(row["IC_DT"]),
                IC_SUBMIT_DATE = Convert.ToString(row["IC_SUBMIT_DATE"]),
                Bk_No = Convert.ToString(row["BK_NO"]),
                Set_No = Convert.ToString(row["SET_NO"]),
                Call_Dt = Convert.ToDateTime(row["CALL_DT"]),
                First_Insp_Dt = Convert.ToDateTime(row["FIRST_INSP_DT"]),
                Last_Insp_Dt = Convert.ToDateTime(row["LAST_INSP_DT"]),
                Delay_Insp = Convert.ToDouble(row["DELAY_INSP"]),
                Delay_Ic = Convert.ToDouble(row["DELAY_IC"]),
                Delay_Subm = Convert.ToDouble(row["DELAY_SUBM"]),
                Delay_Bill = Convert.ToDouble(row["DELAY_BILL"]),
                Bill_No = Convert.ToString(row["BILL_NO"]),
                Bill_Dt = Convert.ToDateTime(row["BILL_DT"]),
                Ie_Name = Convert.ToString(row["IE_NAME"]),
                Vendor = Convert.ToString(row["VENDOR"]),
                Vendor_City = Convert.ToString(row["VENDOR_CITY"]),
                Bill_Amount = Convert.ToDecimal(row["BILL_AMOUNT"]),
            }).ToList();

            query = list.AsQueryable();
            if (IcDt) { query = query.OrderBy(x => x.Ic_Dt).Select(x => x); }
            else if (FInspDt) { query = query.OrderBy(x => x.First_Insp_Dt).Select(x => x); }
            else if (LFnspDt) { query = query.OrderBy(x => x.Last_Insp_Dt).Select(x => x); }
            else { query = query.OrderBy(x => x.Bill_Dt).Select(x => x); }
            dTResult.recordsTotal = query.ToList().Count();
            dTResult.recordsFiltered = query.ToList().Count();
            if (dtParameters.Length == -1) dtParameters.Length = query.Count();
            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();
            dTResult.draw = dtParameters.Draw;
            return dTResult;
        }
    }
}
