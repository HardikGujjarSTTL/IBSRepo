using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IBSAPI.Repositories
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly ModelContext context;
        public DashBoardRepository(ModelContext context)
        {
            this.context = context;
        }

        #region DashBoard For IE 
        public int GetIETotalAssignInspection(int IeCd, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            where t17.IeCd == IeCd && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            select t17).Count();
            return totalCnt;
        }

        public int GetIECompletedInspection(int IeCd, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "A", "R", "C", "G", "B", "T", "PR", "PRB", "CB", "RB" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            where t17.IeCd == IeCd && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            && allowedStatuses.Contains(t17.CallStatus)
                            select t17).Count();
            return totalCnt;
        }

        public int GetIEPendingInspection(int IeCd, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            where t17.IeCd == IeCd && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            && allowedStatuses.Contains(t17.CallStatus)
                            select t17).Count();
            return totalCnt;
        }
        #endregion

        #region DashBoard For Vendor
        public int GetVendorTotalAssignInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            //join t21 in context.T21CallStatusCodes on t17.CallStatus.Trim() equals t21.CallStatusCd.Trim()
                            where t17.MfgCd == Vendor_ID && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            select t17).Count();
            return totalCnt;
        }

        public int GetVendorCompletedInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "A", "R", "C", "G", "B", "T", "PR", "PRB", "CB", "RB" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            //join t21 in context.T21CallStatusCodes on t17.CallStatus.Trim() equals t21.CallStatusCd.Trim()
                            where t17.MfgCd == Vendor_ID && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            && allowedStatuses.Contains(t17.CallStatus)
                            select t17).Count();
            return totalCnt;
        }

        public int GetVendorPendingInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from t17 in context.T17CallRegisters
                            join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo into t13Group
                            from t13 in t13Group.DefaultIfEmpty() // Left Join
                            join t09 in context.T09Ies on t17.IeCd equals t09.IeCd into t09Group
                            from t09 in t09Group.DefaultIfEmpty() // Left Join
                            join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd into t05Group
                            from t05 in t05Group.DefaultIfEmpty() // Left Join
                            //join t21 in context.T21CallStatusCodes on t17.CallStatus.Trim() equals t21.CallStatusCd.Trim()                            
                            where t17.MfgCd == Vendor_ID && t17.CallMarkDt >= fromDT && t17.CallMarkDt <= toDT
                            && allowedStatuses.Contains(t17.CallStatus)
                            select t17).Count();
            return totalCnt;
        }
        #endregion

        #region Client
        public int GetClientTotalInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalInspCount = (from t17 in context.T17CallRegisters
                                  join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                                  where t13.RlyCd == Rly_CD && t13.RlyNonrly == Rly_NoNType
                                        && t17.CallRecvDt >= fromDT && t17.CallRecvDt <= toDT
                                  select t13).Count();
            return totalInspCount;
        }

        public int GetClientCompletedInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "A", "R", "C", "G", "B", "T", "PR", "PRB", "CB", "RB" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalInspCount = (from t17 in context.T17CallRegisters
                                  join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                                  where t13.RlyCd == Rly_CD && t13.RlyNonrly == Rly_NoNType
                                        && allowedStatuses.Contains(t17.CallStatus)
                                        && t17.CallRecvDt >= fromDT && t17.CallRecvDt <= toDT
                                  select t13).Count();
            return totalInspCount;
        }

        public int GetClientPendingInspection(string Rly_CD, string Rly_NoNType, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalInspCount = (from t17 in context.T17CallRegisters
                                  join t13 in context.T13PoMasters on t17.CaseNo equals t13.CaseNo
                                  where t13.RlyCd == Rly_CD && t13.RlyNonrly == Rly_NoNType
                                        && allowedStatuses.Contains(t17.CallStatus)
                                        && t17.CallRecvDt >= fromDT && t17.CallRecvDt <= toDT
                                  select t13).Count();
            return totalInspCount;
        }
        #endregion

        #region CM
        public List<IEModel> Get_CM_Wise_IE(int CO_CD)
        {
            List<IEModel> lstIE = new();
            var IeList = (from x in context.T09Ies
                          where x.IeCoCd == CO_CD
                          select new IEModel
                          {
                              IE_CD = x.IeCd,
                              IE_Name = x.IeName
                          }).ToList();
            return IeList;
        }

        public int Get_CM_TotalInspection(int CO_CD, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalInsp = (from x in context.T17CallRegisters
                             where x.CoCd == CO_CD && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                             select x).Count();
            return totalInsp;
        }

        public int Get_CM_PendingInspection(int CO_CD, string FromDate, string ToDate)
        {
            var validCallStatus = new List<string> { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalInsp = (from x in context.T17CallRegisters
                             where x.CoCd == CO_CD && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                             && validCallStatus.Contains(x.CallStatus)
                             select x).Count();
            return totalInsp;
        }

        public int Get_CM_RequestRejectedInspection(int CO_CD, string FromDate, string ToDate)
        {
            var validCallStatus = new List<string> { "R", "T" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalInsp = (from x in context.T17CallRegisters
                             where x.CoCd == CO_CD && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                             && validCallStatus.Contains(x.CallStatus)
                             select x).Count();
            return totalInsp;
        }
        #endregion
    }
}
