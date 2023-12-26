using IBS.DataAccess;
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

        public IEPerformanceModel GetIEPerformanceData(DateTime FromDate, DateTime ToDate, string Region, int IeCd)
        {
            IEPerformanceModel model = new();
            List<IEPerformanceModel.IEPerformanceListModel> lstPerformance = new();
            List<IEPerformanceModel.IEPerformanceSummaryListModel> lstPerformanceSummaryList = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[6];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_IE_CD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            parameter[4] = new OracleParameter("p_result_cursor1", OracleDbType.RefCursor, ParameterDirection.Output);
            parameter[5] = new OracleParameter("p_result_cursor2", OracleDbType.RefCursor, ParameterDirection.Output);

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
                                               && (IeCd == 0 || (IeCd > 0 && t20.IeCd == IeCd))
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
                CALL_SNO = item.CallSno,
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

        public ConsignRejectModel GetConsignRejectData(DateTime FromDate, DateTime ToDate, string Region, string InspRegion, string Status)
        {
            ConsignRejectModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var query = from t in context.TempOnlineComplaints
                        join v in context.V05Vendors on t.VendCd equals v.VendCd
                        join c in context.T40ConsigneeComplaints on t.ComplaintId equals c.ComplaintId into cc
                        from c in cc.DefaultIfEmpty()
                        where t.TempComplaintDt >= FromDate && t.TempComplaintDt <= ToDate
                        && (!string.IsNullOrEmpty(InspRegion) ? t.InspRegion == InspRegion : true)
                        && (Status == "P" ? t.Status == null : (Status == "A" || Status == "R") ? t.Status == Status : true)
                        orderby t.TempComplaintDt
                        select new
                        {
                            t.TempComplaintId,
                            t.TempComplaintDt,
                            t.ConsigneeName,
                            t.ConsigneeDesig,
                            t.ConsigneeEmail,
                            t.ConsigneeMobile,
                            t.RejMemoNo,
                            t.RejMemoDt,
                            t.CaseNo,
                            t.BkNo,
                            t.SetNo,
                            t.InspRegion,
                            t.IeCd,
                            t.CoCd,
                            t.ConsigneeCd,
                            t.ItemDesc,
                            v.Vendor,
                            t.QtyRejected,
                            t.RejectionValue,
                            t.RejectionReason,
                            Status = t.Status == "A" ? "Accepted"
                                    : t.Status == "R" ? "Rejected" : "Pending",
                            t.TempCompRejReason,
                            t.ComplaintId,
                            JiRequired = c.JiRequired == "Y" ? "YES"
                                    : c.JiRequired == "N" ? "NO" : "NOT DECIDED",
                            c.JiSno
                        };

            model.lstConsignReject = query.AsEnumerable().Select((item, index) => new ConsignRejectModel.ConsignRejectListModel
            {
                ID = index + 1,
                TempComplaintId = item.TempComplaintId,
                TempComplaintDt = item.TempComplaintDt,
                ConsigneeName = item.ConsigneeName,
                ConsigneeDesig = item.ConsigneeDesig,
                ConsigneeEmail = item.ConsigneeEmail,
                ConsigneeMobile = item.ConsigneeMobile,
                RejMemoNo = item.RejMemoNo,
                RejMemoDt = item.RejMemoDt,
                CaseNo = item.CaseNo,
                BkNo = item.BkNo,
                SetNo = item.SetNo,
                InspRegion = item.InspRegion,
                IeCd = item.IeCd,
                CoCd = item.CoCd,
                ConsigneeCd = item.ConsigneeCd,
                ItemDesc = item.ItemDesc,
                Vendor = item.Vendor,
                QtyRejected = item.QtyRejected,
                RejectionValue = item.RejectionValue,
                RejectionReason = item.RejectionReason,
                Status = item.Status,
                TempCompRejReason = item.TempCompRejReason,
                ComplaintId = item.ComplaintId,
                JiRequired = item.JiRequired,
                JiSno = item.JiSno,
            }).ToList();

            return model;
        }

        public OutstandingOverRegionModel GetOutstandingOverRegion(DateTime FromDate)
        {
            OutstandingOverRegionModel model = new();

            model.FromDate = FromDate;

            model.lstOutstandingOverRegion = (from b in context.V22bOutstandingBills
                                              where b.AmountOutstanding > 0 && b.BillDt <= FromDate
                                              group b by new { b.BpoRegion, b.RegionCode } into g
                                              select new OutstandingOverRegionModel.OutstandingOverRegionListModel
                                              {
                                                  Count = g.Count(),
                                                  BpoRegion = g.Key.BpoRegion,
                                                  RegionCode = g.Key.RegionCode,
                                                  Total = g.Sum(b => b.AmountOutstanding) ?? 0
                                              }).ToList();

            return model;
        }

        public ClientWiseRejectionModel GetClientWiseRejection(DateTime FromDate, DateTime ToDate, string ClientType, string BPORailway)
        {
            ClientWiseRejectionModel model = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;

            model.lstClientWiseRejection = (from t13 in context.T13PoMasters
                                            join t20 in context.T20Ics on t13.CaseNo equals t20.CaseNo
                                            join t22 in context.T22Bills on t20.BillNo equals t22.BillNo
                                            join t09 in context.T09Ies on t20.IeCd equals t09.IeCd
                                            join v05 in context.V05Vendors on t13.VendCd equals v05.VendCd
                                            join t23 in context.T23BillItems on t22.BillNo equals t23.BillNo
                                            join t18 in context.T18CallDetails on new { t20.CaseNo, t20.CallRecvDt, t20.CallSno, ItemSrnoPo = t23.ItemSrno ?? 0 } equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno, t18.ItemSrnoPo }
                                            where t20.IcTypeId == 2 && t13.RlyNonrly == ClientType && t13.RlyCd.ToUpper() == BPORailway && t20.IcDt >= FromDate && t20.IcDt <= ToDate
                                            orderby v05.Vendor, t20.IcDt, t18.QtyRejected descending
                                            select new ClientWiseRejectionModel.ClientWiseRejectionListModel
                                            {
                                                BillNo = t22.BillNo,
                                                BillDt = t22.BillDt,
                                                PoNo = t13.PoNo,
                                                PoDt = t13.PoDt,
                                                RlyCd = t13.RlyCd,
                                                BkNo = t20.BkNo,
                                                SetNo = t20.SetNo,
                                                ReasonReject = t20.ReasonReject,
                                                IeName = t09.IeName,
                                                Vendor = v05.Vendor,
                                                IcDt = t20.IcDt,
                                                BillAmount = t22.BillAmount,
                                                ItemDesc = t23.ItemDesc,
                                                QtyToInsp = t18.QtyToInsp,
                                                QtyRejected = t18.QtyRejected
                                            }).ToList();

            int index = 0;
            model.lstClientWiseRejection.ToList().ForEach(i =>
            {
                index = index + 1;
                i.ID = index;
            });

            if (ClientType == "R")
            {
                model.BPORailway = context.T91Railways.Where(x => x.RlyCd.ToUpper() == BPORailway).Select(x => x.Railway).FirstOrDefault();
            }
            else
            {
                model.BPORailway = context.T12BillPayingOfficers.Where(x => x.BpoRly.ToUpper() == BPORailway).Select(x => x.BpoOrgn).FirstOrDefault();
            }

            return model;
        }

        public NonConformityModel GetNonConformityData(string FromYearMonth, string ToYearMonth, int IeCd)
        {
            NonConformityModel model = new();

            model.IEName = context.T09Ies.Where(x => x.IeCd == IeCd).Select(x => x.IeName).FirstOrDefault();

            List<NonConformityModel.NonConformityListModel> lstNonConformity = new();

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Varchar2, FromYearMonth, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Varchar2, string.IsNullOrEmpty(ToYearMonth) ? FromYearMonth : ToYearMonth, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_IE_CD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_NON_CONFORMITY_DATA", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstNonConformity = JsonConvert.DeserializeObject<List<NonConformityModel.NonConformityListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            model.lstNonConformity = lstNonConformity;

            int index = 0;
            model.lstNonConformity.ToList().ForEach(i =>
            {
                index = index + 1;
                i.ID = index;
            });

            return model;
        }

        public PendingCallsModel GetPendingCallsData()
        {
            PendingCallsModel model = new();

            List<PendingCallsModel.PendingCallsListModel> lstPendingCalls = new();

            OracleParameter[] parameter = new OracleParameter[2];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, DateTime.Now.Date, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_PENDING_CALLS_DATA", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstPendingCalls = JsonConvert.DeserializeObject<List<PendingCallsModel.PendingCallsListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            model.lstPendingCalls = lstPendingCalls;

            return model;
        }

        public ICIssuedNotReceivedModel GetICIssuedNotReceived(DateTime FromDate, DateTime ToDate, string Region)
        {
            ICIssuedNotReceivedModel model = new();
            List<ICIssuedNotReceivedModel.ICIssuedNotReceivedListModel> lstICIssuedNotReceived = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[4];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_IC_ISSUED_NOT_RECEIVED_DATA", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstICIssuedNotReceived = JsonConvert.DeserializeObject<List<ICIssuedNotReceivedModel.ICIssuedNotReceivedListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            model.lstICIssuedNotReceived = lstICIssuedNotReceived;

            return model;
        }

        public TentativeInspectionFeeWisePendingCallsModel GetTentativeInspectionFeeWisePendingCalls(DateTime FromDate, DateTime ToDate, string Region, string ParticularCM, string SortedOn)
        {
            TentativeInspectionFeeWisePendingCallsModel model = new();
            List<TentativeInspectionFeeWisePendingCallsModel.TentativeInspectionFeeWisePendingCallsListModel> lstTentativeInspectionFeeWisePendingCalls = new();

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            OracleParameter[] parameter = new OracleParameter[5];
            parameter[0] = new OracleParameter("p_FROM_DT", OracleDbType.Date, FromDate, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_TO_DT", OracleDbType.Date, ToDate, ParameterDirection.Input);
            parameter[2] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[3] = new OracleParameter("p_CO_CD", OracleDbType.Int32, string.IsNullOrEmpty(ParticularCM) ? 0 : Convert.ToInt32(ParticularCM), ParameterDirection.Input);
            parameter[4] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_INSPECTION_FEE_WISE_PENDING_CALLS_DATA", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstTentativeInspectionFeeWisePendingCalls = JsonConvert.DeserializeObject<List<TentativeInspectionFeeWisePendingCallsModel.TentativeInspectionFeeWisePendingCallsListModel>>(serializeddt, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            if (SortedOn == "InspFee")
            {
                lstTentativeInspectionFeeWisePendingCalls = lstTentativeInspectionFeeWisePendingCalls.OrderByDescending(x => x.INSP_FEE).ToList();
            }
            else if (SortedOn == "DeliveryPeriod")
            {
                lstTentativeInspectionFeeWisePendingCalls = lstTentativeInspectionFeeWisePendingCalls.OrderBy(x => x.EXT_DELV_DATE).ToList();
            }
            else if (SortedOn == "DesireDate")
            {
                lstTentativeInspectionFeeWisePendingCalls = lstTentativeInspectionFeeWisePendingCalls.OrderBy(x => x.INSP_DESIRE_DATE).ToList();
            }
            else if (SortedOn == "PendingSince")
            {
                lstTentativeInspectionFeeWisePendingCalls = lstTentativeInspectionFeeWisePendingCalls.OrderByDescending(x => x.PENDING_SINCE).ToList();
            }

            int index = 0;
            lstTentativeInspectionFeeWisePendingCalls.ToList().ForEach(i =>
            {
                index = index + 1;
                i.ID = index;
            });

            model.lstTentativeInspectionFeeWisePendingCalls = lstTentativeInspectionFeeWisePendingCalls;

            return model;
        }

        public CallRemarkingModel GetCallRemarkingData(DateTime FromDate, DateTime ToDate, string Region, string CallRemarkingDate, string CallsStatus)
        {
            CallRemarkingModel model = new();

            FromDate = FromDate.Date.Add(new TimeSpan(0, 0, 0));
            ToDate = ToDate.Date.Add(new TimeSpan(23, 59, 59));

            model.FromDate = FromDate;
            model.ToDate = ToDate;
            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            var query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                        join t02Initiator in context.T02Users on t108.RemInitBy equals t02Initiator.UserId
                        join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (int)t108.CallSno } equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        join t12Approver in context.T02Users on t108.RemAppBy equals t12Approver.UserId into t12Group
                        from t12 in t12Group.DefaultIfEmpty()
                        where (!string.IsNullOrEmpty(CallsStatus) ? t108.RemarkingStatus == CallsStatus : true)
                            && (CallRemarkingDate == "InitiatedDate" ? (t108.RemInitDatetime >= FromDate && t108.RemInitDatetime <= ToDate) : (t108.RemAppDatetime >= FromDate && t108.RemAppDatetime <= ToDate))
                             && t108.CaseNo.StartsWith(Region)
                        orderby CallRemarkingDate == "InitiatedDate" ? t108.RemInitDatetime : t108.RemAppDatetime descending
                        select new
                        {
                            t108.CaseNo,
                            t108.CallRecvDt,
                            t108.CallSno,
                            RemarkingStatus = (t108.RemarkingStatus == "P") ? "Pending" : (t108.RemarkingStatus == "R") ? "Rejected" : (t108.RemarkingStatus == "A") ? "Approved" : "",
                            t108.RemarkReason,
                            IE_Name_From = t09From.IeName,
                            IE_Name_To = t10To.IeName,
                            t108.FrIePendingCalls,
                            t108.ToIePendingCalls,
                            User_Name = t02Initiator.UserName,
                            t108.RemInitDatetime,
                            User_Name_App = t12.UserName,
                            t108.RemAppDatetime,
                            CallRemarkStatus = t17.CallRemarkStatus ?? "0"
                        };

            var result = query.ToList();

            model.lstCallRemarking = query.AsEnumerable().Select((item, index) => new CallRemarkingModel.CallRemarkingListModel
            {
                ID = index + 1,
                CaseNo = item.CaseNo,
                CallRecvDt = item.CallRecvDt,
                CallSno = item.CallSno,
                RemarkingStatus = item.RemarkingStatus,
                RemarkReason = item.RemarkReason,
                IE_Name_From = item.IE_Name_From,
                IE_Name_To = item.IE_Name_To,
                FrIePendingCalls = item.FrIePendingCalls,
                ToIePendingCalls = item.ToIePendingCalls,
                User_Name = item.User_Name,
                RemInitDatetime = item.RemInitDatetime,
                User_Name_App = item.User_Name_App,
                RemAppDatetime = item.RemAppDatetime,
                CallRemarkStatus = item.CallRemarkStatus,
            }).ToList();

            return model;
        }

        public CallDetailsDashboradModel GetCallDetailsDashborad(string Region)
        {
            CallDetailsDashboradModel model = new();

            model.Region = EnumUtility<Enums.Region>.GetDescriptionByKey(Region);

            List<CallDetailsDashboradModel.CallDetailsDashboradListModel> lstCallDetailsDashborad = new();

            OracleParameter[] parameter = new OracleParameter[2];

            parameter[0] = new OracleParameter("p_REGION", OracleDbType.Varchar2, Region, ParameterDirection.Input);
            parameter[1] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("SP_GET_CALLS_DETAILS_DATA", parameter);

            if (ds != null && ds.Tables.Count > 0)
            {
                string serializeddt1 = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                lstCallDetailsDashborad = JsonConvert.DeserializeObject<List<CallDetailsDashboradModel.CallDetailsDashboradListModel>>(serializeddt1, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            model.lstCallDetailsDashborad = lstCallDetailsDashborad;

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

