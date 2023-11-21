using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ModelContext context;

        public DashboardRepository(ModelContext context)
        {
            this.context = context;
        }

        public DashboardModel GetIEDDashBoardCount(int IeCd)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_COUNT", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                    model.DSCExpiryDateCount = 0;
                    model.NCIsuedAgainstIECount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_NO_OF_NC_ISSUE"]);
                    model.OutstandingNCCount = 0;
                    model.NotRecievedCount = Convert.ToInt32(ds.Tables[0].Rows[0]["IC_ISSUE_BUT_NOT_RECEIVE_OFFICE"]);
                    model.ConsigneeCompaintCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CONSIGNEE_COMPLAINT"]);
                }

            }

            OracleParameter[] par1 = new OracleParameter[2];

            par1[0] = new OracleParameter("P_IECD", OracleDbType.Int32, IeCd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_IE_DASHBOARD_PENDING_CALL", par1);
            List<IEList> listIE = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    listIE = dt.AsEnumerable().Select(row => new IEList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        INSP_DESIRE_DT = Convert.ToDateTime(row["INSP_DESIRE_DT"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        CONTACT_PER = Convert.ToString(row["CONTACT_PER"]),
                        CONTACT_NO = Convert.ToString(row["CONTACT_NO"])
                    }).ToList();
                }
            }
            model.lstIE = listIE;

            return model;
        }

        public DashboardModel GetVendorDashBoardCount(int Vend_Cd)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[2];

            par[0] = new OracleParameter("P_VENDCD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_COUNT", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                }
            }

            OracleParameter[] par1 = new OracleParameter[2];

            par1[0] = new OracleParameter("P_VEND_CD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_RECENR_CALL_STATUS", par1);
            List<POCallStatusList> listVend = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    listVend = dt.AsEnumerable().Select(row => new POCallStatusList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        IE_PHONE_NO = Convert.ToString(row["IE_PHONE_NO"]),
                        CO_NAME = Convert.ToString(row["CO_NAME"]),
                        CO_PHONE_NO = Convert.ToString(row["CO_PHONE_NO"])
                    }).ToList();
                }
            }
            model.lstPOCallStatus = listVend;

            OracleParameter[] par2 = new OracleParameter[2];

            par2[0] = new OracleParameter("P_VENDCD", OracleDbType.Int32, Vend_Cd, ParameterDirection.Input);
            par2[1] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds2 = DataAccessDB.GetDataSet("GET_VENDOR_DASHBOARD_RECENT_PO", par2);
            List<RecentPOList> lstRecentPO = new();
            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds2.Tables[0];
                    lstRecentPO = dt.AsEnumerable().Select(row => new RecentPOList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        CLIENT_NAME = Convert.ToString(row["CLIENT_NAME"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                        PURCHASE_ORDER = Convert.ToString(row["PURCHASE_ORDER"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"])
                    }).ToList();
                }
            }
            model.lstRecentPO = lstRecentPO;

            return model;
        }

        public DashboardModel GetClientDashBoardCount(string OrgnType, string Organisation)
        {
            DashboardModel model = new();

            OracleParameter[] par = new OracleParameter[3];

            par[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_COUNT", par);

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    model.TotalCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TOTAL_CALL"]);
                    model.PendingCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["PENDING_CALL"]);
                    model.AcceptedCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["ACCEPTED_CALL"]);
                    model.CancelledCallsCount = Convert.ToInt32(ds.Tables[0].Rows[0]["CANCELLED_CALL"]);
                    model.UnderLabTestingCount = Convert.ToInt32(ds.Tables[0].Rows[0]["UNDER_LAB_TESTING"]);
                    model.StillUnderInspectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STILL_UNDER_INSPECTION"]);
                    model.StageRejectionCount = Convert.ToInt32(ds.Tables[0].Rows[0]["STAGE_REJECTION"]);
                }
            }

            OracleParameter[] par1 = new OracleParameter[3];

            par1[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par1[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par1[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds1 = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_VENDOR_WISE_PERFORMANCE", par1);
            List<ClientVENDPOList> lstClientVEND = new();
            if (ds1 != null && ds1.Tables.Count > 0)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds1.Tables[0];
                    lstClientVEND = dt.AsEnumerable().Select(row => new ClientVENDPOList
                    {
                        RLY_CD = Convert.ToString(row["RLY_CD"]),
                        RLY_NONRLY = Convert.ToString(row["RLY_NONRLY"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        TOTAL_CALL = Convert.ToInt32(row["TOTAL_CALL"]),
                        REJECTED_CALL = Convert.ToInt32(row["REJECTED_CALL"]),
                        CANCELLED_CALL = Convert.ToInt32(row["CANCELLED_CALL"]),
                    }).ToList();
                }
            }
            model.lstClientVEND = lstClientVEND;

            OracleParameter[] par2 = new OracleParameter[3];

            par2[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par2[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par2[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds2 = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_RECENT_REQUEST", par2);
            List<ClientRecentReqList> lstClientRecentReq = new();
            if (ds2 != null && ds2.Tables.Count > 0)
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds2.Tables[0];
                    lstClientRecentReq = dt.AsEnumerable().Select(row => new ClientRecentReqList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        CALL_RECV_DT = Convert.ToDateTime(row["CALL_RECV_DT"]),
                        CALL_SNO = Convert.ToInt32(row["CALL_SNO"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        DETAILS = Convert.ToString(row["DETAILS"]),
                        IE_NAME = Convert.ToString(row["IE_NAME"]),
                        CALL_STATUS = Convert.ToString(row["CALL_STATUS"]),
                    }).ToList();
                }
            }
            model.lstClientRecentReq = lstClientRecentReq;

            OracleParameter[] par3 = new OracleParameter[3];

            par3[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par3[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par3[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds3 = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_RECENT_PO", par3);
            List<ClientRecentPOList> lstClientRecentPO = new();
            if (ds3 != null && ds3.Tables.Count > 0)
            {
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds3.Tables[0];
                    lstClientRecentPO = dt.AsEnumerable().Select(row => new ClientRecentPOList
                    {
                        CASE_NO = Convert.ToString(row["CASE_NO"]),
                        PO_DT = Convert.ToDateTime(row["PO_DT"]),
                        VALUE = Convert.ToDecimal(row["VALUE"]),
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        PO_NO = Convert.ToString(row["PO_NO"]),
                    }).ToList();
                }
            }
            model.lstClientRecentPO = lstClientRecentPO;


            OracleParameter[] par4 = new OracleParameter[3];

            par4[0] = new OracleParameter("P_RLY_CD", OracleDbType.Varchar2, Organisation, ParameterDirection.Input);
            par4[1] = new OracleParameter("P_RLY_NONRLY", OracleDbType.Varchar2, OrgnType, ParameterDirection.Input);
            par4[2] = new OracleParameter("P_RESULT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

            DataSet ds4 = DataAccessDB.GetDataSet("GET_CLIENT_DASHBOARD_VENDOR_CONSIGNEE_COMPLAINTS", par4);
            List<ClientVendConCompList> lstClientVendConComp = new();
            if (ds4 != null && ds4.Tables.Count > 0)
            {
                if (ds4.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds4.Tables[0];
                    lstClientVendConComp = dt.AsEnumerable().Select(row => new ClientVendConCompList
                    {
                        VEND_NAME = Convert.ToString(row["VEND_NAME"]),
                        NO_OF_COMPLAINTS = Convert.ToInt32(row["NO_OF_COMPLAINTS"]),
                    }).ToList();
                }
            }
            model.lstClientVendConComp = lstClientVendConComp;

            return model;
        }

        public DTResult<VenderCallRegisterModel> GetDataListTotalCallListing(DTParameters dtParameters, string Region)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", ActionType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ActionType"]))
            {
                ActionType = Convert.ToString(dtParameters.AdditionalValues["ActionType"]);
            }
            if (ActionType == "TC")
            {
                query = from l in context.ViewGetCallRegCancellations
                        where l.RegionCode == Region
                              && (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate))
                        orderby l.CaseNo, l.CallRecvDt
                        select new VenderCallRegisterModel
                        {
                            CaseNo = l.CaseNo,
                            CallRecvDt = l.CallRecvDt,
                            CallInstallNo = l.CallInstallNo,
                            CallSno = Convert.ToInt16(l.CallSno),
                            CallStatus = l.CallStatus,
                            CallLetterNo = l.CallLetterNo,
                            Remarks = l.Remarks,
                            PoNo = l.PoNo,
                            PoDt = l.PoDt,
                            IeSname = l.IeSname,
                            Vendor = l.Vendor,
                            RegionCode = l.RegionCode,
                        };
            }
            else
            {
                query = from l in context.ViewGetCallRegCancellations
                        where l.RegionCode == Region
                              && (l.CallRecvDt >= Convert.ToDateTime(FromDate) && l.CallRecvDt <= Convert.ToDateTime(ToDate))
                              && l.CStatus == ActionType
                        orderby l.CaseNo, l.CallRecvDt
                        select new VenderCallRegisterModel
                        {
                            CaseNo = l.CaseNo,
                            CallRecvDt = l.CallRecvDt,
                            CallInstallNo = l.CallInstallNo,
                            CallSno = Convert.ToInt16(l.CallSno),
                            CallStatus = l.CallStatus,
                            CallLetterNo = l.CallLetterNo,
                            Remarks = l.Remarks,
                            PoNo = l.PoNo,
                            PoDt = l.PoDt,
                            IeSname = l.IeSname,
                            Vendor = l.Vendor,
                            RegionCode = l.RegionCode,
                        };
            }



            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<VenderCallRegisterModel> GetDataCallDeskInfoListing(DTParameters dtParameters, string Region)
        {
            DTResult<VenderCallRegisterModel> dTResult = new() { draw = 0 };
            IQueryable<VenderCallRegisterModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "" || orderCriteria == null)
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "desc";
            }
            else
            {
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }

            string FromDate = "", ToDate = "", ActionType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["FromDate"]))
            {
                FromDate = Convert.ToString(dtParameters.AdditionalValues["FromDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ToDate"]))
            {
                ToDate = Convert.ToString(dtParameters.AdditionalValues["ToDate"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["ActionType"]))
            {
                ActionType = Convert.ToString(dtParameters.AdditionalValues["ActionType"]);
            }
            if (ActionType == "ACM")
            {
                query = from T17 in context.T17CallRegisters
                        join T09 in context.T09Ies on T17.IeCd equals T09.IeCd
                        join T05 in context.T05Vendors on T17.MfgCd equals T05.VendCd
                        where T17.RegionCode == Region
                        && (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        && T17.AutomaticCall == "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            CallMarkDt = T17.CallMarkDt,
                            CallStatus = T17.CallStatus,
                            IE_name = T09.IeName,
                            Vendor = T05.VendName,
                            RegionCode = T17.RegionCode,
                        };
            }
            else if (ActionType == "MCM")
            {
                query = from T17 in context.T17CallRegisters
                        join T09 in context.T09Ies on T17.IeCd equals T09.IeCd
                        join T05 in context.T05Vendors on T17.MfgCd equals T05.VendCd
                        where T17.RegionCode == Region
                        && (T17.CallRecvDt >= Convert.ToDateTime(FromDate) && T17.CallRecvDt <= Convert.ToDateTime(ToDate))
                        && T17.AutomaticCall != "Y"
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T17.CaseNo,
                            CallRecvDt = T17.CallRecvDt,
                            CallSno = Convert.ToInt16(T17.CallSno),
                            CallMarkDt = T17.CallMarkDt,
                            CallStatus = T17.CallStatus,
                            IE_name = T09.IeName,
                            Vendor = T05.VendName,
                            RegionCode = T17.RegionCode,
                        };
            }
            else if (ActionType == "POAC")
            {
                query = from T80 in context.ViewPomasterlists
                        where T80.RegionCode == Region
                        && T80.RealCaseNo == null
                        && T80.Isdeleted != Convert.ToByte(true)
                        && (T80.PoDt >= Convert.ToDateTime(FromDate) && T80.PoDt <= Convert.ToDateTime(ToDate))
                        select new VenderCallRegisterModel
                        {
                            CaseNo = T80.CaseNo,
                            PoNo = T80.PoNo,
                            PoDt = Convert.ToDateTime(T80.PoDt),
                            Rly  = T80.RlyCd,
                            Vendor = T80.VendName,
                            Consignee = T80.ConsigneeSName,
                            Remarks = T80.Remarks,
                            RegionCode = T80.RegionCode,
                        };
            }

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }
    }

}

