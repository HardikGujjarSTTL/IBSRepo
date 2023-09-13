using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories.Reports
{
    public class ManagementReportsRepository : IManagementReportsRepository
    {
        private readonly ModelContext context;

        public ManagementReportsRepository(ModelContext context)
        {
            this.context = context;
        }

        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region)
        {
            IEPerformanceModel model = new();
            List<IEPerformanceListModel> lstPerformance = new();
            List<IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[4] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_IE_PERFORMANCE_DETAILS", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstPerformance = JsonConvert.DeserializeObject<List<IEPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                model.RejectionsIssued = Convert.ToInt32(ds.Tables[1].Rows[0]["REJECTIONS_ISSUED"]);
                model.TotalICs = Convert.ToInt32(ds.Tables[1].Rows[0]["TOTAL_IC"]);
                model.CallsAttendedWithin7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_WITHIN"]);
                model.CallsAttendedBeyond7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_BEYOND"]);

            }

            model.lstPerformance = lstPerformance;

            model.lstTotalPerformance = new()
            {
                IE_NAME = "Totals",
                DEPT = "",
                CALLS = lstPerformance.Sum(x => x.CALLS),
                CALL_CANCEL = lstPerformance.Sum(x => x.CALL_CANCEL),
                C0 = lstPerformance.Sum(x => x.C0),
                C7 = lstPerformance.Sum(x => x.C7),
                CM7 = lstPerformance.Sum(x => x.CM7),
                REJECTIONS = lstPerformance.Sum(x => x.REJECTIONS),
                MATERIAL_VALUE = lstPerformance.Sum(x => x.MATERIAL_VALUE),
                AVERAGE_FEE = lstPerformance.Sum(x => x.AVERAGE_FEE),
                C3 = lstPerformance.Sum(x => x.C3),
                C10 = lstPerformance.Sum(x => x.C10),
            };

            model.lstPerformanceSummaryList = (from t20 in context.T20Ics
                                               join t13 in context.T13PoMasters on t20.CaseNo equals t13.CaseNo
                                               join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                                               where t20.CaseNo.Substring(0, 1) == Region && (t22.BillDt >= FromDate && t22.BillDt <= ToDate)
                                               select new IEPerformanceSummaryListModel
                                               {
                                                   RLY_NONRLY = t13.RlyNonrly == "R" ? "Railway Inspections" : "Non-Railway Inspections",
                                                   IC_COUNT = 1,
                                                   MATERIAL_VALUE = t22.MaterialValue ?? 0
                                               }
                                              ).GroupBy(group => group.RLY_NONRLY).Select(x => new IEPerformanceSummaryListModel { RLY_NONRLY = x.Key, IC_COUNT = x.Count(), MATERIAL_VALUE = x.Sum(x => x.MATERIAL_VALUE) }).OrderByDescending(x => x.RLY_NONRLY).ToList();

            return model;
        }

        public int DiffDays(DateTime StartDate, DateTime EndDate)
        {
            TimeSpan difference = EndDate - StartDate;
            int daysDifference = (int)difference.TotalDays;
            return daysDifference;
        }

    }

}

