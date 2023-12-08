using IBSAPI.DataAccess;
using IBSAPI.Helper;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace IBSAPI.Repositories
{
    public class CallRepository : ICallRepository
    {

        private readonly ModelContext context;
        public CallRepository(ModelContext context)
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

        public int SheduleInspection(SheduleInspectionRequestModel sheduleInspectionRequestModel, int PlanDHours)
        {
            int ID = 0;
            string CallRecvDt = sheduleInspectionRequestModel.CallRecvDt.ToString("dd-MM-yy");
            var query = (from t17 in context.T17CallRegisters
                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                         where t17.CaseNo == sheduleInspectionRequestModel.CaseNo && t17.CallRecvDt.Date == sheduleInspectionRequestModel.CallRecvDt.Date
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
                    DateTime CDate = DateTime.Now;
                    if (sheduleInspectionRequestModel.InspectionDay == "TD")
                    {
                        if (CDate.Hour > PlanDHours)
                        {
                            ID = 999;
                        }
                        else
                        {
                            T47IeWorkPlan obj = new T47IeWorkPlan();
                            obj.IeCd = query.IeCd;
                            obj.CoCd = Convert.ToByte(query.CoCd);
                            if (sheduleInspectionRequestModel.InspectionDay == "TD")
                            {
                                obj.VisitDt = Convert.ToDateTime(DateTime.Now);
                            }
                            else if (sheduleInspectionRequestModel.InspectionDay == "TM")
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
                    else
                    {
                        T47IeWorkPlan obj = new T47IeWorkPlan();
                        obj.IeCd = query.IeCd;
                        obj.CoCd = Convert.ToByte(query.CoCd);
                        if (sheduleInspectionRequestModel.InspectionDay == "TD")
                        {
                            obj.VisitDt = Convert.ToDateTime(DateTime.Now);
                        }
                        else if (sheduleInspectionRequestModel.InspectionDay == "TM")
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
            }
            return ID;
        }

        public List<CallStatusModel> Get_Call_Status_List()
        {
            List<CallStatusModel> lstStatus = new();

            lstStatus = (from x in context.T21CallStatusCodes
                         select new CallStatusModel
                         {
                             CallStatusCd = x.CallStatusCd,
                             CallStatusDesc = x.CallStatusDesc,
                             CallStatusColor = x.CallStatusColor,
                         }).ToList();
            return lstStatus;
        }
        public int CancelInspection(int IeCd, string CaseNo, DateTime PlanDt, DateTime CallRecvDt, int CallSno)
        {
            int ID = 0;
            var T47 = context.T47IeWorkPlans.Where(x => x.IeCd == IeCd && x.VisitDt.Date == PlanDt.Date && x.CaseNo == CaseNo && x.CallRecvDt.Date == CallRecvDt.Date && x.CallSno == CallSno).FirstOrDefault();
            if (T47 != null)
            {
                context.T47IeWorkPlans.RemoveRange(T47);
                context.SaveChanges();
                ID = Convert.ToInt32(T47.CallSno);
            }
            return ID;
        }
    }
}
