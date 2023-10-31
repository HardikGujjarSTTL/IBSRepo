using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using System.Globalization;

namespace IBSAPI.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ModelContext context;
        public InspectionRepository(ModelContext context)
        {
            this.context = context;
        }
        public List<TodayInspectionModel> GetToDayInspection()
        {
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
                        where t47.IeCd == 670
                            && t47.VisitDt == DateTime.ParseExact("31/10/2023", "dd/MM/yyyy", CultureInfo.InvariantCulture) // Assuming PlanDt is in 'dd/MM/yyyy' format
                        orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                        select new TodayInspectionModel
                        {
                            //VisitDt = t47.VisitDt,
                            Case_No = t47.CaseNo,
                            Date = t47.CallRecvDt,
                            Call_Sno = t47.CallSno,
                            Vend_Name = t05.VendName,
                            Qty = t18.QtyToInsp,
                            //MfgPlace = t47.MfgPlace,
                            //MFGCity = t03.City,
                            Status = t21.CallStatusDesc
                        }).ToList();

            //var result = query.ToList();
            return lst;

        }
    }
}
