using IBS.DataAccess;
using IBS.Interfaces.IE;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

namespace IBS.Repositories.IE
{
    public class DailyWorkPlanRepository : IDailyWorkPlanRepository
    {
        private readonly ModelContext context;

        public DailyWorkPlanRepository(ModelContext context)
        {
            this.context = context;
        }

        public DailyWorkPlanModel FindByDetails(DailyWorkPlanModel model, string Region)
        {
            var defaultDate = DateTime.ParseExact("28/01/2013", "dd/MM/yyyy", null);
            var Dt = context.T09Ies.Where(x => x.IeCd == model.IeCd).Select(x => new { JOINDT = x.IeJoinDt != null ? x.IeJoinDt.Value.ToString("dd/MM/yyyy") : defaultDate.ToString("dd/MM/yyyy") }).FirstOrDefault();


            model.FromDt = DateTime.Now.Date;
            model.ToDt = DateTime.Now.Date.AddDays(1);


            string WkDt = Convert.ToString(Dt.JOINDT);
            string myYear, myMonth, myDay;
            myYear = WkDt.Substring(6, 4);
            myMonth = WkDt.Substring(3, 2);
            myDay = WkDt.Substring(0, 2);
            string dt1 = myYear + myMonth + myDay;
            int c = dt1.CompareTo("20130128");



            DateTime IEWkDt = Convert.ToDateTime(WkDt);
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            DateTime toDtValue = DateTime.Now;

            //comment remove testing purpose only
            model.FromDt = Convert.ToDateTime("22/04/2019");
            model.ToDt = Convert.ToDateTime("23/04/2019");

            DateTime tdt = Convert.ToDateTime(model.FromDt);

            int numberOfDays = 0;
            if (c < 0)
            {
                startDate = DateTime.ParseExact("28/01/2013", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                endDate = tdt;
                toDtValue = DateTime.ParseExact("27/07/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                numberOfDays = (endDate - startDate).Days + 1;
            }
            else
            {
                startDate = IEWkDt;
                endDate = tdt;
                toDtValue = DateTime.ParseExact("27/07/2015", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                numberOfDays = (endDate - startDate).Days + 1;
            }

            var query = Enumerable.Range(0, numberOfDays)
                .Select(offset => startDate.AddDays(offset))
                .Where(date => date <= (endDate - TimeSpan.FromDays(1)))
                .OrderByDescending(date => date)
                .Select(date => date.ToString("dd/MM/yyyy"));
            var results = query.ToList();

            int err = 0;
            for (int i = 0; i <= results.Count - 1; i++)
            {
                var targetDate = IEWkDt;
                var ieCd = model.IeCd;

                var count1 = context.T47IeWorkPlans.Where(t => t.VisitDt == Convert.ToDateTime(results[i]) && t.IeCd == ieCd).Count();
                var count2 = context.T48NiIeWorkPlans.Where(t => t.NiWorkDt == Convert.ToDateTime(results[i]) && t.IeCd == ieCd).Count();
                var count3 = context.NoIeWorkPlans.Where(t => t.NwpDt == Convert.ToDateTime(results[i]) && t.IeCd == ieCd).Count();

                int TotalCount = count1 + count2 + count3;
                if (TotalCount == 0)
                {
                    err = 1;
                    model.NwpDt = Convert.ToDateTime(results[i]);
                }
                else if (TotalCount >= 1)
                {
                    break;
                }
            }
            model.errcode = err;





            return model;
        }

        public DTResult<DailyWorkPlanModel> GetLoadTable(DTParameters dtParameters, string Region, int GetIeCd)
        {

            DTResult<DailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<DailyWorkPlanModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }
            string PlanDt = "", InspWorkType = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PlanDt"]))
            {
                PlanDt = Convert.ToString(dtParameters.AdditionalValues["PlanDt"]);
            }
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["InspWorkType"]))
            {
                InspWorkType = Convert.ToString(dtParameters.AdditionalValues["InspWorkType"]);
            }

            query = from t17 in context.T17CallRegisters
                    join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                    join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                    where t17.CaseNo.StartsWith(Region)
                          && t17.IeCd == GetIeCd
                          && new[] { "M", "U", "S", "W" }.Contains(t17.CallStatus)
                          && !(from t47 in context.T47IeWorkPlans
                               where t17.CaseNo == t47.CaseNo
                                     && t17.CallRecvDt == t47.CallRecvDt
                                     && t17.CallSno == t47.CallSno
                                     && t47.VisitDt == DateTime.ParseExact(PlanDt, "dd/MM/yyyy", null)
                               select t47).Any()
                    //orderby t03.City, t05.VendName, t17.CallRecvDt, t17.CallSno ascending
                    orderby t03.City ascending
                    select new DailyWorkPlanModel
                    {
                        CaseNo = t17.CaseNo,
                        CallRecvDt = t17.CallRecvDt,
                        CallSno = t17.CallSno,
                        CallStatus = t17.CallStatus == "M" ? "Pending" :
                                          t17.CallStatus == "U" ? "Under Lab Testing" :
                                          t17.CallStatus == "S" ? "Still Under Inspection" :
                                          t17.CallStatus == "G" ? "Stage Inspection" :
                                          t17.CallStatus == "W" ? "Withheld" : "",
                        MfgCd = t17.MfgCd,
                        MfgPlace = t17.MfgPlace,
                        CoCd = t17.CoCd ?? 0,
                        VendName = t05.VendName,
                        CityCd = t03.CityCd,
                        MFGCity = t03.City,
                        DtInspDesire = t17.DtInspDesire.HasValue ? t17.DtInspDesire.Value : null
                    };

            //query = from l in context.NoIeWorkPlans
            //        where l.IeCd == GetIeCd
            //        select new DailyWorkPlanModel
            //        {
            //            IeCd = l.IeCd,
            //            CoCd = l.CoCd,
            //            Reason = l.Reason,
            //            NwpDt = l.NwpDt,
            //            RegionCode = l.RegionCode,
            //            UserId = l.UserId,
            //            Datetime = l.Datetime
            //        };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<DailyWorkPlanModel> GetLoadTableCurrentDay(DTParameters dtParameters, string Region, int GetIeCd)
        {
            DTResult<DailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<DailyWorkPlanModel>? query = null;

            var searchBy = dtParameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;

                if (orderCriteria == "")
                {
                    orderCriteria = "CaseNo";
                }
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CaseNo";
                orderAscendingDirection = true;
            }
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
                        where t47.IeCd == GetIeCd && t47.VisitDt == Convert.ToDateTime(PlanDt)
                        orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                        select new DailyWorkPlanModel
                        {
                            VisitDt = t47.VisitDt,
                            CaseNo = t47.CaseNo,
                            CallRecvDt = t47.CallRecvDt,
                            CallSno = t47.CallSno,
                            VendName = t05.VendName,
                            MfgPlace = t47.MfgPlace,
                            MFGCity = t03.City
                        };

            dTResult.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(w => Convert.ToString(w.CaseNo).ToLower().Contains(searchBy.ToLower())
                );

            dTResult.recordsFiltered = query.Count();

            dTResult.data = DbContextHelper.OrderByDynamic(query, orderCriteria, orderAscendingDirection).Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }



        public int DetailsInsertUpdate(DailyWorkPlanModel model)
        {
            int ID = 0;
            var co_cd = context.T09Ies.Where(x => x.IeCd == model.IeCd).FirstOrDefault();
            if (co_cd != null)
            {
                NoIeWorkPlan obj = new NoIeWorkPlan();
                obj.IeCd = model.IeCd;
                obj.CoCd = Convert.ToByte(co_cd.IeCoCd);
                obj.Reason = model.Reason;
                obj.NwpDt = model.ReasonDt;
                obj.RegionCode = model.RegionCode;
                obj.UserId = model.Createdby;
                obj.Datetime = DateTime.Now;
                context.NoIeWorkPlans.Add(obj);
                context.SaveChanges();
                ID = Convert.ToInt32(obj.IeCd);
            }

            return ID;
        }

        //public string SaveDetails(DailyWorkPlanModel model, string Region)
        //{
        //    string str = "";

        //    return str;
        //}
    }
}
