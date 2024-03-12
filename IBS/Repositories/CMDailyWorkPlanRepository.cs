using IBS.DataAccess;
using IBS.Interfaces;
using IBS.Models;
using Newtonsoft.Json;

namespace IBS.Repositories
{
    public class CMDailyWorkPlanRepository : ICMDailyWorkPlanRepository
    {
        private readonly ModelContext context;

        public CMDailyWorkPlanRepository(ModelContext context)
        {
            this.context = context;
        }

        public DTResult<CMDailyWorkPlanModel> GetLoadTable(DTParameters dtParameters, string Region)
        {
            DTResult<CMDailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<CMDailyWorkPlanModel>? query = null;

            string PlanDt = "", InspWorkType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PlanDt"]))
            {
                PlanDt = Convert.ToString(dtParameters.AdditionalValues["PlanDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["InspWorkType"]))
            {
                InspWorkType = Convert.ToString(dtParameters.AdditionalValues["InspWorkType"]);
            }

            query = from t47 in context.T47IeWorkPlans
                    join t05 in context.T05Vendors on t47.MfgCd equals t05.VendCd
                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                    join t09 in context.T09Ies on t47.IeCd equals t09.IeCd
                    join t17 in context.T17CallRegisters on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno } equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno }
                    where t47.VisitDt == Convert.ToDateTime(PlanDt) && t47.RegionCode == Region
                    orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                    select new CMDailyWorkPlanModel
                    {
                        VisitDt = t47.VisitDt,
                        CaseNo = t47.CaseNo,
                        CallRecvDt = t47.CallRecvDt,
                        CallSno = t47.CallSno,
                        VendName = t05.VendName,
                        MfgPlace = t47.MfgPlace,
                        MFGCity = t03.City,
                        IeName = t09.IeName,
                        CmApproval = t17.CmApproval,
                        IsUrgency = t17.Isfinalizedstatus
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int SaveApproval(CMDailyWorkPlanModel model, string Region)
        {
            int ID = 0;
            List<DeSerializeDailyWorkModel> deserializedData = JsonConvert.DeserializeObject<List<DeSerializeDailyWorkModel>>(model.checkedWork);
            foreach (var details in deserializedData)
            {
                model.CaseNo = details.CaseNo;
                model.CallRecvDt = details.CallRecvDt;
                model.CallSno = details.CallSno;

                var T17 = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                if (T17 != null)
                {
                    T17.CmApproval = "A";
                    T17.CmApprovalDt = DateTime.Now.Date;

                    context.SaveChanges();
                    ID = Convert.ToInt32(T17.CallSno);
                }
            }
            return ID;
        }

        public int UpdateUrgency(CMDailyWorkPlanModel model, string Region)
        {
            int ID = 0;
            List<DeSerializeDailyWorkModel> deserializedData = JsonConvert.DeserializeObject<List<DeSerializeDailyWorkModel>>(model.Urgency);
            foreach (var details in deserializedData)
            {
                model.CaseNo = details.CaseNo;
                model.CallRecvDt = details.CallRecvDt;
                model.CallSno = details.CallSno;

                var T17 = context.T17CallRegisters.Where(x => x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                if (T17 != null)
                {
                    T17.Isfinalizedstatus = details.IsUrgency;
                    context.SaveChanges();
                    ID = Convert.ToInt32(T17.CallSno);
                }
            }
            return ID;
        }
    }
}
