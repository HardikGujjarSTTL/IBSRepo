using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }

}

