﻿using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces.Reports;
using IBS.Models.Reports;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

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
            List<IEPerformanceModel.IEPerformanceListModel> lstPerformance = new();
            List<IEPerformanceModel.IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();

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
                lstPerformance = JsonConvert.DeserializeObject<List<IEPerformanceModel.IEPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstPerformance.ToList().ForEach(i =>
                {
                    i.C3 = decimal.Truncate(i.C3);
                    i.C7 = decimal.Truncate(i.C7);
                    i.CM7 = decimal.Truncate(i.CM7);
                    i.C10 = decimal.Truncate(i.C10);
                    i.C0 = decimal.Truncate(i.C0);
                    i.CALLS = decimal.Truncate(i.CALLS);
                    i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL);
                    i.REJECTIONS = decimal.Truncate(i.REJECTIONS);
                });

                model.RejectionsIssued = Convert.ToInt32(ds.Tables[1].Rows[0]["REJECTIONS_ISSUED"]);
                model.TotalICs = Convert.ToInt32(ds.Tables[1].Rows[0]["TOTAL_IC"]);
                model.CallsAttendedWithin7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_WITHIN"]);
                model.CallsAttendedBeyond7Days = Convert.ToInt32(ds.Tables[1].Rows[0]["CALLS_ATTENDED_BEYOND"]);
            }

            model.lstPerformance = lstPerformance;

            model.lstPerformanceSummaryList = (from t20 in context.T20Ics
                                               join t13 in context.T13PoMasters on t20.CaseNo equals t13.CaseNo
                                               join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                                               where t20.CaseNo.Substring(0, 1) == Region && (t22.BillDt >= FromDate && t22.BillDt <= ToDate)
                                               select new IEPerformanceModel.IEPerformanceSummaryListModel
                                               {
                                                   RLY_NONRLY = t13.RlyNonrly == "R" ? "Railway Inspections" : "Non-Railway Inspections",
                                                   IC_COUNT = 1,
                                                   MATERIAL_VALUE = t22.MaterialValue ?? 0
                                               }
                                              ).GroupBy(group => group.RLY_NONRLY).Select(x => new IEPerformanceModel.IEPerformanceSummaryListModel { RLY_NONRLY = x.Key, IC_COUNT = x.Count(), MATERIAL_VALUE = x.Sum(x => x.MATERIAL_VALUE) }).OrderByDescending(x => x.RLY_NONRLY).ToList();

            return model;
        }

        public ClusterPerformanceModel GetClusterPerformanceData(DateTime FromDate, DateTime ToDate, string Region)
        {
            ClusterPerformanceModel model = new();
            List<ClusterPerformanceModel.ClusterPerformanceListModel> lstPerformance = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CLUSTER_PERFORMANCE_DETAILS", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstPerformance = JsonConvert.DeserializeObject<List<ClusterPerformanceModel.ClusterPerformanceListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstPerformance.ToList().ForEach(i =>
                {
                    i.C3 = decimal.Truncate(i.C3);
                    i.C7 = decimal.Truncate(i.C7);
                    i.CM7 = decimal.Truncate(i.CM7);
                    i.C10 = decimal.Truncate(i.C10);
                    i.C0 = decimal.Truncate(i.C0);
                    i.CALLS = decimal.Truncate(i.CALLS);
                    i.CALL_CANCEL = decimal.Truncate(i.CALL_CANCEL);
                    i.REJECTIONS = decimal.Truncate(i.REJECTIONS);
                });
            }

            model.lstPerformance = lstPerformance;

            return model;
        }

        public RWBSummaryModel GetRWBSummaryData(string FromYearMonth, string ToYearMonth)
        {
            RWBSummaryModel model = new();
            List<RWBSummaryModel.RWBSummaryListModel> lstRWBSummaryList = new();
            List<RWBSummaryModel.RBWSectorListModel> lstRBWSectorList = new();

            if (!string.IsNullOrEmpty(ToYearMonth))
            {
                model.FilterTitle = "The Period " + GetFilterTitle(FromYearMonth) + " to " + GetFilterTitle(ToYearMonth);
            }
            else
            {
                model.FilterTitle = "The Month of " + GetFilterTitle(FromYearMonth);
            }

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Varchar2, FromYearMonth, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Varchar2, string.IsNullOrEmpty(ToYearMonth) ? FromYearMonth : ToYearMonth, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[3] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REGION_WISE_BILLING_SUMMARY", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstRWBSummaryList = JsonConvert.DeserializeObject<List<RWBSummaryModel.RWBSummaryListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                string serializeddt2 = JsonConvert.SerializeObject(ds.Tables[1], Formatting.Indented);
                lstRBWSectorList = JsonConvert.DeserializeObject<List<RWBSummaryModel.RBWSectorListModel>>(serializeddt2, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                lstRWBSummaryList.ToList().ForEach(i =>
                {
                    i.NR_FEE = decimal.Truncate(i.NR_FEE);
                    i.NR_TAX = decimal.Truncate(i.NR_TAX);
                    i.NR_BILL_AMT = decimal.Truncate(i.NR_BILL_AMT);
                    i.NR_BILLLS = decimal.Truncate(i.NR_BILLLS);
                    i.WR_FEE = decimal.Truncate(i.WR_FEE);
                    i.WR_TAX = decimal.Truncate(i.WR_TAX);
                    i.WR_BILL_AMT = decimal.Truncate(i.WR_BILL_AMT);
                    i.WR_BILLLS = decimal.Truncate(i.WR_BILLLS);
                    i.ER_FEE = decimal.Truncate(i.ER_FEE);
                    i.ER_TAX = decimal.Truncate(i.ER_TAX);
                    i.ER_BILL_AMT = decimal.Truncate(i.ER_BILL_AMT);
                    i.ER_BILLLS = decimal.Truncate(i.ER_BILLLS);
                    i.SR_FEE = decimal.Truncate(i.SR_FEE);
                    i.SR_TAX = decimal.Truncate(i.SR_TAX);
                    i.SR_BILL_AMT = decimal.Truncate(i.SR_BILL_AMT);
                    i.SR_BILLLS = decimal.Truncate(i.SR_BILLLS);
                    i.CR_FEE = decimal.Truncate(i.CR_FEE);
                    i.CR_TAX = decimal.Truncate(i.CR_TAX);
                    i.CR_BILL_AMT = decimal.Truncate(i.CR_BILL_AMT);
                    i.CR_BILLLS = decimal.Truncate(i.CR_BILLLS);
                });

                lstRBWSectorList.ToList().ForEach(i =>
                {
                    i.INSP_FEE = decimal.Truncate(i.INSP_FEE);
                    i.BILL_AMOUNT = decimal.Truncate(i.BILL_AMOUNT);
                    i.NO_OF_BILLLS = decimal.Truncate(i.NO_OF_BILLLS);
                });
            }

            model.lstRWBSummaryList = lstRWBSummaryList;
            model.lstRBWSectorList = lstRBWSectorList;

            return model;
        }

        public RWCOModel GetRWCOData(DateTime FromDate, string Outstanding)
        {
            RWCOModel model = new();

            model.FromDate = FromDate;
            model.Outstanding = Outstanding;

            List<RWCOModel.RWCOListModel> lstRWCOList = new();

            OracleParameter[] parameter = new OracleParameter[2];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_REGION_WISE_COMPARISON_OUTSTANDING_RPT", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstRWCOList = JsonConvert.DeserializeObject<List<RWCOModel.RWCOListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (model.Outstanding == "2") lstRWCOList = lstRWCOList.Where(x => x.TOT_ALL_OUTSTANDING > 0 || x.TOT_ALL_SUSPENSE > 0).ToList();

            model.lsttRWCOList = lstRWCOList;

            return model;
        }

        public ICSubmissionModel GetICSubmissionData(DateTime FromDate, DateTime ToDate, string Region)
        {
            ICSubmissionModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var query = from t20 in context.T20Ics
                        join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
                        where t20.IcSubmitDt >= FromDate && t20.IcSubmitDt <= ToDate &&
                              t20.CaseNo.Substring(0, 1) == Region
                        orderby t20.IcSubmitDt, t09.IeCd, t20.BkNo, t20.SetNo
                        select new
                        {
                            ID = 0,
                            t20.IcSubmitDt,
                            t09.IeName,
                            t20.BkNo,
                            t20.SetNo
                        };

            model.lstICSubmission = query.AsEnumerable().Select((item, index) => new ICSubmissionModel.ICSubmissionListModel
            {
                ID = index + 1,
                IC_SUBMIT_DATE = item.IcSubmitDt,
                IE_NAME = item.IeName,
                BK_NO = item.BkNo,
                SET_NO = item.SetNo
            }).ToList();


            return model;
        }

        public PendingICAgainstCallsModel GetPendingICAgainstCallsData(DateTime FromDate, DateTime ToDate, string Region)
        {
            PendingICAgainstCallsModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var validCallStatus = new List<string> { "A", "R", "M" };

            var query = from t17 in context.T17CallRegisters
                        join t09 in context.T09Ies on t17.IeCd equals t09.IeCd
                        join t20 in context.T20Ics on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } equals new { t20.CaseNo, t20.CallRecvDt, t20.CallSno } into t20Group
                        from t20 in t20Group.DefaultIfEmpty()
                        where t20 == null &&
                              t17.CallStatus != null && validCallStatus.Contains(t17.CallStatus) &&
                              t17.CallRecvDt >= FromDate && t17.CallRecvDt <= ToDate &&
                              t17.CaseNo.Substring(0, 1) == Region
                        orderby t09.IeName, t17.CallStatus, t17.CallRecvDt, t17.CaseNo
                        select new
                        {
                            t17.CaseNo,
                            t17.CallRecvDt,
                            t17.CallSno,
                            STATUS = t17.CallStatus == "A" ? "Accepted" :
                                     t17.CallStatus == "R" ? "Rejected" :
                                     t17.CallStatus == "M" ? "Pending" :
                                     t17.CallStatus == "B" ? "Accepted and Billed" : "",
                            t09.IeName,
                            IE_STATUS = t09.IeStatus == "R" ? "Retired" :
                                        t09.IeStatus == "T" ? "Transferred" :
                                        t09.IeStatus == "L" ? "Left/Repatriated" :
                                        "Working"
                        };

            var result = query.ToList();

            model.lstPendingICAgainstCalls = query.AsEnumerable().Select((item, index) => new PendingICAgainstCallsModel.PendingICAgainstCallsListModel
            {
                ID = index + 1,
                CASE_NO = item.CaseNo,
                CALL_RECV_DT = item.CallRecvDt,
                CALL_SNO = (int)item.CallSno,
                STATUS = item.STATUS,
                IE_NAME = item.IeName,
                IE_STATUS = item.IE_STATUS
            }).ToList();


            return model;
        }

        public SuperSurpriseDetailsModel GetSuperSurpriseDetailsData(DateTime FromDate, DateTime ToDate, string Region, string ParticularCM, string ParticularSector)
        {
            SuperSurpriseDetailsModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var query = from t44 in context.T44SuperSurprises
                        join t13 in context.T13PoMasters on t44.CaseNo equals t13.CaseNo
                        join v05 in context.V05Vendors on t13.VendCd equals v05.VendCd
                        join t08 in context.T08IeControllOfficers on t44.CoCd equals t08.CoCd
                        join t09 in context.T09Ies on t44.IeCd equals t09.IeCd
                        where t44.SuperSurpriseDt >= FromDate && t44.SuperSurpriseDt <= ToDate && t44.SuperSurpriseNo.Substring(0, 1) == Region
                        && (!string.IsNullOrEmpty(ParticularCM) ? t44.CoCd == Convert.ToInt32(ParticularCM) : true)
                        && (!string.IsNullOrEmpty(ParticularSector) ? t44.NameScopeItem == ParticularSector : true)
                        orderby t08.CoName, t44.SuperSurpriseDt, t44.SuperSurpriseNo
                        select new
                        {
                            t44.SuperSurpriseNo,
                            t44.SuperSurpriseDt,
                            t08.CoName,
                            t09.IeName,
                            v05.Vendor,
                            t44.ItemDesc,
                            NameScopeItem = EnumUtility<Enums.ScopeOfsector>.GetDescriptionByKey(t44.NameScopeItem),
                            t44.PreIntRej,
                            t44.Discrepancy,
                            t44.Outcome,
                            t44.SbuHeadRemarks
                        };

            var result = query.ToList();


            model.lstSuperSurprise = query.AsEnumerable().Select((item, index) => new SuperSurpriseDetailsModel.SuperSurpriseListModel
            {
                ID = index + 1,
                SuperSurpriseNo = item.SuperSurpriseNo,
                SuperSurpriseDt = item.SuperSurpriseDt,
                CoName = item.CoName,
                IeName = item.IeName,
                Vendor = item.Vendor,
                ItemDesc = item.ItemDesc,
                NameScopeItem = item.NameScopeItem,
                PreIntRej = item.PreIntRej,
                Discrepancy = item.Discrepancy,
                Outcome = item.Outcome,
                SbuHeadRemarks = item.SbuHeadRemarks,
            }).ToList();

            return model;
        }

        public SuperSurpriseSummaryModel GetSuperSurpriseSummaryData(DateTime FromDate, DateTime ToDate, string Region)
        {
            SuperSurpriseSummaryModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var query = from t44 in context.T44SuperSurprises
                        join t09 in context.T09Ies on t44.IeCd equals t09.IeCd
                        join t08 in context.T08IeControllOfficers on t44.CoCd equals t08.CoCd
                        where t44.SuperSurpriseDt >= FromDate && t44.SuperSurpriseDt <= ToDate &&
                              t44.CaseNo.Substring(0, 1) == Region
                        group t44 by new
                        {
                            t44.CoCd,
                            t08.CoName,
                            t44.IeCd,
                            t09.IeName
                        } into grouped
                        select new
                        {
                            CO_CD = grouped.Key.CoCd,
                            CO_NAME = grouped.Key.CoName,
                            IE_CD = grouped.Key.IeCd,
                            IE_NAME = grouped.Key.IeName,
                            SUP_SUR_NO = grouped.Count()
                        }
                        into resultGrouped
                        orderby resultGrouped.CO_NAME, resultGrouped.IE_NAME
                        select resultGrouped;

            model.lstSuperSurpriseSummary = query.AsEnumerable().Select(x => new SuperSurpriseSummaryModel.SuperSurpriseSummaryListModel
            {
                CO_CD = x.CO_CD,
                CO_NAME = x.CO_NAME,
                IE_CD = x.IE_CD,
                IE_NAME = x.IE_NAME,
                SUP_SUR_NO = x.SUP_SUR_NO,

            }).ToList();

            return model;
        }

        public string GetFilterTitle(string YearMonth)
        {
            string filterTitle = string.Empty;
            if (!string.IsNullOrEmpty(YearMonth))
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(YearMonth.Substring(4, 2)));
                filterTitle = monthName + "-" + YearMonth.Substring(0, 4);
            }
            return filterTitle;
        }

    }
}

