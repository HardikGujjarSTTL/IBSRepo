using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Numerics;

namespace IBSAPI.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ModelContext context;
        public InspectionRepository(ModelContext context)
        {
            this.context = context;
        }
        public List<TodayInspectionModel> GetToDayInspection(int IeCd)
        {
            var currDate = DateTime.Now.ToString("dd/MM/yyyy");

            List<TodayInspectionModel> lst = new();
            lst = (from t47 in context.T47IeWorkPlans
                   join t17 in context.T17CallRegisters
                       on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno }
                       equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                   join t18 in context.T18CallDetails
                       on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno }
                       equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                   join t05 in context.T05Vendors on t47.MfgCd equals t05.VendCd
                   join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                   join t21 in context.T21CallStatusCodes on t17.CallStatus equals t21.CallStatusCd
                   where t47.IeCd == IeCd //670
                       && t47.VisitDt == DateTime.ParseExact(currDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) // Assuming PlanDt is in 'dd/MM/yyyy' format
                   orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                   select new TodayInspectionModel
                   {
                       //VisitDt = t47.VisitDt,
                       Case_No = t47.CaseNo,
                       Call_Recv_Dt = t47.CallRecvDt,
                       Call_Sno = t47.CallSno,
                       Name = t18.ItemDescPo,
                       Vend_Name = t05.VendName,
                       Qty = t18.QtyToInsp,
                       Status = t21.CallStatusDesc
                   }).ToList();
            return lst;
        }

        public List<TomorrowInspectionModel> GetTomorrowInspection(int IeCd)
        {
            var tomoDate = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");

            List<TomorrowInspectionModel> lst = new();
            lst = (from t47 in context.T47IeWorkPlans
                   join t17 in context.T17CallRegisters
                       on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno }
                       equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                   join t18 in context.T18CallDetails
                       on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno }
                       equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                   join t05 in context.T05Vendors on t47.MfgCd equals t05.VendCd
                   join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                   join t21 in context.T21CallStatusCodes on t17.CallStatus equals t21.CallStatusCd
                   where t47.IeCd == IeCd //670
                       && t47.VisitDt == DateTime.ParseExact(tomoDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) // Assuming PlanDt is in 'dd/MM/yyyy' format
                   orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                   select new TomorrowInspectionModel
                   {
                       //VisitDt = t47.VisitDt,
                       Case_No = t47.CaseNo,
                       Call_Recv_Dt = t47.CallRecvDt,
                       Call_Sno = t47.CallSno,
                       Name = t18.ItemDescPo,
                       Vend_Name = t05.VendName,
                       Qty = t18.QtyToInsp,
                       Status = t21.CallStatusDesc
                   }).ToList();
            return lst;
        }

        public List<PendingInspectionModel> GetPendingInspection(int IeCd, string Region, string CurrDate)
        {
            List<PendingInspectionModel> lst = new();
            lst = (from t17 in context.T17CallRegisters
                   join t18 in context.T18CallDetails
                       on new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                       equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                   join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                   join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                   where t17.CaseNo.StartsWith(Region)
                         && t17.IeCd == IeCd
                         && new[] { "M", "U", "S", "W" }.Contains(t17.CallStatus)
                         && !(from t47 in context.T47IeWorkPlans
                              where t17.CaseNo == t47.CaseNo
                                    && t17.CallRecvDt == t47.CallRecvDt
                                    && t17.CallSno == t47.CallSno
                                    && t47.VisitDt == DateTime.ParseExact(CurrDate, "dd/MM/yyyy", null)
                              select t47).Any()
                   //orderby t03.City, t05.VendName, t17.CallRecvDt, t17.CallSno ascending
                   orderby t03.City ascending
                   select new PendingInspectionModel
                   {
                       Case_No = t17.CaseNo,
                       Call_Recv_Dt = t17.CallRecvDt,
                       Call_Sno = t17.CallSno,
                       Name = t18.ItemDescPo,
                       Vend_Name = t05.VendName,
                       Qty = t18.QtyToInsp,
                       Status = t17.CallStatus == "M" ? "Pending" :
                                         t17.CallStatus == "U" ? "Under Lab Testing" :
                                         t17.CallStatus == "S" ? "Still Under Inspection" :
                                         t17.CallStatus == "G" ? "Stage Inspection" :
                                         t17.CallStatus == "W" ? "Withheld" : ""
                   }).ToList();
            return lst;
        }
        public CaseDetailIEModel GetCaseDetailForIE(string Case_No, string CallRecvDt, int CallSNo, int IeCd)
        {
            var query = (from x in context.T17CallRegisters
                         join t18 in context.T18CallDetails
                              on new { x.CaseNo, x.CallRecvDt, x.CallSno }
                              equals new { t18.CaseNo, t18.CallRecvDt, t18.CallSno }
                         join y in context.T13PoMasters on x.CaseNo equals y.CaseNo
                         join p in context.V06Consignees on y.PurchaserCd equals p.ConsigneeCd
                         join v in context.T05Vendors on y.VendCd equals v.VendCd
                         join cs in context.T21CallStatusCodes on x.CallStatus equals cs.CallStatusCd
                         join ie in context.T09Ies on x.IeCd equals ie.IeCd
                         join ct in context.T03Cities on v.VendCityCd equals ct.CityCd
                         where x.IeCd == IeCd//670
                             && x.CaseNo == Case_No//"N21080776"
                             && x.CallRecvDt == DateTime.ParseExact(CallRecvDt, "dd/MM/yyyy", null)  //"20-08-22"
                             && x.CallSno == CallSNo //13
                         group new
                         {
                             x.CaseNo,
                             x.CallRecvDt,
                             x.CallSno,
                             //t18.QtyToInsp,
                             v.VendName,
                             v.VendCd,
                             ct.City,
                             y.PoDt,
                             y.PoNo,
                             cs.CallStatusDesc,
                             ie.IeEmail,
                             ie.IePhoneNo
                         } by new
                         {
                             x.CaseNo,
                             x.CallRecvDt,
                             x.CallSno,
                             //t18.QtyToInsp,
                             v.VendName,
                             v.VendCd,
                             ct.City,
                             y.PoDt,
                             y.PoNo,
                             cs.CallStatusDesc,
                             ie.IeEmail,
                             ie.IePhoneNo
                         } into grouped
                         select new CaseDetailIEModel
                         {
                             Case_No = grouped.Key.CaseNo,
                             Call_Recv_DT = grouped.Key.CallRecvDt,
                             Call_SNo = grouped.Key.CallSno,
                             //Qty = grouped.Sum(item => item.QtyToInsp),
                             Vendor = grouped.Key.VendName,
                             Vendor_Code = Convert.ToString(grouped.Key.VendCd),
                             Vendor_City = grouped.Key.City,
                             PO_DT = grouped.Key.PoDt,
                             PO_No = grouped.Key.PoNo,
                             Status = grouped.Key.CallStatusDesc,
                             EMail = grouped.Key.IeEmail,
                             MobileNo = grouped.Key.IePhoneNo
                         }).FirstOrDefault();
            return query;
        }
    }
}
