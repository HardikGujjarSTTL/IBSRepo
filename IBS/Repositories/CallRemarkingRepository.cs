using IBS.DataAccess;
using IBS.Helper;
using IBS.Interfaces;
using IBS.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBS.Repositories
{
    public class CallRemarkingRepository : ICallRemarkingRepository
    {
        private readonly ModelContext context;

        public CallRemarkingRepository(ModelContext context)
        {
            this.context = context;
        }

        public int GetPendingCallsFromIE(string Region, int IeCd)
        {
            return context.T17CallRegisters.Where(x => x.CallStatus == "M" && x.CaseNo.Substring(0, 1) == Region && x.IeCd == IeCd).Count();
        }

        public DTResult<PendingCallsListModel> GetPendingCallsList1(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int IeCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]) ? int.Parse(dtParameters.AdditionalValues["IeCd"]) : 0;

            query = from t17 in context.T17CallRegisters
                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                    join t108 in context.T108RemarkedCalls on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                           equals new { CaseNo = Convert.ToString(t108.CaseNo), CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (short)t108.CallSno } into t108Group
                    from t108 in t108Group.Where(x => x.RemarkingStatus == "P").DefaultIfEmpty()
                    where t108.CaseNo == null && t108.CallRecvDt == null && t108.CallSno == null
                    && t17.CaseNo.Substring(0, 1) == Region
                    && t17.IeCd == IeCd
                    && t17.CallStatus == "M"
                    select new PendingCallsListModel
                    {
                        CaseNo = t17.CaseNo,
                        CallRecvDt = t17.CallRecvDt,
                        CallSno = t17.CallSno,
                        CallStatus = t17.CallStatus == "M" ? "Pending" : null,
                        MfgCd = t17.MfgCd,
                        MfgPlace = t17.MfgPlace,
                        Mfg_CityCd = t03.CityCd,
                        Mfg_City = t03.City,
                        CoCd = t17.CoCd ?? 0,
                        VendName = t05.VendName,
                        DtInspDesire = t17.DtInspDesire,
                        CallRemarkStatus = t17.CallRemarkStatus ?? "0"
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public DTResult<PendingCallsListModel> GetPendingCallsList2(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int IeCd = !string.IsNullOrEmpty(dtParameters.AdditionalValues["IeCd"]) ? int.Parse(dtParameters.AdditionalValues["IeCd"]) : 0;

            query = from t108 in context.T108RemarkedCalls
                    join t09From in context.T09Ies on (int)t108.FrIeCd equals t09From.IeCd
                    join t10To in context.T09Ies on (int)t108.ToIeCd equals t10To.IeCd
                    join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                    where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region && t108.FrIeCd == IeCd
                    select new PendingCallsListModel
                    {
                        CaseNo = t108.CaseNo,
                        CallRecvDt = t108.CallRecvDt,
                        CallSno = t108.CallSno,
                        CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                        FrIeName = t09From.IeName,
                        ToIeName = t10To.IeName,
                        UserName = t02.UserName,
                        RemInitDatetime = t108.RemInitDatetime
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public void SaveDetails(CallRemarkingModel model)
        {
            if (string.IsNullOrEmpty(model.CaseNos)) return;

            string[] CaseNos = model.CaseNos.Split(",");

            foreach (var details in CaseNos)
            {
                string[] data = details.Split("##");
                if (data.Length == 3)
                {
                    T108RemarkedCall remarkedCall = new()
                    {
                        CaseNo = data[0],
                        CallRecvDt = Convert.ToDateTime(data[1]),
                        CallSno = short.Parse(data[2]),
                        RemarkReason = model.RemarkingStatus,
                        FrIeCd = model.FrIeCd,
                        ToIeCd = model.ToIeCd,
                        RemInitBy = model.RemInitBy,
                        RemInitDatetime = DateTime.Now,
                        RemarkingStatus = "P",
                        FrIePendingCalls = model.FrIePendingCalls,
                        ToIePendingCalls = model.ToIePendingCalls
                    };

                    context.T108RemarkedCalls.Add(remarkedCall);
                }
            }

            context.SaveChanges();
        }

        public DTResult<PendingCallsListModel> GetCallRemarkingListForApproval(DTParameters dtParameters)
        {
            DTResult<PendingCallsListModel> dTResult = new() { draw = 0 };
            IQueryable<PendingCallsListModel>? query = null;

            string Region = !string.IsNullOrEmpty(dtParameters.AdditionalValues["Region"]) ? Convert.ToString(dtParameters.AdditionalValues["Region"]) : "";
            int UserId = !string.IsNullOrEmpty(dtParameters.AdditionalValues["UserId"]) ? int.Parse(dtParameters.AdditionalValues["UserId"]) : 0;

            var CallRemarking = context.T02Users.Where(x => x.Id == UserId).Select(x => x.CallRemarking).FirstOrDefault();

            if (CallRemarking != "Y")
            {
                List<PendingCallsListModel> lst = new();
                dTResult.recordsTotal = 0;
                dTResult.recordsFiltered = 0;
                dtParameters.Length = 0;
                dTResult.data = lst.ToList();
                dTResult.draw = dtParameters.Draw;
                return dTResult;
            }

            var UserType = context.T02Users.Where(x => x.Id == UserId).Select(x => x.UserType).FirstOrDefault();

            if (UserType == "S")
            {
                query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                        join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                        join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (short)t108.CallSno }
                            equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region
                        select new PendingCallsListModel
                        {
                            Id = t108.Id,
                            CaseNo = t108.CaseNo,
                            CallRecvDt = t108.CallRecvDt,
                            CallSno = t108.CallSno,
                            CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                            RemarkReason = t108.RemarkReason,
                            FrIeName = t09From.IeName,
                            ToIeName = t10To.IeName,
                            FrIePendingCalls = t108.FrIePendingCalls,
                            ToIePendingCalls = t108.ToIePendingCalls,
                            UserName = t02.UserName,
                            RemInitDatetime = t108.RemInitDatetime,
                        };
            }
            else
            {
                query = from t108 in context.T108RemarkedCalls
                        join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                        join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                        join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                        join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (short)t108.CallSno }
                            equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                        where t108.RemarkingStatus == "P" && t108.CaseNo.Substring(0, 1) == Region
                         && (t17.CallRemarkStatus == null || t17.CallRemarkStatus == "0")
                        select new PendingCallsListModel
                        {
                            Id = t108.Id,
                            CaseNo = t108.CaseNo,
                            CallRecvDt = t108.CallRecvDt,
                            CallSno = t108.CallSno,
                            CallRemarkStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                            RemarkReason = t108.RemarkReason,
                            FrIeName = t09From.IeName,
                            ToIeName = t10To.IeName,
                            FrIePendingCalls = t108.FrIePendingCalls,
                            ToIePendingCalls = t108.ToIePendingCalls,
                            UserName = t02.UserName,
                            RemInitDatetime = t108.RemInitDatetime,
                        };
            }

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;

        }

        public CallRemarkingApprovalModel FindCallRemarkingDetailsForApproval(int id)
        {
            CallRemarkingApprovalModel model = new();

            model = (from t108 in context.T108RemarkedCalls
                     join t09From in context.T09Ies on t108.FrIeCd equals t09From.IeCd
                     join t10To in context.T09Ies on t108.ToIeCd equals t10To.IeCd
                     join t02 in context.T02Users on t108.RemInitBy equals t02.UserId
                     join t08 in context.T08IeControllOfficers on t10To.IeCoCd equals t08.CoCd
                     join t17 in context.T17CallRegisters on new { t108.CaseNo, CallRecvDt = (DateTime)t108.CallRecvDt, CallSno = (short)t108.CallSno }
                         equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                     join v05 in context.V05Vendors on t17.MfgCd equals v05.VendCd
                     join t18 in context.T18CallDetails on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                         equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                     where t108.RemarkingStatus == "P" && t108.Id == id
                        && t18.ItemSrnoPo == (from b in context.T18CallDetails
                                              where b.CaseNo == t18.CaseNo && b.CallRecvDt == t18.CallRecvDt && b.CallSno == t18.CallSno
                                              select b.ItemSrnoPo).Min()
                     select new CallRemarkingApprovalModel
                     {
                         Id = t108.Id,
                         CaseNo = t108.CaseNo,
                         CallRecvDt = t108.CallRecvDt,
                         CallSno = t108.CallSno,
                         RemarkingStatus = t108.RemarkingStatus == "P" ? "Pending" : null,
                         RemarkReason = t108.RemarkReason,
                         FrIeName = t09From.IeName,
                         ToIeName = t10To.IeName,
                         FrIePendingCalls = t108.FrIePendingCalls,
                         ToIePendingCalls = t108.ToIePendingCalls,
                         IeCd = t10To.IeCd,
                         CoCd = t08.CoCd,
                         UserName = t02.UserName,
                         CALL_DES_DT = t17.DtInspDesire,
                         Mfg = v05.VendCd + "-" + v05.Vendor,
                         ItemDescPo = t18.ItemDescPo,
                         CallRemarkStatus = t108.CallRemarkStatus ?? "0",
                         COUNT = (from a in context.T18CallDetails
                                  where a.CaseNo == t18.CaseNo
                                     && a.CallRecvDt == t18.CallRecvDt
                                     && a.CallSno == t18.CallSno
                                  select a).Count()
                     }).FirstOrDefault();

            if (model != null)
            {
                if (model.COUNT > 1)
                {
                    model.ItemDescPo = model.ItemDescPo + " and more items as per call";
                }

                OracleParameter[] par = new OracleParameter[4];
                par[0] = new OracleParameter("p_call_recv_dt", OracleDbType.Date, model.CallRecvDt, ParameterDirection.Input);
                par[1] = new OracleParameter("p_case_no", OracleDbType.Varchar2, model.CaseNo, ParameterDirection.Input);
                par[2] = new OracleParameter("p_call_sno", OracleDbType.Int32, model.CallSno, ParameterDirection.Input);
                par[3] = new OracleParameter("p_result_cursor", OracleDbType.RefCursor, ParameterDirection.Output);

                var ds = DataAccessDB.GetDataSet("SP_GET_CALL_REMARK_APPROVAL_DETAILS", par, 1);

                if (ds != null && ds.Tables.Count > 0)
                {
                    string RLY = string.Empty;
                    string CLIENT_RLY = string.Empty;
                    decimal? MAT_VALUE = null;
                    DateTime? EXT_DELV_DT = null;
                    DateTime? INSP_DESIRE_DT = null;
                    string PENDING_SINCE = string.Empty;

                    if (ds.Tables[0].Rows[0]["RLY"] != null) RLY = Convert.ToString(ds.Tables[0].Rows[0]["RLY"]);
                    if (ds.Tables[0].Rows[0]["CLIENT_RLY"] != null) CLIENT_RLY = Convert.ToString(ds.Tables[0].Rows[0]["CLIENT_RLY"]);
                    if (ds.Tables[0].Rows[0]["MAT_VALUE"] != null) MAT_VALUE = Convert.ToDecimal(ds.Tables[0].Rows[0]["MAT_VALUE"]);
                    if (ds.Tables[0].Rows[0]["EXT_DELV_DT"] != null) EXT_DELV_DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["EXT_DELV_DT"]);
                    if (ds.Tables[0].Rows[0]["INSP_DESIRE_DT"] != null) INSP_DESIRE_DT = Convert.ToDateTime(ds.Tables[0].Rows[0]["INSP_DESIRE_DT"]);
                    if (ds.Tables[0].Rows[0]["PENDING_SINCE"] != null) PENDING_SINCE = Convert.ToString(ds.Tables[0].Rows[0]["PENDING_SINCE"]);

                    model.RLY = RLY + " & " + CLIENT_RLY;
                    model.MAT_VALUE = MAT_VALUE;
                    model.ExtDelvDt = EXT_DELV_DT;
                    model.DtInspDesire = INSP_DESIRE_DT;
                    model.pending_since = PENDING_SINCE + " Days";
                }
            }

            return model;
        }

        public void SaveDetails(CallRemarkingApprovalModel model)
        {
            if (model.Action == "Approve")
            {
                model.CallRemarkStatus = Convert.ToString(Convert.ToInt32(model.CallRemarkStatus) + 1);

                T17CallRegister callRegister = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();

                if (callRegister != null)
                {
                    callRegister.IeCd = model.IeCd;
                    callRegister.CoCd = model.CoCd;
                    callRegister.CallRemarkStatus = model.CallRemarkStatus;
                    callRegister.DtInspDesire = model.DtInspDesire;

                    context.SaveChanges();
                }

                T108RemarkedCall remarkedCall = context.T108RemarkedCalls.Find(model.Id);

                if (remarkedCall != null)
                {
                    remarkedCall.CallRemarkStatus = model.CallRemarkStatus;
                    remarkedCall.RemarkingStatus = "A";
                    remarkedCall.RemAppBy = model.UserId;
                    remarkedCall.RemAppDatetime = DateTime.Now;
                    remarkedCall.RemRejRemark = model.Remark;

                    context.SaveChanges();
                }
            }
            else if (model.Action == "Reject")
            {
                T108RemarkedCall remarkedCall = context.T108RemarkedCalls.Find(model.Id);

                if (remarkedCall != null)
                {
                    remarkedCall.RemarkingStatus = "R";
                    remarkedCall.RemAppBy = model.UserId;
                    remarkedCall.RemAppDatetime = DateTime.Now;
                    remarkedCall.RemRejRemark = model.Remark;

                    context.SaveChanges();
                }
            }
        }
    }
}

