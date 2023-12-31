﻿using IBS.DataAccess;
using IBS.Interfaces.IE;
using IBS.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Linq;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            //model.FromDt = Convert.ToDateTime("22/04/2019");
            //model.ToDt = Convert.ToDateTime("23/04/2019");

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
                                     && t47.VisitDt.Date == DateTime.ParseExact(PlanDt, "dd/MM/yyyy", null).Date
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

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public DTResult<DailyWorkPlanModel> GetLoadTableCurrentDay(DTParameters dtParameters, string Region, int GetIeCd)
        {
            DTResult<DailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<DailyWorkPlanModel>? query = null;

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
                    where t47.IeCd == GetIeCd && t47.VisitDt.Date == Convert.ToDateTime(PlanDt).Date
                    join t17 in context.T17CallRegisters on new { t47.CaseNo, t47.CallRecvDt, t47.CallSno } equals new { t17.CaseNo, t17.CallRecvDt, t17.CallSno } into t17Group
                    from t17 in t17Group.DefaultIfEmpty()
                    orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
                    select new DailyWorkPlanModel
                    {
                        VisitDt = t47.VisitDt,
                        CaseNo = t47.CaseNo,
                        CallRecvDt = t47.CallRecvDt,
                        CallSno = t47.CallSno,
                        VendName = t05.VendName,
                        MfgPlace = t47.MfgPlace,
                        MFGCity = t03.City,
                        IsUrgency = (t17.Isfinalizedstatus == null || t17.Isfinalizedstatus == "") ? "N" : t17.Isfinalizedstatus
                    };

            //query = from t47 in context.T47IeWorkPlans
            //        join t05 in context.T05Vendors on t47.MfgCd equals t05.VendCd
            //        join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
            //        where t47.IeCd == GetIeCd && t47.VisitDt == Convert.ToDateTime(PlanDt)
            //        orderby t03.City, t05.VendName, t47.CallRecvDt, t47.CallSno
            //        select new DailyWorkPlanModel
            //        {
            //            VisitDt = t47.VisitDt,
            //            CaseNo = t47.CaseNo,
            //            CallRecvDt = t47.CallRecvDt,
            //            CallSno = t47.CallSno,
            //            VendName = t05.VendName,
            //            MfgPlace = t47.MfgPlace,
            //            MFGCity = t03.City
            //        };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public int DetailsInsertUpdate(DailyWorkPlanModel model, string Region, int GetIeCd)
        {
            int ID = 0;
            List<DeSerializeDailyWorkModel> deserializedData = JsonConvert.DeserializeObject<List<DeSerializeDailyWorkModel>>(model.checkedWork);

            //int vend_count = 0, count = 0;
            //var qryList = (from T47 in context.T47IeWorkPlans
            //               where T47.IeCd == GetIeCd && T47.VisitDt == Convert.ToDateTime(model.PlanDt)
            //               group T47 by T47.MfgCd into grouped
            //               select new
            //               {
            //                   MfgCd = grouped.Key,
            //                   Count = grouped.Count()
            //               }).ToList();


            //foreach (var dt1 in deserializedData)
            //{
            //    var InsertRcrd = (from t17 in context.T17CallRegisters
            //                      where t17.CaseNo == dt1.CaseNo && t17.CallRecvDt == dt1.CallRecvDt && t17.CallSno == dt1.CallSno
            //                      group t17 by t17.MfgCd into grouped
            //                      select new
            //                      {
            //                          MfgCd = grouped.Key,
            //                          Count = grouped.Count()
            //                      }).FirstOrDefault();
            //    foreach (var dtl in qryList)
            //    {
            //        if (InsertRcrd.MfgCd == dtl.MfgCd)
            //        {
            //            vend_count = vend_count + 1;
            //        }
            //    }
            //}

            //vend_count = vend_count + deserializedData.Count;
            //count = count + deserializedData.Count + qryList.Count;

            //if (vend_count > 3 || count > 5)
            //{
            //    ID = 0;
            //}


            int VendCount = 0, RecCount = 0;
            var distinctVCodes = deserializedData.Select(x => x.MfgCd).Distinct();
            foreach (var VCode in distinctVCodes)
            {
                VendCount = VendCount + 1;
                RecCount = Convert.ToInt32(deserializedData.Count);
            }

            if (VendCount > 3 || RecCount > 5)
            {
                ID = 0;
            }
            else
            {
                DateTime CDate = DateTime.Now;
                if (model.PlanDt == Convert.ToDateTime(CDate).ToString("dd/MM/yyyy"))
                {
                    if (CDate.Hour > model.PlanDHours)
                    {
                        ID = 1;
                    }
                    else
                    {
                        foreach (var details in deserializedData)
                        {
                            model.CaseNo = details.CaseNo;
                            model.CallRecvDt = details.CallRecvDt;
                            model.CallSno = details.CallSno;

                            var approvalCount = context.T47IeWorkPlans.Count(x => x.VisitDt.Date == DateTime.Now.Date && x.IeCd == GetIeCd);
                            var T17 = context.T17CallRegisters.Where(x => x.CaseNo == details.CaseNo && x.CallRecvDt == details.CallRecvDt && x.CallSno == details.CallSno).FirstOrDefault();
                            if (approvalCount < 5)
                            {
                                T17.CmApproval = "A";
                                T17.CmApprovalDt = DateTime.Now.Date;
                                context.SaveChanges();
                            }
                            var query = (from t17 in context.T17CallRegisters
                                         join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                         join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                         where t17.CaseNo.StartsWith(Region) && t17.IeCd == GetIeCd
                                         && t17.CaseNo == details.CaseNo && t17.CallRecvDt == details.CallRecvDt && t17.CallSno == details.CallSno
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
                                if (details.CaseNo != null && details.CallRecvDt != null && details.CallSno > 0)
                                {
                                    T47IeWorkPlan obj = new T47IeWorkPlan();
                                    obj.IeCd = query.IeCd;
                                    obj.CoCd = Convert.ToByte(query.CoCd);
                                    obj.VisitDt = Convert.ToDateTime(model.PlanDt);
                                    obj.CaseNo = query.CaseNo;
                                    obj.CallRecvDt = query.CallRecvDt;
                                    obj.CallSno = query.CallSno;
                                    obj.MfgCd = query.MfgCd;
                                    obj.MfgPlace = query.MfgPlace;
                                    obj.RegionCode = Region;
                                    obj.UserId = model.Createdby;
                                    obj.Datetime = DateTime.Now;

                                    context.T47IeWorkPlans.Add(obj);
                                    context.SaveChanges();
                                    ID = 2;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var details in deserializedData)
                    {
                        model.CaseNo = details.CaseNo;
                        model.CallRecvDt = details.CallRecvDt;
                        model.CallSno = details.CallSno;

                        var approvalCount = context.T47IeWorkPlans.Count(x => x.VisitDt.Date == DateTime.Now.Date && x.IeCd == GetIeCd);
                        var T17 = context.T17CallRegisters.Where(x => x.CaseNo == details.CaseNo && x.CallRecvDt == details.CallRecvDt && x.CallSno == details.CallSno).FirstOrDefault();
                        if (approvalCount < 5)
                        {
                            T17.CmApproval = "A";
                            T17.CmApprovalDt = DateTime.Now.Date;
                            context.SaveChanges();
                        }

                        var query = (from t17 in context.T17CallRegisters
                                     join t05 in context.T05Vendors on t17.MfgCd equals t05.VendCd
                                     join t03 in context.T03Cities on t05.VendCityCd equals t03.CityCd
                                     where t17.CaseNo.StartsWith(Region) && t17.IeCd == GetIeCd
                                     && t17.CaseNo == details.CaseNo && t17.CallRecvDt == details.CallRecvDt && t17.CallSno == details.CallSno
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
                            if (details.CaseNo != null && details.CallRecvDt != null && details.CallSno > 0)
                            {
                                T47IeWorkPlan obj = new T47IeWorkPlan();
                                obj.IeCd = query.IeCd;
                                obj.CoCd = Convert.ToByte(query.CoCd);
                                obj.VisitDt = Convert.ToDateTime(model.PlanDt);
                                obj.CaseNo = query.CaseNo;
                                obj.CallRecvDt = query.CallRecvDt;
                                obj.CallSno = query.CallSno;
                                obj.MfgCd = query.MfgCd;
                                obj.MfgPlace = query.MfgPlace;
                                obj.RegionCode = Region;
                                obj.UserId = model.Createdby;
                                obj.Datetime = DateTime.Now;

                                context.T47IeWorkPlans.Add(obj);
                                context.SaveChanges();
                                ID = 2;
                            }
                        }
                    }
                }
            }

            return ID;
        }

        public int DetailsDelete(DailyWorkPlanModel model, string Region, int GetIeCd)
        {
            int ID = 0;
            List<DeSerializeDailyWorkModel> deserializedData = JsonConvert.DeserializeObject<List<DeSerializeDailyWorkModel>>(model.checkedWork);
            foreach (var details in deserializedData)
            {
                model.CaseNo = details.CaseNo;
                model.CallRecvDt = details.CallRecvDt;
                model.CallSno = details.CallSno;

                var T47 = context.T47IeWorkPlans.Where(x => x.IeCd == GetIeCd && x.VisitDt == Convert.ToDateTime(model.PlanDt) && x.CaseNo == model.CaseNo && x.CallRecvDt == model.CallRecvDt && x.CallSno == model.CallSno).FirstOrDefault();
                if (T47 != null)
                {
                    context.T47IeWorkPlans.RemoveRange(T47);
                    context.SaveChanges();
                    ID = Convert.ToInt32(T47.CallSno);
                }
                var approvalCount = context.T47IeWorkPlans.Count(x => x.VisitDt.Date == DateTime.Now.Date && x.IeCd == GetIeCd);
                var T17 = context.T17CallRegisters.Where(x => x.CaseNo == details.CaseNo && x.CallRecvDt == details.CallRecvDt && x.CallSno == details.CallSno).FirstOrDefault();
                if (T17 != null)
                {
                    T17.CmApproval = null;
                    T17.CmApprovalDt = null;
                    context.SaveChanges();
                }
            }
            return ID;
        }

        public int NonInspectionSave(DailyWorkPlanModel model, string Region, int GetIeCd)
        {
            int ID = 0;
            int co_cd = 0;
            var T09 = context.T09Ies.Where(x => x.IeCd == GetIeCd).FirstOrDefault();
            if (T09 != null)
            {
                co_cd = Convert.ToInt32(T09.IeCoCd);
            }

            DateTime startDate = Convert.ToDateTime(model.FromDt);
            DateTime endDate = Convert.ToDateTime(model.ToDt);
            int daysDifference = (int)(endDate - startDate).TotalDays;
            var result = Enumerable.Range(0, daysDifference + 1)
                .Select(offset => startDate.AddDays(offset))
                .Select(date => date.ToString("dd/MM/yyyy"));

            foreach (var wkDt in result)
            {
                var Exist = context.T48NiIeWorkPlans.Where(x => x.IeCd == GetIeCd && x.CoCd == Convert.ToByte(co_cd) && x.NiWorkCd == model.NIWorkType && x.NiWorkDt == Convert.ToDateTime(wkDt)).FirstOrDefault();
                if (Exist == null)
                {
                    T48NiIeWorkPlan T48 = new();
                    T48.IeCd = GetIeCd;
                    T48.CoCd = Convert.ToByte(co_cd) == 0 ? null : Convert.ToByte(co_cd);
                    T48.NiWorkCd = model.NIWorkType;
                    T48.NiOtherDesc = model.OtherDesc;
                    T48.NiWorkDt = Convert.ToDateTime(wkDt);
                    T48.RegionCode = Region;
                    T48.UserId = model.UserId;
                    T48.Datetime = DateTime.Now.Date;

                    context.T48NiIeWorkPlans.Add(T48);
                    context.SaveChanges();
                    ID = Convert.ToInt32(T48.IeCd);
                }
            }
            return ID;
        }

        public DTResult<DailyWorkPlanModel> GetLoadTableNonInspection(DTParameters dtParameters, string Region, int GetIeCd)
        {

            DTResult<DailyWorkPlanModel> dTResult = new() { draw = 0 };
            IQueryable<DailyWorkPlanModel>? query = null;

            string PlanDt = "";
            if (!string.IsNullOrEmpty(dtParameters.AdditionalValues["PlanDt"]))
            {
                PlanDt = Convert.ToString(dtParameters.AdditionalValues["PlanDt"]);
            }

            query = from T48 in context.T48NiIeWorkPlans
                    where T48.IeCd == GetIeCd && T48.NiWorkDt == Convert.ToDateTime(PlanDt)
                    orderby T48.NiWorkDt
                    select new DailyWorkPlanModel
                    {
                        NIWorkType = T48.NiWorkCd == "T" ? "Training" :
                                          T48.NiWorkCd == "L" ? "Leave" :
                                          T48.NiWorkCd == "O" ? "Office" :
                                          T48.NiWorkCd == "J" ? "JI" :
                                          T48.NiWorkCd == "F" ? "Firm Visit" :
                                          "Others - " + T48.NiOtherDesc,
                        FromDt = T48.NiWorkDt
                    };

            dTResult.recordsTotal = query.Count();

            dTResult.recordsFiltered = query.Count();

            if (dtParameters.Length == -1) dtParameters.Length = query.Count();

            dTResult.data = query.Skip(dtParameters.Start).Take(dtParameters.Length).Select(p => p).ToList();

            dTResult.draw = dtParameters.Draw;

            return dTResult;
        }

        public string ReasonSave(DateTime? NwpDt, string Reason, int GetIeCd, string Region, string UserName)
        {
            string Dtl = "";
            int co_cd = 0;
            var T09 = context.T09Ies.Where(x => x.IeCd == GetIeCd).FirstOrDefault();
            if (T09 != null)
            {
                co_cd = Convert.ToInt32(T09.IeCoCd);
            }
            if (co_cd > 0)
            {
                NoIeWorkPlan plan = new NoIeWorkPlan();
                plan.IeCd = GetIeCd;
                plan.CoCd = Convert.ToByte(co_cd);
                plan.Reason = Reason;
                plan.NwpDt = Convert.ToDateTime(NwpDt);
                plan.RegionCode = Region;
                plan.UserId = UserName;
                plan.Datetime = DateTime.Now.Date;

                context.NoIeWorkPlans.Add(plan);
                context.SaveChanges();
                //Dtl = Convert.ToString(Convert.ToDateTime(NwpDt).AddDays());
                Dtl = Convert.ToDateTime(NwpDt).AddDays(1).ToString("dd/MM/yyyy");
            }
            return Dtl;
        }
    }
}
