using IBS.Interfaces.Transaction;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using static IBS.Helper.Enums;
using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models.Reports;

namespace IBS.Repositories.Reports
{
    public class HighValueInspecReportRepository : IHighValueInspecReportRepository
    {
        private readonly ModelContext context;

        public HighValueInspecReportRepository(ModelContext context)
        {
            this.context = context;
        }

        public HighValueInspReport GetHighValueInspdata(string month, string year, string valinsp, string FromDate, string ToDate, string ICDate, string BillDate, string formonth, string forperiod, string Region)
        {
            HighValueInspReport model = new();
            List<ValueInspList> lstValueInspList = new();

            DataSet ds = null;
            DataTable dt = new DataTable();

            model.month = month; model.year = year; model.valinsp = valinsp; model.FromDate = FromDate; model.ToDate = ToDate; model.ICDate = ICDate; model.BillDate = BillDate; model.formonth = formonth; model.forperiod = forperiod;

            OracleParameter[] par = new OracleParameter[7];
            par[0] = new OracleParameter("p_region", OracleDbType.Varchar2,Region , ParameterDirection.Input);
            par[1] = new OracleParameter("p_rdoBill", OracleDbType.Varchar2,BillDate, ParameterDirection.Input);
            par[2] = new OracleParameter("p_rdoDt", OracleDbType.Varchar2,forperiod , ParameterDirection.Input);
            par[3] = new OracleParameter("p_fromdt", OracleDbType.Varchar2,FromDate , ParameterDirection.Input);
            par[4] = new OracleParameter("p_todate", OracleDbType.Varchar2,ToDate , ParameterDirection.Input);
            par[5] = new OracleParameter("p_monthyear", OracleDbType.Varchar2,year+month , ParameterDirection.Input);
            par[6] = new OracleParameter("p_result", OracleDbType.RefCursor, ParameterDirection.Output);

            ds = DataAccessDB.GetDataSet("GetNHighValueInspReport", par, 1);

            int recCount = 0;
            List<ValueInspList> listcong = new List<ValueInspList>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (recCount < Convert.ToInt32(valinsp))
                {
                    dt = ds.Tables[0];
                    ValueInspList valueInsp = new ValueInspList
                    {
                        // Populate the properties based on the row data
                        BillNo = row.Field<string>("BILL_NO"),
                        BillDate = DateTime.TryParseExact(row.Field<string>("BILL_DT"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue)
                            ? dateValue
                            : (DateTime?)null,
                        CaseNo = row.Field<string>("CASE_NO"),
                        EngName = row.Field<string>("IE_NAME"),
                        VENDOR = row.Field<string>("VENDOR"),
                        CONSIGNEE = row.Field<string>("CONSIGNEE"),
                        ITEMDESC = row.Field<string>("ITEM_DESC"),
                        MATERIALVALUE = row.Field<decimal>("MATERIAL_VALUE").ToString(), // Convert to string if needed
                        PLNO = row.Field<string>("PL_NO"),
                        INSPFEE = row.Field<decimal>("INSP_FEE").ToString(), // Convert to string if needed
                        ICDate = DateTime.TryParseExact(row.Field<string>("IC_DT"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValues)
                        ? dateValues
                        : (DateTime?)null
                    };
                    listcong.Add(valueInsp);
                    recCount++;
                }
                else
                {
                    break; 
                }
            }

            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    dt = ds.Tables[0];
            //    List<ValueInspList> listcong = dt.AsEnumerable().Select(row => new ValueInspList
            //    {
            //        BillNo = row.Field<string>("BillNo"),
            //        BillDate = DateTime.TryParseExact(row.Field<string>("COMPLAINT_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValue)
            //        ? dateValue
            //        : (DateTime?)null,
            //        CaseNo = row.Field<string>("CONSIGNEE"),
            //        EngName = row.Field<string>("PO"),
            //        VENDOR = row.Field<string>("IE_NAME"),
            //        CONSIGNEE = row.Field<string>("INSP_REGION_NAME"),
            //        ITEMDESC = row.Field<string>("IC"),
            //        MATERIALVALUE = row.Field<string>("BK_NO") + "/" + row.Field<string>("SET_NO"),
            //        INSPFEE = row.Field<string>("VENDOR"),
            //        ICDate = DateTime.TryParseExact(row.Field<string>("JI_DATE"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateValues)
            //        ? dateValues
            //        : (DateTime?)null,
            //    }).ToList();
            //    model.lstValueInspList = listcong;
            //}
            model.lstValueInspList = listcong;
            return model;
        }
    }
}
