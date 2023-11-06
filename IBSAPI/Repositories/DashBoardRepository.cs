using IBSAPI.DataAccess;
using IBSAPI.Interfaces;

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
            var totalCnt = (from x in context.T17CallRegisters
                            where x.IeCd == IeCd && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            select x).Count();
            return totalCnt;
        }

        public int GetIECompletedInspection(int IeCd, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "A", "R", "C", "G", "B", "T", "PR", "PRB", "CB", "RB" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from x in context.T17CallRegisters
                            where x.IeCd == IeCd && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            && allowedStatuses.Contains(x.CallStatus)
                            select x).Count();
            return totalCnt;
        }

        public int GetIEPendingInspection(int IeCd, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from x in context.T17CallRegisters
                            where x.IeCd == IeCd && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            && allowedStatuses.Contains(x.CallStatus)
                            select x).Count();
            return totalCnt;
        }
        #endregion

        #region DashBoard For Vendor
        public int GetVendorTotalAssignInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            var totalCnt = (from x in context.T17CallRegisters
                            where x.MfgCd == Vendor_ID && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            select x).Count();
            return totalCnt;
        }

        public int GetVendorCompletedInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "A", "R", "C", "G", "B", "T", "PR", "PRB", "CB", "RB" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from x in context.T17CallRegisters
                            where x.MfgCd == Vendor_ID && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            && allowedStatuses.Contains(x.CallStatus)
                            select x).Count();
            return totalCnt;
        }

        public int GetVendorPendingInspection(int Vendor_ID, string FromDate, string ToDate)
        {
            var allowedStatuses = new string[] { "M", "U", "S", "W" };
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);

            var totalCnt = (from x in context.T17CallRegisters
                            where x.MfgCd == Vendor_ID && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
                            && allowedStatuses.Contains(x.CallStatus)
                            select x).Count();
            return totalCnt;
        }
        #endregion

        #region Client
        public int GetClientTotalInspection(string Rly_CD, string RlyNoNType, string FromDate, string ToDate)
        {
            DateTime fromDT = DateTime.ParseExact(FromDate, "dd/MM/yyyy", null);
            DateTime toDT = DateTime.ParseExact(ToDate, "dd/MM/yyyy", null);
            //var totalInspCount = (from t13 in context.T13PoMasters
            //                      join t17 in context.T17CallRegisters on t13.CaseNo equals t17.CaseNo
            //                      where t13.RlyCd == "SR" && t13.RlyNonrly == "R" 
            //                            && x.CallMarkDt >= fromDT && x.CallMarkDt <= toDT
            //                      select t13).Count();
            //return totalInspCount;
            return 0;
        }
        #endregion
    }
}
