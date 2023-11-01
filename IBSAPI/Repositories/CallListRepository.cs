using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;

namespace IBSAPI.Repositories
{
    public class CallListRepository : ICallListRepository
    {

        private readonly ModelContext context;
        public CallListRepository(ModelContext context)
        {
            this.context = context;
        }

        public List<CallListModel> GetCallList()
        {
            List<CallListModel> lst = new();

            lst = (from x in context.T01Regions
                   select new CallListModel
                   {
                       ID = x.RegionCode,
                       Name = x.Region,
                       MobileNo = "+91 " + x.MobileNo,
                   }).ToList();
            return lst;
        }

        public int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel)
        {
            int ID = 0;
            var query = (from t17 in context.T17CallRegisters
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t17.CaseNo == sheduleInspectionRequestModel.CaseNo && t17.CallRecvDt == sheduleInspectionRequestModel.CallRecvDt 
                         && t17.CallSno == sheduleInspectionRequestModel.CallSno
                         select new
                         {
                             t17.CaseNo,
                             t17.CallRecvDt,
                             t17.CallSno,
                             t17.CallStatus,
                             t17.MfgCd,
                             t17.MfgPlace,
                             t17.IeCd,
                             t17.CoCd,
                             t03.CityCd,
                             t03.City,
                             t17.DtInspDesire
                         }).FirstOrDefault();
            if (query != null)
            {
                if (sheduleInspectionRequestModel.CaseNo != null && sheduleInspectionRequestModel.CallRecvDt != null && sheduleInspectionRequestModel.CallSno > 0)
                {
                    
                    T47IeWorkPlan obj = new T47IeWorkPlan();
                    obj.IeCd = query.IeCd;
                    obj.CoCd = Convert.ToByte(query.CoCd);
                    if (sheduleInspectionRequestModel.InspectionDay == "TD")
                    {
                        obj.VisitDt = Convert.ToDateTime(DateTime.Now);
                    }
                    else if(sheduleInspectionRequestModel.InspectionDay == "TM")
                    {
                        obj.VisitDt = Convert.ToDateTime(DateTime.Now.AddDays(1));
                    }
                    obj.CaseNo = query.CaseNo;
                    obj.CallRecvDt = query.CallRecvDt;
                    obj.CallSno = query.CallSno;
                    obj.MfgCd = query.MfgCd;
                    obj.MfgPlace = query.MfgPlace;
                    obj.RegionCode = sheduleInspectionRequestModel.RegionCode;
                    obj.UserId = sheduleInspectionRequestModel.UserId;
                    obj.Datetime = DateTime.Now;
                    context.T47IeWorkPlans.Add(obj);
                    context.SaveChanges();
                    ID = Convert.ToInt32(obj.CallSno);
                }
            }
            return ID;
        }

    }
}
